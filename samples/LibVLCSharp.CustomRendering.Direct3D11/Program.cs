﻿using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Numerics;

using Windows.Win32.System.Threading;
using Windows.Win32.Graphics.Dxgi;
using Windows.Win32.Graphics.Dxgi.Common;
using Windows.Win32.Graphics.Direct3D11;
using Windows.Win32.Graphics.Direct3D;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Direct3D10;
using Windows.Win32.System.Com;
using static Windows.Win32.PInvoke;

using static LibVLCSharp.MediaPlayer;
using Windows.Win32;
using System.Runtime.CompilerServices;

namespace LibVLCSharp.CustomRendering.Direct3D11
{
    unsafe class Program
    {
        static Form form;
        static IDXGISwapChain* _swapchain;
        static ID3D11RenderTargetView* _swapchainRenderTarget;

        static ID3D11Device* _d3dDevice;
        static ID3D11DeviceContext* _d3dctx;

        const int WIDTH = 1500;
        const int HEIGHT = 900;

        static ID3D11Device* _d3deviceVLC;
        static ID3D11DeviceContext* _d3dctxVLC;

        static ID3D11Texture2D* _textureVLC;
        static ID3D11RenderTargetView* _textureRenderTarget;
        static HANDLE _sharedHandle;
        static ID3D11Texture2D* _texture;
        static ID3D11ShaderResourceView* _textureShaderInput;

        ///* our vertex/pixel shader */
        static ID3D11VertexShader* pVS;
        static ID3D11PixelShader* pPS;
        static ID3D11InputLayout* pShadersInputLayout;

        static ID3D11Buffer* pVertexBuffer;
        static int vertexBufferStride;

        static uint quadIndexCount;
        static ID3D11Buffer* pIndexBuffer;

        static RTL_CRITICAL_SECTION sizeLock = new();

        static readonly float BORDER_LEFT = -0.95f;
        static readonly float BORDER_RIGHT = 0.85f;
        static readonly float BORDER_TOP = 0.95f;
        static readonly float BORDER_BOTTOM = -0.90f;

        static ID3D11SamplerState* samplerState;

        static LibVLC libvlc;
        static MediaPlayer mediaplayer;

        static ReportSizeChange reportSize;
        static IntPtr reportOpaque;

        static uint width, height;

        static void ThrowIfFailed(HRESULT hr)
        {
            if (hr.Failed)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
        }

        static void Main()
        {
            CreateWindow();

            InitializeDirect3D();

            InitializeLibVLC();

            Application.Run();
        }

        static void CreateWindow()
        {
            form = new Form() { Width = WIDTH, Height = HEIGHT, Text = typeof(Program).Namespace };
            form.Show();
            form.Resize += Form_Resize;
            form.FormClosing += Form_FormClosing;
        }

        static void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            mediaplayer?.Stop();
            mediaplayer?.Dispose();
            mediaplayer = null;

            libvlc?.Dispose();
            libvlc = null;

            Environment.Exit(0);
        }

        static void Form_Resize(object sender, EventArgs e)
        {
            width = Convert.ToUInt32(form.ClientRectangle.Width);
            height = Convert.ToUInt32(form.ClientRectangle.Height);

            fixed (RTL_CRITICAL_SECTION* sl = &sizeLock)
            {
                EnterCriticalSection(sl);

                reportSize?.Invoke(reportOpaque, width, height);

                LeaveCriticalSection(sl);
            }

            reportSize?.Invoke(reportOpaque, width, height);
        }

        static void InitializeDirect3D()
        {
            
            var desc = new DXGI_SWAP_CHAIN_DESC
            {
                BufferDesc = new DXGI_MODE_DESC
                {
                    Width = WIDTH,
                    Height = HEIGHT,
                    Format = DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_UNORM
                },
                SampleDesc = new DXGI_SAMPLE_DESC
                {
                    Count = 1
                },
                BufferCount = 1,
                Windowed = true,
                OutputWindow = new HWND(form.Handle),
                BufferUsage = DXGI_USAGE.DXGI_USAGE_RENDER_TARGET_OUTPUT,
                Flags = (uint)DXGI_SWAP_CHAIN_FLAG.DXGI_SWAP_CHAIN_FLAG_ALLOW_MODE_SWITCH
            };

            D3D11_CREATE_DEVICE_FLAG creationFlags = 0;
#if DEBUG
            creationFlags |= D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_DEBUG;
#endif
            fixed (IDXGISwapChain** swapchain = &_swapchain)
            fixed (ID3D11Device** device = &_d3dDevice)
            fixed (ID3D11DeviceContext** context = &_d3dctx)
            { 
                ThrowIfFailed(D3D11CreateDeviceAndSwapChain(null,
                    D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE,
                    HINSTANCE.Null,
                    creationFlags,
                    null,
                    0,
                    D3D11_SDK_VERSION,
                    &desc,
                    swapchain,
                    device,
                    null,
                    context));
            }
            var id3d10multithreadGuid = typeof(ID3D10Multithread).GUID;

            ID3D10Multithread* multithread = null;

            _d3dDevice->QueryInterface(&id3d10multithreadGuid, (void**)&multithread);
            multithread->SetMultithreadProtected(true);
            multithread->Release();


            var viewport = new D3D11_VIEWPORT
            {
                Height = HEIGHT,
                Width = WIDTH
            };

            _d3dctx->RSSetViewports(1, &viewport);

            fixed (ID3D11Device** device = &_d3deviceVLC)
            fixed (ID3D11DeviceContext** context = &_d3dctxVLC)
            {
                ThrowIfFailed(D3D11CreateDevice(null,
                      D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE,
                      HINSTANCE.Null,
                      creationFlags | D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_VIDEO_SUPPORT, /* needed for hardware decoding */
                      null, 0,
                      D3D11_SDK_VERSION,
                      device, null, context));
            }

            ID3D11Resource* backBuffer = null;
            D3D11_RENDER_TARGET_VIEW_DESC? renderTargetViewDesc = null;

            var texture2dGuid = typeof(ID3D11Texture2D).GUID;
            ThrowIfFailed(_swapchain->GetBuffer(0, (Guid*)Unsafe.AsPointer(ref texture2dGuid), (void**)&backBuffer));

            fixed (ID3D11RenderTargetView** swapchainRenderTarget = &_swapchainRenderTarget)
                ThrowIfFailed(_d3dDevice->CreateRenderTargetView(backBuffer, renderTargetViewDesc, swapchainRenderTarget));

            backBuffer->Release();

            fixed (ID3D11RenderTargetView** swapchainRenderTarget = &_swapchainRenderTarget)
                _d3dctx->OMSetRenderTargets(1, swapchainRenderTarget, null);

            ID3DBlob* VS, PS, pErrBlob;

            ID3DBlob* vertexShaderBlob = null;

            fixed (byte* shader = Encoding.ASCII.GetBytes(DefaultShaders.HLSL))
            {
                var result = D3DCompile(shader, (nuint)DefaultShaders.HLSL.Length, null, null, null, "VShader", "vs_4_0", 0, 0, &VS, &pErrBlob);
                if (result.Failed && pErrBlob != null)
                {
                    var errorMessage = Encoding.ASCII.GetString((byte*)pErrBlob->GetBufferPointer(), (int)pErrBlob->GetBufferSize());
                    Debug.WriteLine(errorMessage);
                    ThrowIfFailed(result);
                }

                result = D3DCompile(shader, (nuint)DefaultShaders.HLSL.Length, null, null, null, "PShader", "ps_4_0", 0, 0, &PS, &pErrBlob);
                if (result.Failed && pErrBlob != null)
                {
                    var errorMessage = Encoding.ASCII.GetString((byte*)pErrBlob->GetBufferPointer(), (int)pErrBlob->GetBufferSize());
                    Debug.WriteLine(errorMessage);
                    ThrowIfFailed(result);
                }
            }

            fixed (ID3D11VertexShader** vertexShader = &pVS)
            fixed (ID3D11PixelShader** pixelShader = &pPS)
            {
                ThrowIfFailed(_d3dDevice->CreateVertexShader(VS->GetBufferPointer(), VS->GetBufferSize(), null, vertexShader));
                ThrowIfFailed(_d3dDevice->CreatePixelShader(PS->GetBufferPointer(), PS->GetBufferSize(), null, pixelShader));
            }

            fixed (byte* position = Encoding.ASCII.GetBytes("POSITION"))
            fixed (byte* textcoord = Encoding.ASCII.GetBytes("TEXCOORD"))
            fixed (ID3D11InputLayout** shadersInputLayout = &pShadersInputLayout)
            {
                var inputElementDescs = stackalloc D3D11_INPUT_ELEMENT_DESC[2];
                {
                    inputElementDescs[0] = new D3D11_INPUT_ELEMENT_DESC
                    {
                        SemanticName = new PCSTR(position),
                        SemanticIndex = 0,
                        Format = DXGI_FORMAT.DXGI_FORMAT_R32G32B32_FLOAT,
                        InputSlot = 0,
                        AlignedByteOffset = D3D11_APPEND_ALIGNED_ELEMENT,
                        InputSlotClass = D3D11_INPUT_CLASSIFICATION.D3D11_INPUT_PER_VERTEX_DATA,
                        InstanceDataStepRate = 0
                    };

                    inputElementDescs[1] = new D3D11_INPUT_ELEMENT_DESC
                    {
                        SemanticName = new PCSTR(textcoord),
                        SemanticIndex = 0,
                        Format = DXGI_FORMAT.DXGI_FORMAT_R32G32_FLOAT,
                        InputSlot = 0,
                        AlignedByteOffset = D3D11_APPEND_ALIGNED_ELEMENT,
                        InputSlotClass = D3D11_INPUT_CLASSIFICATION.D3D11_INPUT_PER_VERTEX_DATA,
                        InstanceDataStepRate = 0
                    };
                }

                ThrowIfFailed(_d3dDevice->CreateInputLayout(inputElementDescs, 2, VS->GetBufferPointer(), VS->GetBufferSize(), shadersInputLayout));
            }

            var ourVerticles = new ShaderInput[4];

            ourVerticles[0] = new ShaderInput
            {
                position = new Position
                {
                    x = BORDER_LEFT,
                    y = BORDER_BOTTOM,
                    z = 0.0f
                },
                texture = new Texture { x = 0.0f, y = 1.0f }
            };

            ourVerticles[1] = new ShaderInput
            {
                position = new Position
                {
                    x = BORDER_RIGHT,
                    y = BORDER_BOTTOM,
                    z = 0.0f
                },
                texture = new Texture { x = 1.0f, y = 1.0f }
            };

            ourVerticles[2] = new ShaderInput
            {
                position = new Position
                {
                    x = BORDER_RIGHT,
                    y = BORDER_TOP,
                    z = 0.0f
                },
                texture = new Texture { x = 1.0f, y = 0.0f }
            };

            ourVerticles[3] = new ShaderInput
            {
                position = new Position
                {
                    x = BORDER_LEFT,
                    y = BORDER_TOP,
                    z = 0.0f
                },
                texture = new Texture { x = 0.0f, y = 0.0f }
            };

            var verticlesSize = (uint)sizeof(ShaderInput) * 4;

            var bd = new D3D11_BUFFER_DESC
            {
                Usage = D3D11_USAGE.D3D11_USAGE_DYNAMIC,
                ByteWidth = verticlesSize,
                BindFlags = D3D11_BIND_FLAG.D3D11_BIND_VERTEX_BUFFER,
                CPUAccessFlags = D3D11_CPU_ACCESS_FLAG.D3D11_CPU_ACCESS_WRITE
            };

            pVertexBuffer = CreateBuffer(bd);
            vertexBufferStride = Marshal.SizeOf(ourVerticles[0]);

            D3D11_MAPPED_SUBRESOURCE ms;

            ID3D11Resource* res;
            var resourceGuid = typeof(ID3D11Resource).GUID;

            pVertexBuffer->QueryInterface(&resourceGuid, (void**)&res);

            ThrowIfFailed(_d3dctx->Map(res, 0, D3D11_MAP.D3D11_MAP_WRITE_DISCARD, 0, &ms));
            for (var i = 0; i < ourVerticles.Length; i++)
            {
                Marshal.StructureToPtr(ourVerticles[i], (IntPtr)ms.pData + (i * vertexBufferStride), false);
            }
            _d3dctx->Unmap(res, 0);

            quadIndexCount = 6;

            var bufferDesc = new D3D11_BUFFER_DESC
            {
                Usage = D3D11_USAGE.D3D11_USAGE_DYNAMIC,
                ByteWidth = sizeof(ushort) * quadIndexCount,
                BindFlags = D3D11_BIND_FLAG.D3D11_BIND_INDEX_BUFFER,
                CPUAccessFlags = D3D11_CPU_ACCESS_FLAG.D3D11_CPU_ACCESS_WRITE
            };

            pIndexBuffer = CreateBuffer(bufferDesc);

            ThrowIfFailed(pIndexBuffer->QueryInterface(&resourceGuid, (void**)&res));

            ThrowIfFailed(_d3dctx->Map(res, 0, D3D11_MAP.D3D11_MAP_WRITE_DISCARD, 0, &ms));
            Marshal.WriteInt16((IntPtr)ms.pData, 0 * sizeof(ushort), 3);
            Marshal.WriteInt16((IntPtr)ms.pData, 1 * sizeof(ushort), 1);
            Marshal.WriteInt16((IntPtr)ms.pData, 2 * sizeof(ushort), 0);
            Marshal.WriteInt16((IntPtr)ms.pData, 3 * sizeof(ushort), 2);
            Marshal.WriteInt16((IntPtr)ms.pData, 4 * sizeof(ushort), 1);
            Marshal.WriteInt16((IntPtr)ms.pData, 5 * sizeof(ushort), 3);

            _d3dctx->Unmap(res, 0);

            _d3dctx->IASetPrimitiveTopology(D3D_PRIMITIVE_TOPOLOGY.D3D10_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
            _d3dctx->IASetInputLayout(pShadersInputLayout);
            uint offset = 0;

            var vv = (uint)vertexBufferStride;
            fixed (ID3D11Buffer** buffer = &pVertexBuffer)
                _d3dctx->IASetVertexBuffers(0, 1, buffer, &vv, &offset);

            _d3dctx->IASetIndexBuffer(pIndexBuffer, DXGI_FORMAT.DXGI_FORMAT_R16_UINT, 0);

            _d3dctx->VSSetShader(pVS, null, 0);
            _d3dctx->PSSetShader(pPS, null, 0);

            var samplerDesc = new D3D11_SAMPLER_DESC
            {
                Filter = D3D11_FILTER.D3D11_FILTER_MIN_MAG_MIP_POINT,
                AddressU = D3D11_TEXTURE_ADDRESS_MODE.D3D11_TEXTURE_ADDRESS_CLAMP,
                AddressV = D3D11_TEXTURE_ADDRESS_MODE.D3D11_TEXTURE_ADDRESS_CLAMP,
                AddressW = D3D11_TEXTURE_ADDRESS_MODE.D3D11_TEXTURE_ADDRESS_CLAMP,
                ComparisonFunc = D3D11_COMPARISON_FUNC.D3D11_COMPARISON_ALWAYS,
                MinLOD = 0,
                MaxLOD = D3D11_FLOAT32_MAX
            };

            fixed (ID3D11SamplerState** ss = &samplerState)
            {
                ThrowIfFailed(_d3dDevice->CreateSamplerState(&samplerDesc, ss));
                _d3dctx->PSSetSamplers(0, 1, ss);
            }

            fixed (RTL_CRITICAL_SECTION* sl = &sizeLock)
                InitializeCriticalSection(sl);
        }

        static void InitializeLibVLC()
        {
            libvlc = new LibVLC(enableDebugLogs: true);
            libvlc.Log += (s, e) => Debug.WriteLine(e.FormattedLog);

            mediaplayer = new MediaPlayer(libvlc);

            using var media = new Media(new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4"));
            mediaplayer.Media = media;

            mediaplayer.SetOutputCallbacks(VideoEngine.D3D11, OutputSetup, OutputCleanup, OutputSetResize, UpdateOuput, Swap, StartRendering, null, null, SelectPlane);

            mediaplayer.Play();
        }

        static ID3D11Buffer* CreateBuffer(D3D11_BUFFER_DESC bd)
        {
            ID3D11Buffer* buffer;

            ThrowIfFailed(_d3dDevice->CreateBuffer(&bd, null, &buffer));

            return buffer;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct ShaderInput
        {
            internal Position position;
            internal Texture texture;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Position
        {
            internal float x;
            internal float y;
            internal float z;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Texture
        {
            internal float x;
            internal float y;
        }

        // libvlc

        static bool OutputSetup(ref IntPtr opaque, SetupDeviceConfig* config, ref SetupDeviceInfo setup)
        {
            setup.D3D11.DeviceContext = _d3dctxVLC;
            _d3dctxVLC->AddRef();
            return true;
        }

        static void OutputCleanup(IntPtr opaque)
        {
            // here we can release all things Direct3D11 for good (if playing only one file)
            _d3dctxVLC->Release();
        }

        static void OutputSetResize(IntPtr opaque, ReportSizeChange report_size_change, IntPtr report_opaque)
        {
            fixed (RTL_CRITICAL_SECTION* sl = &sizeLock)
            {
                EnterCriticalSection(sl);

                if (report_size_change != null && report_opaque != IntPtr.Zero)
                {
                    reportSize = report_size_change;
                    reportOpaque = report_opaque;
                    reportSize?.Invoke(reportOpaque, width, height);
                }

                LeaveCriticalSection(sl);
            }
        }

        static bool UpdateOuput(IntPtr opaque, RenderConfig* config, ref OutputConfig output)
        {
            ReleaseTextures();

            var renderFormat = DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_UNORM;

            var texDesc = new D3D11_TEXTURE2D_DESC
            {
                MipLevels = 1,
                SampleDesc = new DXGI_SAMPLE_DESC { Count = 1 },
                BindFlags = D3D11_BIND_FLAG.D3D11_BIND_RENDER_TARGET| D3D11_BIND_FLAG.D3D11_BIND_SHADER_RESOURCE,
                Usage = D3D11_USAGE.D3D11_USAGE_DEFAULT,
                CPUAccessFlags = 0,
                ArraySize = 1,
                Format = renderFormat,
                Height = config->Height,
                Width = config->Width,
                MiscFlags = D3D11_RESOURCE_MISC_FLAG.D3D11_RESOURCE_MISC_SHARED | D3D11_RESOURCE_MISC_FLAG.D3D11_RESOURCE_MISC_SHARED_NTHANDLE
            };

            fixed (ID3D11Texture2D** texture = &_texture)
                ThrowIfFailed(_d3dDevice->CreateTexture2D(&texDesc, null, texture));

            IDXGIResource1* sharedResource = null;
            var dxgiresource1Guid = typeof(IDXGIResource1).GUID;

            _texture->QueryInterface(&dxgiresource1Guid, (void**)&sharedResource);

            fixed (HANDLE* handle = &_sharedHandle)
                ThrowIfFailed(sharedResource->CreateSharedHandle(null, DXGI_SHARED_RESOURCE_READ | DXGI_SHARED_RESOURCE_WRITE, null, handle));
            sharedResource->Release();

            ID3D11Device1* d3d11VLC1;
            var d3d11device1Guid = typeof(ID3D11Device1).GUID;
            _d3deviceVLC->QueryInterface(&d3d11device1Guid, (void**)&d3d11VLC1);

            var texture2dGuid = typeof(ID3D11Texture2D).GUID;
            fixed (ID3D11Texture2D** texture = &_textureVLC)
                ThrowIfFailed(d3d11VLC1->OpenSharedResource1(_sharedHandle, &texture2dGuid, (void**)texture));
            d3d11VLC1->Release();

            var shaderResourceViewDesc = new D3D11_SHADER_RESOURCE_VIEW_DESC
            {
                ViewDimension = D3D_SRV_DIMENSION.D3D10_1_SRV_DIMENSION_TEXTURE2D,
                Format = texDesc.Format,
            };

            shaderResourceViewDesc.Anonymous.Texture2D.MipLevels = 1;

            ID3D11Resource* res;
            var d3d11resourceGuid = typeof(ID3D11Resource).GUID;
            _texture->QueryInterface(&d3d11resourceGuid, (void**)&res);
            fixed (ID3D11ShaderResourceView** tsi = &_textureShaderInput)
            {
                ThrowIfFailed(_d3dDevice->CreateShaderResourceView(res, &shaderResourceViewDesc, tsi));
                res->Release();
                _d3dctx->PSSetShaderResources(0, 1, tsi);
            }

            var renderTargetViewDesc = new D3D11_RENDER_TARGET_VIEW_DESC
            {
                Format = texDesc.Format,
                ViewDimension = D3D11_RTV_DIMENSION.D3D11_RTV_DIMENSION_TEXTURE2D
            };

            _textureVLC->QueryInterface(&d3d11resourceGuid, (void**)&res);

            fixed(ID3D11RenderTargetView** trt = &_textureRenderTarget)
            {
                ThrowIfFailed(_d3deviceVLC->CreateRenderTargetView(res, &renderTargetViewDesc, trt));
                res->Release();
                _d3dctxVLC->OMSetRenderTargets(1, trt, null);
            }

            output.Union.DxgiFormat = (int)renderFormat;
            output.FullRange = true;
            output.ColorSpace = ColorSpace.BT709;
            output.ColorPrimaries = ColorPrimaries.BT709;
            output.TransferFunction = TransferFunction.SRGB;
            output.Orientation = VideoOrientation.TopLeft;

            return true;
        }

        static void ReleaseTextures()
        {
            if (_sharedHandle != HANDLE.Null)
            {
                CloseHandle(_sharedHandle);
                _sharedHandle = HANDLE.Null;
            }
            if (_textureVLC != null)
            {
                var count = _textureVLC->Release();
                Debug.Assert(count == 0);
                _textureVLC = null;
            }
            if (_textureShaderInput != null)
            {
                var count = _textureShaderInput->Release();
                Debug.Assert(count == 0);
                _textureShaderInput = null;
            }
            if (_textureRenderTarget != null)
            {
                var count = _textureRenderTarget->Release();
                Debug.Assert(count == 0);
                _textureRenderTarget = null;
            }
            if (_texture != null)
            {
                var count = _texture->Release();
                Debug.Assert(count == 0);
                _texture = null;
            }
        }

        static void Swap(IntPtr opaque)
        {
            _swapchain->Present(0, 0);
        }

        static bool StartRendering(IntPtr opaque, bool enter)
        {
            if(enter)
            {
                // DEBUG: draw greenish background to show where libvlc doesn't draw in the texture
                // Normally you should Clear with a black background
                var greenRGBA = new Vector4(0.5f, 0.5f, 0.0f, 1.0f);
                //var blackRGBA = new Vector4(0, 0, 0, 1);

                _d3dctxVLC->ClearRenderTargetView(_textureRenderTarget, (float*)&greenRGBA);
            }
            else
            {
                var orangeRGBA = new Vector4(1.0f, 0.5f, 0.0f, 1.0f);
                _d3dctx->ClearRenderTargetView(_swapchainRenderTarget, (float*)&orangeRGBA);
                // Render into the swapchain
                // We start the drawing of the shared texture in our app as early as possible
                // in hope it's done as soon as Swap_cb is called
                _d3dctx->DrawIndexed(quadIndexCount, 0, 0);
            }
            return true;
        }

        static unsafe bool SelectPlane(IntPtr opaque, UIntPtr plane, void* output)
        {
            if ((ulong)plane != 0)
                return false;
            return true;
        }        
    }
}