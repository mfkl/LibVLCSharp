﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using LibVLCSharp.Shared.Helpers;

namespace LibVLCSharp.Shared
{
    /// <summary>
    /// The Core class handles libvlc loading intricacies on various platforms as well as
    /// the libvlc/libvlcsharp version match check.
    /// </summary>
    public static partial class Core
    {
        partial struct Native
        {
#if !UWP10_0 && !NETSTANDARD1_1
            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_get_version")]
            internal static extern IntPtr LibVLCVersion();
#endif
            [DllImport(Constants.Kernel32, SetLastError = true)]
            internal static extern IntPtr LoadLibrary(string dllToLoad);

            [DllImport(Constants.LibSystem, EntryPoint = "dlopen")]
            internal static extern IntPtr Dlopen(string libraryPath, int mode = 1);
        }

#if !UWP10_0 && !NETSTANDARD1_1
        /// <summary>
        /// Checks whether the major version of LibVLC and LibVLCSharp match <para/>
        /// Throws an NotSupportedException if the major versions mismatch
        /// </summary>
        static void EnsureVersionsMatch()
        {
            var libvlcMajorVersion = int.Parse(Native.LibVLCVersion().FromUtf8()?.Split('.').FirstOrDefault() ?? "0");
            var libvlcsharpMajorVersion = Assembly.GetExecutingAssembly().GetName().Version.Major;
            if (libvlcMajorVersion != libvlcsharpMajorVersion)
                throw new VLCException($"Version mismatch between LibVLC {libvlcMajorVersion} and LibVLCSharp {libvlcsharpMajorVersion}. " +
                    $"They must share the same major version number");
        }

        static List<(string libvlccore, string libvlc)> ComputeLibVLCSearchPaths()
        {
            var paths = new List<(string, string)>();
            string arch;

            if (PlatformHelper.IsMac)
            {
                arch = ArchitectureNames.MacOS64;
            }
            else
            {
                arch = PlatformHelper.IsX64BitProcess ? ArchitectureNames.Win64 : ArchitectureNames.Win86;
            }

            var libvlcDirPath1 = Path.Combine(Path.GetDirectoryName(typeof(LibVLC).Assembly.Location),
                Constants.LibrariesRepositoryFolderName, arch);

            var libvlccorePath1 = string.Empty;
            if (PlatformHelper.IsWindows)
            {
                libvlccorePath1 = LibVLCCorePath(libvlcDirPath1);
            }
            var libvlcPath1 = LibVLCPath(libvlcDirPath1);
            paths.Add((libvlccorePath1, libvlcPath1));

            var assemblyLocation = Assembly.GetEntryAssembly()?.Location ?? Assembly.GetExecutingAssembly()?.Location;

            var libvlcDirPath2 = Path.Combine(Path.GetDirectoryName(assemblyLocation),
                Constants.LibrariesRepositoryFolderName, arch);

            var libvlccorePath2 = string.Empty;
            if (PlatformHelper.IsWindows)
            {
                libvlccorePath2 = LibVLCCorePath(libvlcDirPath2);
            }

            var libvlcPath2 = LibVLCPath(libvlcDirPath2);
            paths.Add((libvlccorePath2, libvlcPath2));

            var libvlcPath3 = LibVLCPath(Path.GetDirectoryName(typeof(LibVLC).Assembly.Location));

            paths.Add((string.Empty, libvlcPath3));

            if (PlatformHelper.IsMac)
            {
                var libvlcPath4 = Path.Combine(Path.Combine(Path.GetDirectoryName(typeof(LibVLC).Assembly.Location), "lib"), $"libvlc{LibraryExtension}");
                var libvlccorePath4 = LibVLCCorePath(Path.Combine(Path.GetDirectoryName(typeof(LibVLC).Assembly.Location), "lib"));
                paths.Add((libvlccorePath4, libvlcPath4));
            }

            return paths;
        }
#endif
        static string LibVLCPath(string dir) => Path.Combine(dir, $"{Constants.LibraryName}{LibraryExtension}");
        static string LibVLCCorePath(string dir) => Path.Combine(dir, $"{Constants.CoreLibraryName}{LibraryExtension}");
        static string LibraryExtension => PlatformHelper.IsWindows ? Constants.WindowsLibraryExtension : Constants.MacLibraryExtension;
        static void Log(string message)
        {
#if !UWP10_0 && !NETSTANDARD1_1
            Trace.WriteLine(message);
#else
            Debug.WriteLine(message);
#endif
        }
#if MAC || NETFRAMEWORK || NETSTANDARD
        static bool Loaded => LibvlcHandle != IntPtr.Zero;
#endif
        static bool LoadNativeLibrary(string nativeLibraryPath, out IntPtr handle)
        {
            handle = IntPtr.Zero;
            Log($"Loading {nativeLibraryPath}");

#if !NETSTANDARD1_1
            if (!File.Exists(nativeLibraryPath))
            {
                Log($"Cannot find {nativeLibraryPath}");
                return false;
            }
#endif
            if (PlatformHelper.IsMac)
            {
                handle = Native.Dlopen(nativeLibraryPath);
            }
            else
            {
                handle = Native.LoadLibrary(nativeLibraryPath);
            }

            return handle != IntPtr.Zero;
        }
    }
}
