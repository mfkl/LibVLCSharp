﻿using LibVLCSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace LibVLCSharp
{
    /// <summary>
    /// Media is an abstract representation of a playable media. It can be a network stream or a local video/audio file.
    /// </summary>
    public class Media : Internal
    {
        internal struct Native
        {
            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_new_location")]
            internal static extern IntPtr LibVLCMediaNewLocation(IntPtr mrl);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_new_path")]
            internal static extern IntPtr LibVLCMediaNewPath(IntPtr path);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_new_as_node")]
            internal static extern IntPtr LibVLCMediaNewAsNode(IntPtr name);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_new_fd")]
            internal static extern IntPtr LibVLCMediaNewFd(int fd);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_release")]
            internal static extern void LibVLCMediaRelease(IntPtr media);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_list_media")]
            internal static extern IntPtr LibVLCMediaListMedia(IntPtr mediaList);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_new_callbacks")]
            internal static extern IntPtr LibVLCMediaNewCallbacks(InternalOpenMedia openCb, InternalReadMedia readCb,
                InternalSeekMedia? seekCb, InternalCloseMedia closeCb, IntPtr opaque);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_add_option")]
            internal static extern void LibVLCMediaAddOption(IntPtr media, IntPtr option);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_add_option_flag")]
            internal static extern void LibVLCMediaAddOptionFlag(IntPtr media, IntPtr options, uint flags);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_get_mrl")]
            internal static extern IntPtr LibVLCMediaGetMrl(IntPtr media);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_duplicate")]
            internal static extern IntPtr LibVLCMediaDuplicate(IntPtr media);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_get_meta")]
            internal static extern IntPtr LibVLCMediaGetMeta(IntPtr media, MetadataType metadataType);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_set_meta")]
            internal static extern void LibVLCMediaSetMeta(IntPtr media, MetadataType metadataType, IntPtr value);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_save_meta")]
            internal static extern int LibVLCMediaSaveMeta(IntPtr libvlc, IntPtr media);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_event_manager")]
            internal static extern IntPtr LibVLCMediaEventManager(IntPtr media);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_get_stats")]
            internal static extern bool LibVLCMediaGetStats(IntPtr media, out MediaStats statistics);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_get_filestat")]
            internal static extern int LibVLCMediaGetFileStat(IntPtr media, FileStat type, out ulong value);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_get_duration")]
            internal static extern long LibVLCMediaGetDuration(IntPtr media);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_parse_request")]
            internal static extern int LibVLCMediaParseRequest(IntPtr libvlc, IntPtr media, MediaParseOptions mediaParseOptions, int timeout);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_get_parsed_status")]
            internal static extern MediaParsedStatus LibVLCMediaGetParsedStatus(IntPtr media);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_parse_stop")]
            internal static extern void LibVLCMediaParseStop(IntPtr libvlc, IntPtr media);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_set_user_data")]
            internal static extern void LibVLCMediaSetUserData(IntPtr media, IntPtr userData);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_get_user_data")]
            internal static extern IntPtr LibVLCMediaGetUserData(IntPtr media);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_get_tracklist")]
            internal static extern IntPtr LibVLCMediaGetTracklist(IntPtr media, TrackType trackType);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_subitems")]
            internal static extern IntPtr LibVLCMediaSubitems(IntPtr media);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_get_type")]
            internal static extern MediaType LibVLCMediaGetType(IntPtr media);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_slaves_add")]
            internal static extern int LibVLCMediaAddSlaves(IntPtr media, MediaSlaveType slaveType, uint priority, IntPtr uri);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_slaves_clear")]
            internal static extern void LibVLCMediaClearSlaves(IntPtr media);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_slaves_get")]
            internal static extern uint LibVLCMediaGetSlaves(IntPtr media, out IntPtr slaves);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_slaves_release")]
            internal static extern void LibVLCMediaReleaseSlaves(IntPtr slaves, uint count);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_retain")]
            internal static extern void LibVLCMediaRetain(IntPtr media);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_get_codec_description")]
            internal static extern IntPtr LibVLCMediaGetCodecDescription(TrackType type, uint codec);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_thumbnail_request_by_time")]
            internal static extern IntPtr LibVLCMediaThumbnailRequestByTime(IntPtr libvlc, IntPtr media, long time, ThumbnailerSeekSpeed speed,
                uint width, uint height, bool crop, PictureType pictureType, long timeout);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_thumbnail_request_by_pos")]
            internal static extern IntPtr LibVLCMediaThumbnailRequestByPosition(IntPtr libvlc, IntPtr media, double position, ThumbnailerSeekSpeed speed,
                uint width, uint height, bool crop, PictureType pictureType, long timeout);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_thumbnail_request_destroy")]
            internal static extern IntPtr LibVLCMediaThumbnailRequestDestroy(IntPtr thumbnailRequest);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_get_meta_extra")]
            internal static extern IntPtr LibVLCMediaGetMetaExtra(IntPtr media, IntPtr name);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_set_meta_extra")]
            internal static extern void LibVLCMediaSetMetaExtra(IntPtr media, IntPtr name, IntPtr value);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_get_meta_extra_names")]
            internal static extern uint LibVLCMediaGetMetaExtraNames(IntPtr media, out IntPtr names);

            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_media_meta_extra_names_release")]
            internal static extern void LibVLCMediaMetaExtraNamesRelease(IntPtr names, uint count);
        }

        Media(Func<IntPtr> create, Action<IntPtr> release, params string[] options)
            : base(create, release)
        {
            if(options == null) return;

            foreach(var optionUtf8 in options.ToUtf8())
                if(optionUtf8 != IntPtr.Zero)
                    MarshalUtils.PerformInteropAndFree(() => Native.LibVLCMediaAddOption(NativeReference, optionUtf8), optionUtf8);
        }

        /// <summary>
        /// Media Constructs a libvlc Media instance
        /// </summary>
        /// <param name="mrl">A path, location, or node name, depending on the 3rd parameter</param>
        /// <param name="type">The type of the 2nd argument.</param>
        /// <param name="options">the libvlc options, in the form of ":your-option"</param>
        public Media(string mrl, FromType type = FromType.FromPath, params string[] options)
            : this(() => SelectNativeCtor(mrl, type), Native.LibVLCMediaRelease, options)
        {
        }

        /// <summary>
        /// Media Constructs a libvlc Media instance
        /// </summary>
        /// <param name="uri">The absolute URI of the resource.</param>
        /// <param name="options">the libvlc options, in the form of ":your-option"</param>
        public Media(Uri uri, params string[] options)
            : this(() => SelectNativeCtor(uri?.AbsoluteUri ?? string.Empty, FromType.FromLocation),
                  Native.LibVLCMediaRelease,
                  options)
        {
        }

        /// <summary>
        /// Create a media for an already open file descriptor.
        /// The file descriptor shall be open for reading(or reading and writing).
        ///
        /// Regular file descriptors, pipe read descriptors and character device
        /// descriptors(including TTYs) are supported on all platforms.
        /// Block device descriptors are supported where available.
        /// Directory descriptors are supported on systems that provide fdopendir().
        /// Sockets are supported on all platforms where they are file descriptors,
        /// i.e.all except Windows.
        ///
        /// \note This library will <b>not</b> automatically close the file descriptor
        /// under any circumstance.Nevertheless, a file descriptor can usually only be
        /// rendered once in a media player.To render it a second time, the file
        /// descriptor should probably be rewound to the beginning with lseek().
        /// </summary>
        /// <param name="fd">open file descriptor</param>
        /// <param name="options">the libvlc options, in the form of ":your-option"</param>
        public Media(int fd, params string[] options)
            : this(() => Native.LibVLCMediaNewFd(fd), Native.LibVLCMediaRelease, options)
        {
        }

        /// <summary>
        /// Create a media from a media list
        /// </summary>
        /// <param name="mediaList">media list to create media from</param>
        public Media(MediaList mediaList)
            : base(() => Native.LibVLCMediaListMedia(mediaList.NativeReference), Native.LibVLCMediaRelease)
        {
        }

        /// <summary>
        /// Create a media from a MediaInput
        /// requires libvlc 3.0 or higher
        /// </summary>
        /// <param name="input">the media to be used by libvlc. LibVLCSharp will NOT dispose or close it.
        /// Use <see cref="StreamMediaInput"/> or implement your own.</param>
        /// <param name="options">the libvlc options, in the form of ":your-option"</param>
        public Media(MediaInput input, params string[] options)
            : this(() => CtorFromInput(input), Native.LibVLCMediaRelease, options)
        {
        }

        internal Media(IntPtr mediaPtr)
            : base(() => mediaPtr, Native.LibVLCMediaRelease)
        {
        }

        static IntPtr SelectNativeCtor(string mrl, FromType type)
        {
            if (string.IsNullOrEmpty(mrl))
                throw new ArgumentNullException(nameof(mrl));

            if(PlatformHelper.IsWindows && type == FromType.FromPath)
            {
                mrl = mrl.Replace("/", @"\");
            }

            var mrlPtr = mrl.ToUtf8();
            if (mrlPtr == IntPtr.Zero)
                throw new ArgumentException($"error marshalling {mrl} to UTF-8 for native interop");

            IntPtr result;
            switch (type)
            {
                case FromType.FromLocation:
                    result = Native.LibVLCMediaNewLocation(mrlPtr);
                    break;
                case FromType.FromPath:
                    result = Native.LibVLCMediaNewPath(mrlPtr);
                    break;
                case FromType.AsNode:
                    result = Native.LibVLCMediaNewAsNode(mrlPtr);
                    break;
                default:
                    result = IntPtr.Zero;
                    break;
            }

            Marshal.FreeHGlobal(mrlPtr);

            return result;
        }

        static IntPtr CtorFromInput(MediaInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return Native.LibVLCMediaNewCallbacks(OpenMediaCallbackHandle,
                ReadMediaCallbackHandle,
                input.CanSeek ? SeekMediaCallbackHandle : null,
                CloseMediaCallbackHandle,
                GCHandle.ToIntPtr(input.GcHandle));
        }

        /// <summary>Add an option to the media.
        /// <example>
        /// <code>
        /// // example <br/>
        /// media.AddOption(":no-audio");
        /// </code>
        /// </example></summary>
        /// <param name="option">the media option, in the form of ":your-option"</param>
        /// <remarks>
        /// <para>This option will be used to determine how the media_player will</para>
        /// <para>read the media. This allows to use VLC's advanced</para>
        /// <para>reading/streaming options on a per-media basis.</para>
        /// <para>The options are listed in 'vlc --long-help' from the command line,</para>
        /// <para>e.g. &quot;-sout-all&quot;. Keep in mind that available options and their semantics</para>
        /// <para>vary across LibVLC versions and builds.</para>
        /// <para>Not all options affects libvlc_media_t objects:</para>
        /// <para>Specifically, due to architectural issues most audio and video options,</para>
        /// <para>such as text renderer options, have no effects on an individual media.</para>
        /// <para>These options must be set through libvlc_new() instead.</para>
        /// </remarks>
        public void AddOption(string option)
        {
            if(string.IsNullOrEmpty(option)) throw new ArgumentNullException(nameof(option));

            var optionUtf8 = option.ToUtf8();
            MarshalUtils.PerformInteropAndFree(() => Native.LibVLCMediaAddOption(NativeReference, optionUtf8), optionUtf8);
        }

        /// <summary>
        /// Convenience method for crossplatform media configuration
        /// </summary>
        /// <param name="mediaConfiguration">mediaConfiguration translate to strings parsed by the vlc engine, some are platform specific</param>
        public void AddOption(MediaConfiguration mediaConfiguration)
        {
            if (mediaConfiguration == null) throw new ArgumentNullException(nameof(mediaConfiguration));

            foreach(var option in mediaConfiguration.Build())
            {
                AddOption(option);
            }
        }

        /// <summary>Add an option to the media with configurable flags.</summary>
        /// <param name="option">the media option</param>
        /// <param name="flags">the flags for this option</param>
        /// <remarks>
        /// <para>This option will be used to determine how the media_player will</para>
        /// <para>read the media. This allows to use VLC's advanced</para>
        /// <para>reading/streaming options on a per-media basis.</para>
        /// <para>The options are detailed in vlc --long-help, for instance</para>
        /// <para>&quot;--sout-all&quot;. Note that all options are not usable on medias:</para>
        /// <para>specifically, due to architectural issues, video-related options</para>
        /// <para>such as text renderer options cannot be set on a single media. They</para>
        /// <para>must be set on the whole libvlc instance instead.</para>
        /// </remarks>
        public void AddOptionFlag(string option, uint flags)
        {
            if (string.IsNullOrEmpty(option)) throw new ArgumentNullException(nameof(option));

            var optionUtf8 = option.ToUtf8();

            MarshalUtils.PerformInteropAndFree(() => Native.LibVLCMediaAddOptionFlag(NativeReference, optionUtf8, flags), optionUtf8);
        }

        string? _mrl;
        /// <summary>Get the media resource locator (mrl) from a media descriptor object</summary>
        public string Mrl
        {
            get
            {
                if (string.IsNullOrEmpty(_mrl))
                {
                    var mrlPtr = Native.LibVLCMediaGetMrl(NativeReference);
                    _mrl = mrlPtr.FromUtf8(libvlcFree: true);
                }
                return _mrl!;
            }
        }

        /// <summary>Duplicate a media descriptor object.</summary>
        public Media Duplicate()
        {
            var duplicatePtr = Native.LibVLCMediaDuplicate(NativeReference);
            if(duplicatePtr == IntPtr.Zero) throw new Exception("Failure to duplicate");
            return new Media(duplicatePtr);
        }

        /// <summary>Read the meta of the media.</summary>
        /// <param name="metadataType">the meta to read</param>
        /// <returns>the media's meta</returns>
        /// <remarks>
        /// If the media has not yet been parsed this will return NULL.
        /// </remarks>
        public string? Meta(MetadataType metadataType)
        {
            var metaPtr = Native.LibVLCMediaGetMeta(NativeReference, metadataType);
            return metaPtr.FromUtf8(libvlcFree: true);
        }

        /// <summary>
        /// Read the meta extra of the media.
        /// </summary>
        /// <remarks>If the media has not yet been parsed, this will return null.</remarks>
        /// <param name="name">the meta extra to read</param>
        /// <returns>the media's meta extra or null</returns>
        public string? MetaExtra(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var metaExtraPtr = Native.LibVLCMediaGetMetaExtra(NativeReference, name.ToUtf8());
            return metaExtraPtr.FromUtf8(true);
        }

        /// <summary>
        /// Set the meta extra of the media
        /// </summary>
        /// <remarks>This function will not save the meta, call <see cref="SaveMeta(LibVLC)"/> in order to save the meta</remarks>
        /// <param name="name">the meta extra name to write</param>
        /// <param name="value">the meta extra value to write</param>
        public void SetMetaExtra(string name, string? value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Native.LibVLCMediaSetMetaExtra(NativeReference, name.ToUtf8(), value.ToUtf8());
        }

        /// <summary>
        /// Read the meta extra names of the media.
        /// </summary>
        public string?[] MetaExtraNames
        {
            get
            {
                return MarshalUtils.Retrieve(NativeReference,
                    (IntPtr nativeRef, out IntPtr array) => Native.LibVLCMediaGetMetaExtraNames(nativeRef, out array),
                    p => p.FromUtf8(),
                    Native.LibVLCMediaMetaExtraNamesRelease);
            }
        }

        /// <summary>
        /// <para>Set the meta of the media (this function will not save the meta, call</para>
        /// <para>libvlc_media_save_meta in order to save the meta)</para>
        /// </summary>
        /// <param name="metadataType">the <see cref="MetadataType"/>  to write</param>
        /// <param name="metaValue">the media's meta</param>
        public void SetMeta(MetadataType metadataType, string metaValue)
        {
            if(string.IsNullOrEmpty(metaValue)) throw new ArgumentNullException(metaValue);

            var metaUtf8 = metaValue.ToUtf8();
            MarshalUtils.PerformInteropAndFree(() => Native.LibVLCMediaSetMeta(NativeReference, metadataType, metaUtf8), metaUtf8);
        }

        /// <summary>Save the meta previously set</summary>
        /// <param name="libvlc">libvlc instance</param>
        /// <returns>true if the write operation was successful</returns>
        public bool SaveMeta(LibVLC libvlc) => Native.LibVLCMediaSaveMeta(libvlc.NativeReference, NativeReference) != 0;

        /// <summary>
        /// Get information about the media file, such as size and modified timestamp
        /// </summary>
        /// <param name="type">the type of information</param>
        /// <param name="value">the returned value</param>
        /// <returns>returns false if error/not found, true otherwise</returns>
        public bool FileStat(FileStat type, out ulong value)
        {
            if (Native.LibVLCMediaGetFileStat(NativeReference, type, out value) == 1)
                return true;
            
            value = 0;
            return false;
        }

        /// <summary>Get the current statistics about the media
        /// structure that contain the statistics about the media
        /// </summary>
        public MediaStats Statistics => Native.LibVLCMediaGetStats(NativeReference, out var mediaStats)
            ? default : mediaStats;

        MediaEventManager? _eventManager;
        /// <summary>
        /// <para>Get event manager from media descriptor object.</para>
        /// <para>NOTE: this function doesn't increment reference counting.</para>
        /// </summary>
        /// <returns>event manager object</returns>
        MediaEventManager EventManager
        {
            get
            {
                if (_eventManager != null) return _eventManager;
                var eventManagerPtr = Native.LibVLCMediaEventManager(NativeReference);
                _eventManager = new MediaEventManager(eventManagerPtr);
                return _eventManager;
            }
        }

        /// <summary>Get duration (in ms) of media descriptor object item.</summary>
        /// <returns>duration of media item or -1 on error</returns>
        public long Duration => Native.LibVLCMediaGetDuration(NativeReference);

        /// <summary>
        /// Parse the media asynchronously with options.
        /// It uses a flag to specify parse options (see <see cref="MediaParseOptions"/>). All these flags can be combined. By default, the media is parsed only if it's a local file.
        /// <para/> Note: Parsing can be aborted with ParseStop().
        /// </summary>
        /// <param name="libvlc">LibVLC instance that is to parse the media</param>
        /// <param name="options">Parse options flags. They can be combined</param>
        /// <param name="timeout">maximum time allowed to preparse the media.
        /// <para/>If -1, the default "preparse-timeout" option will be used as a timeout.
        /// <para/>If 0, it will wait indefinitely. If > 0, the timeout will be used (in milliseconds).
        /// </param>
        /// <param name="cancellationToken">token to cancel the operation</param>
        /// <returns>the parse status of the media</returns>
        public Task<MediaParsedStatus> ParseAsync(LibVLC libvlc, MediaParseOptions options = MediaParseOptions.ParseLocal, int timeout = -1, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var tcs = new TaskCompletionSource<MediaParsedStatus>();

            var ctr = cancellationToken.Register(() =>
            {
                Native.LibVLCMediaParseStop(libvlc.NativeReference, NativeReference);
                tcs.TrySetCanceled();
            });

            void OnParsedChanged (object? sender, MediaParsedChangedEventArgs mediaParsedChangedEventArgs)
                => tcs.TrySetResult(mediaParsedChangedEventArgs.ParsedStatus);

            return MarshalUtils.InternalAsync(
                nativeCall: () =>
                {
                    var result = Native.LibVLCMediaParseRequest(libvlc.NativeReference, NativeReference, options, timeout);
                    if (result == -1)
                    {
                        tcs.TrySetResult(MediaParsedStatus.Failed);
                    }
                },
                sub: () => ParsedChanged += OnParsedChanged,
                unsub: () => ParsedChanged -= OnParsedChanged,
                tcs,
                ctr);
        }

        /// <summary>Get Parsed status for media descriptor object.</summary>
        /// <returns>a value of the libvlc_media_parsed_status_t enum</returns>
        /// <remarks>
        /// <para>libvlc_MediaParsedChanged</para>
        /// <para>libvlc_media_parsed_status_t</para>
        /// <para>LibVLC 3.0.0 or later</para>
        /// </remarks>
        public MediaParsedStatus ParsedStatus => Native.LibVLCMediaGetParsedStatus(NativeReference);

        /// <summary>Stop the parsing of the media</summary>
        /// <param name="libvlc">LibVLC instance that is to cease or give up parsing the media</param>
        /// <remarks>
        /// <para>When the media parsing is stopped, the libvlc_MediaParsedChanged event will</para>
        /// <para>be sent with the libvlc_media_parsed_status_timeout status.</para>
        /// <para>libvlc_media_parse_request</para>
        /// <para>LibVLC 3.0.0 or later</para>
        /// </remarks>
        public void ParseStop(LibVLC libvlc) => Native.LibVLCMediaParseStop(libvlc.NativeReference, NativeReference);

        /// <summary>
        /// Get the track list for one type
        /// LibVLC 4.0.0 and later.
        /// You need to call libvlc_media_parse_request() or play the media
        /// at least once before calling this function.Not doing this will result in
        /// an empty list.
        /// </summary>
        /// <param name="type">type of the track list to request</param>
        /// <returns>a valid libvlc_media_tracklist_t or NULL in case of error, if there
        /// is no track for a category, the returned list will have a size of 0, delete
        /// with libvlc_media_tracklist_delete()
        /// </returns>
        public MediaTrackList? TrackList(TrackType type)
        {
            var trackListPtr = Native.LibVLCMediaGetTracklist(NativeReference, type);
            if (trackListPtr == IntPtr.Zero)
                return null;
            return new MediaTrackList(trackListPtr);
        }

        /// <summary>
        /// <para>Get subitems of media descriptor object. This will increment</para>
        /// <para>the reference count of supplied media descriptor object. Use</para>
        /// <para>libvlc_media_list_release() to decrement the reference counting.</para>
        /// </summary>
        /// <returns>list of media descriptor subitems or NULL</returns>
        public MediaList SubItems => new MediaList(Native.LibVLCMediaSubitems(NativeReference));

        /// <summary>
        /// The type of the media
        /// </summary>
        public MediaType Type => Native.LibVLCMediaGetType(NativeReference);

        /// <summary>Add a slave to the current media.</summary>
        /// <param name="type">subtitle or audio</param>
        /// <param name="priority">from 0 (low priority) to 4 (high priority)</param>
        /// <param name="uri">Uri of the slave (should contain a valid scheme).</param>
        /// <returns>true on success, false on error.</returns>
        /// <remarks>
        /// <para>A slave is an external input source that may contains an additional subtitle</para>
        /// <para>track (like a .srt) or an additional audio track (like a .ac3).</para>
        /// <para>This function must be called before the media is parsed (via</para>
        /// <para>libvlc_media_parse_request()) or before the media is played (via</para>
        /// <para>libvlc_media_player_play())</para>
        /// <para>LibVLC 3.0.0 and later.</para>
        /// </remarks>
        public bool AddSlave(MediaSlaveType type, uint priority, string uri)
        {
            var uriUtf8 = uri.ToUtf8();
            return MarshalUtils.PerformInteropAndFree(() => Native.LibVLCMediaAddSlaves(NativeReference, type, priority, uriUtf8) == 0, uriUtf8);
        }

        /// <summary>Add a slave to the current media.</summary>
        /// <param name="type">subtitle or audio</param>
        /// <param name="priority">from 0 (low priority) to 4 (high priority)</param>
        /// <param name="uri">Uri of the slave (should contain a valid scheme).</param>
        /// <returns>true on success, false on error.</returns>
        /// <remarks>
        /// <para>A slave is an external input source that may contains an additional subtitle</para>
        /// <para>track (like a .srt) or an additional audio track (like a .ac3).</para>
        /// <para>This function must be called before the media is parsed (via</para>
        /// <para>libvlc_media_parse_request()) or before the media is played (via</para>
        /// <para>libvlc_media_player_play())</para>
        /// <para>LibVLC 3.0.0 and later.</para>
        /// </remarks>
        public bool AddSlave(MediaSlaveType type, uint priority, Uri uri)
        {
            var uriUtf8 = uri?.AbsoluteUri?.ToUtf8() ?? IntPtr.Zero;
            return MarshalUtils.PerformInteropAndFree(() => Native.LibVLCMediaAddSlaves(NativeReference, type, priority, uriUtf8) == 0, uriUtf8);
        }

        /// <summary>
        /// <para>Clear all slaves previously added by libvlc_media_slaves_add() or</para>
        /// <para>internally.</para>
        /// </summary>
        /// <remarks>LibVLC 3.0.0 and later.</remarks>
        public void ClearSlaves() => Native.LibVLCMediaClearSlaves(NativeReference);

        /// <summary>Get a media descriptor's slave list</summary>
        /// <para>address to store an allocated array of slaves (must be</para>
        /// <para>freed with libvlc_media_slaves_release()) [OUT]</para>
        /// <returns>the number of slaves (zero on error)</returns>
        /// <remarks>
        /// <para>The list will contain slaves parsed by VLC or previously added by</para>
        /// <para>libvlc_media_slaves_add(). The typical use case of this function is to save</para>
        /// <para>a list of slave in a database for a later use.</para>
        /// <para>LibVLC 3.0.0 and later.</para>
        /// <para>libvlc_media_slaves_add</para>
        /// </remarks>
        public MediaSlave[] Slaves => MarshalUtils.Retrieve(NativeReference, (IntPtr nativeRef, out IntPtr array) => Native.LibVLCMediaGetSlaves(nativeRef, out array),
            MarshalUtils.PtrToStructure<MediaSlaveStructure>,
            s => s.Build(),
            Native.LibVLCMediaReleaseSlaves);

        /// <summary>Get a media's codec description</summary>
        /// <param name="type">The type of the track</param>
        /// <param name="codec">the codec or fourcc</param>
        /// <returns>the codec description</returns>
        public string CodecDescription(TrackType type, uint codec) => Native.LibVLCMediaGetCodecDescription(type, codec).FromUtf8()!;

        /// <summary>Start an asynchronous thumbnail generation.
        /// If the request is successfuly queued, the MediaThumbnailGenerated event is guaranteed to be emited.</summary>
        /// <param name="libvlc">LibVLC instance to generate the thumbnail with</param>
        /// <param name="time">The time at which the thumbnail should be generated</param>
        /// <param name="speed">The seeking speed</param>
        /// <param name="width">The thumbnail width</param>
        /// <param name="height">The thumbnail height</param>
        /// <param name="crop">Should the picture be cropped to preserve aspect ratio</param>
        /// <param name="pictureType">The thumbnail picture type</param>
        /// <param name="timeout">A timeout value in ms, or 0 to disable timeout</param>
        /// <param name="cancellationToken">The cancellation token needed to cancel the thumbnail generation</param>
        /// <returns>A valid Picture object or null in case of failure</returns>
        public Task<Picture> GenerateThumbnailAsync(LibVLC libvlc, long time, ThumbnailerSeekSpeed speed,
                uint width, uint height, bool crop, PictureType pictureType, long timeout = 0, CancellationToken cancellationToken = default)
        {
            return ThumbnailRequestInternal(() =>
                            Native.LibVLCMediaThumbnailRequestByTime(libvlc.NativeReference, NativeReference, time, speed, width, height, crop, pictureType, timeout),
                            cancellationToken);
        }

        /// <summary>Start an asynchronous thumbnail generation.
        /// If the request is successfuly queued, the MediaThumbnailGenerated event is guaranteed to be emited.</summary>
        /// <param name="libvlc">LibVLC instance to generate the thumbnail with</param>
        /// <param name="position">The position at which the thumbnail should be generated</param>
        /// <param name="speed">The seeking speed</param>
        /// <param name="width">The thumbnail width</param>
        /// <param name="height">The thumbnail height</param>
        /// <param name="crop">Should the picture be cropped to preserve aspect ratio</param>
        /// <param name="pictureType">The thumbnail picture type</param>
        /// <param name="timeout">A timeout value in ms, or 0 to disable timeout</param>
        /// <param name="cancellationToken">The cancellation token needed to cancel the thumbnail generation</param>
        /// <returns>A valid Picture object or null in case of failure</returns>
        public Task<Picture> GenerateThumbnailAsync(LibVLC libvlc, double position, ThumbnailerSeekSpeed speed,
                uint width, uint height, bool crop, PictureType pictureType, long timeout = 0, CancellationToken cancellationToken = default)
        {
            return ThumbnailRequestInternal(() =>
                Native.LibVLCMediaThumbnailRequestByPosition(libvlc.NativeReference, NativeReference, position, speed, width, height, crop, pictureType, timeout),
                cancellationToken);
        }

        Task<Picture> ThumbnailRequestInternal(Func<IntPtr> nativeCall, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ThumbnailerRequest request = default;
            var tcs = new TaskCompletionSource<Picture>();

            var ctr = cancellationToken.Register(() =>
            {
                if (request.Valid)
                    Native.LibVLCMediaThumbnailRequestDestroy(request.NativeReference);
                tcs.TrySetCanceled();
            });

            void OnThumbnailGenerated(object? sender, MediaThumbnailGeneratedEventArgs mediaThumbnailGeneratedEventArgs)
            {
                if (mediaThumbnailGeneratedEventArgs.Thumbnail != null)
                    tcs.TrySetResult(mediaThumbnailGeneratedEventArgs.Thumbnail);
                else
                    tcs.TrySetCanceled();
            }

            return MarshalUtils.InternalAsync(
                nativeCall: () =>
                {
                    var result = nativeCall();
                    request = new ThumbnailerRequest(result);
                },
                sub: () => ThumbnailGenerated += OnThumbnailGenerated,
                unsub: () =>
                {
                    ThumbnailGenerated -= OnThumbnailGenerated;
                    if (request.Valid)
                        Native.LibVLCMediaThumbnailRequestDestroy(request.NativeReference);
                },
                tcs: tcs,
                ctr: ctr);
        }

        /// <summary>
        /// Equality override for this media instance
        /// </summary>
        /// <param name="obj">the media to compare this one with</param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            return obj is Media media &&
                   EqualityComparer<IntPtr>.Default.Equals(NativeReference, media.NativeReference);
        }

        /// <summary>
        /// Custom hascode implemenation for this Media instance
        /// </summary>
        /// <returns>the hashcode for this Media instance</returns>
        public override int GetHashCode()
        {
            return NativeReference.GetHashCode();
        }

        /// <summary>Increments the native reference counter for the media</summary>
        internal void Retain()
        {
            if (NativeReference != IntPtr.Zero)
                Native.LibVLCMediaRetain(NativeReference);
        }

        internal override void OnNativeInstanciationError()
        {
            throw new VLCException("Failed to instanciate the Media on the native side. " +
                    $"{Environment.NewLine}Have you installed the latest LibVLC package from nuget for your target platform?" +
                    $"{Environment.NewLine}Is your MRL correct? Do check the native LibVLC verbose logs for more information.");
        }

        #region MediaFromStream

        static readonly InternalOpenMedia OpenMediaCallbackHandle = OpenMediaCallback;
        static readonly InternalReadMedia ReadMediaCallbackHandle = ReadMediaCallback;
        static readonly InternalSeekMedia SeekMediaCallbackHandle = SeekMediaCallback;
        static readonly InternalCloseMedia CloseMediaCallbackHandle = CloseMediaCallback;

        [MonoPInvokeCallback(typeof(InternalOpenMedia))]
        static int OpenMediaCallback(IntPtr opaque, ref IntPtr data, out ulong size)
        {
            data = opaque;
            var input = MarshalUtils.GetInstance<MediaInput>(opaque);
            if (input == null)
            {
                size = 0UL;
                return -1;
            }

            return input.Open(out size) ? 0 : -1;
        }

        [MonoPInvokeCallback(typeof(InternalReadMedia))]
        static nint ReadMediaCallback(IntPtr opaque, IntPtr buf, uint len)
        {
            var input = MarshalUtils.GetInstance<MediaInput>(opaque);
            if (input == null)
            {
                return -1;
            }
            return input.Read(buf, len);
        }

        [MonoPInvokeCallback(typeof(InternalSeekMedia))]
        static int SeekMediaCallback(IntPtr opaque, ulong offset)
        {
            var input = MarshalUtils.GetInstance<MediaInput>(opaque);
            if (input == null)
            {
                return -1;
            }
            return input.Seek(offset) ? 0 : -1;
        }

        [MonoPInvokeCallback(typeof(InternalCloseMedia))]
        static void CloseMediaCallback(IntPtr opaque)
        {
            var input = MarshalUtils.GetInstance<MediaInput>(opaque);
            input?.Close();
        }

        #endregion

        #region MediaFromCallbacks

        /// <summary>
        /// <para>It consists of a media location and various optional meta data.</para>
        /// <para>@{</para>
        /// <para></para>
        /// <para>LibVLC media item/descriptor external API</para>
        /// </summary>
        /// <summary>Callback prototype to open a custom bitstream input media.</summary>
        /// <param name="opaque">private pointer as passed to libvlc_media_new_callbacks()</param>
        /// <param name="data">storage space for a private data pointer [OUT]</param>
        /// <param name="size">byte length of the bitstream or UINT64_MAX if unknown [OUT]</param>
        /// <returns>
        /// <para>0 on success, non-zero on error. In case of failure, the other</para>
        /// <para>callbacks will not be invoked and any value stored in *datap and *sizep is</para>
        /// <para>discarded.</para>
        /// </returns>
        /// <remarks>
        /// <para>The same media item can be opened multiple times. Each time, this callback</para>
        /// <para>is invoked. It should allocate and initialize any instance-specific</para>
        /// <para>resources, then store them in *datap. The instance resources can be freed</para>
        /// <para>in the</para>
        /// <para>For convenience, *datap is initially NULL and *sizep is initially 0.</para>
        /// </remarks>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int InternalOpenMedia(IntPtr opaque, ref IntPtr data, out ulong size);

        /// <summary>Callback prototype to read data from a custom bitstream input media.</summary>
        /// <param name="opaque">private pointer as set by the</param>
        /// <param name="buf">start address of the buffer to read data into</param>
        /// <param name="len">bytes length of the buffer</param>
        /// <returns>
        /// <para>strictly positive number of bytes read, 0 on end-of-stream,</para>
        /// <para>or -1 on non-recoverable error</para>
        /// </returns>
        /// <remarks>
        /// <para>callback</para>
        /// <para>If no data is immediately available, then the callback should sleep.</para>
        /// <para>The application is responsible for avoiding deadlock situations.</para>
        /// <para>In particular, the callback should return an error if playback is stopped;</para>
        /// <para>if it does not return, then libvlc_media_player_stop() will never return.</para>
        /// </remarks>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate nint InternalReadMedia(IntPtr opaque, IntPtr buf, uint len);

        /// <summary>Callback prototype to seek a custom bitstream input media.</summary>
        /// <param name="opaque">private pointer as set by the</param>
        /// <param name="offset">absolute byte offset to seek to</param>
        /// <returns>0 on success, -1 on error.</returns>
        /// <remarks>callback</remarks>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int InternalSeekMedia(IntPtr opaque, ulong offset);

        /// <summary>Callback prototype to close a custom bitstream input media.</summary>
        /// <param name="opaque">private pointer as set by the</param>
        /// <remarks>callback</remarks>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void InternalCloseMedia(IntPtr opaque);
        #endregion

        #region Events

        /// <summary>
        /// The meta information changed
        /// </summary>
        public event EventHandler<MediaMetaChangedEventArgs> MetaChanged
        {
            add => EventManager.AttachEvent(EventType.MediaMetaChanged, value);
            remove => EventManager.DetachEvent(EventType.MediaMetaChanged, value);
        }

        /// <summary>
        /// The parsing status changed
        /// </summary>
        public event EventHandler<MediaParsedChangedEventArgs> ParsedChanged
        {
            add => EventManager.AttachEvent(EventType.MediaParsedChanged, value);
            remove => EventManager.DetachEvent(EventType.MediaParsedChanged, value);
        }

        /// <summary>
        /// A sub item was added to this media's MediaList
        /// </summary>
        public event EventHandler<MediaSubItemAddedEventArgs> SubItemAdded
        {
            add => EventManager.AttachEvent(EventType.MediaSubItemAdded, value);
            remove => EventManager.DetachEvent(EventType.MediaSubItemAdded, value);
        }

        /// <summary>
        /// The duration of the media changed
        /// </summary>
        public event EventHandler<MediaDurationChangedEventArgs> DurationChanged
        {
            add => EventManager.AttachEvent(EventType.MediaDurationChanged, value);
            remove => EventManager.DetachEvent(EventType.MediaDurationChanged, value);
        }

        /// <summary>
        /// A sub item tree was added to this media
        /// </summary>
        public event EventHandler<MediaSubItemTreeAddedEventArgs> SubItemTreeAdded
        {
            add => EventManager.AttachEvent(EventType.MediaSubItemTreeAdded, value);
            remove => EventManager.DetachEvent(EventType.MediaSubItemTreeAdded, value);
        }

        /// <summary>
        /// A thumbnail was generated for this media
        /// </summary>
        public event EventHandler<MediaThumbnailGeneratedEventArgs> ThumbnailGenerated
        {
            add => EventManager.AttachEvent(EventType.MediaThumbnailGenerated, value);
            remove => EventManager.DetachEvent(EventType.MediaThumbnailGenerated, value);
        }

        /// <summary>
        /// Attached thumbnails were found on the media
        /// </summary>
        public event EventHandler<MediaAttachedThumbnailsFoundEventArgs> AttachedThumbnailsFound
        {
            add => EventManager.AttachEvent(EventType.MediaAttachedThumbnailsFound, value);
            remove => EventManager.DetachEvent(EventType.MediaAttachedThumbnailsFound, value);
        }

        #endregion
    }

    #region enums

    /// <summary>
    /// libvlc media or media_player state
    /// </summary>
    public enum VLCState
    {
        /// <summary>
        /// Nothing special happening
        /// </summary>
        NothingSpecial = 0,

        /// <summary>
        /// Opening media
        /// </summary>
        Opening = 1,

        /// <summary>
        /// Buffering media
        /// </summary>
        Buffering = 2,

        /// <summary>
        /// Playing media
        /// </summary>
        Playing = 3,

        /// <summary>
        /// Paused media
        /// </summary>
        Paused = 4,

        /// <summary>
        /// Stopped media
        /// </summary>
        Stopped = 5,

        /// <summary>
        /// Stopping media
        /// </summary>
        Stopping = 6,

        /// <summary>
        /// Error media
        /// </summary>
        Error = 7
    }

    /// <summary>
    /// Media track type such as Audio, Video or Text
    /// </summary>
    public enum TrackType
    {
        /// <summary>
        /// Unknown track
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// Audio track
        /// </summary>
        Audio = 0,

        /// <summary>
        /// Video track
        /// </summary>
        Video = 1,

        /// <summary>
        /// Text/Subtitle track
        /// </summary>
        Text = 2
    }

    /// <summary>
    /// Video orientation
    /// </summary>
    public enum VideoOrientation
    {
        /// <summary>Normal. Top line represents top, left column left.</summary>
        TopLeft = 0,
        /// <summary>Flipped horizontally</summary>
        TopRight = 1,
        /// <summary>Flipped vertically</summary>
        BottomLeft = 2,
        /// <summary>Rotated 180 degrees</summary>
        BottomRight = 3,
        /// <summary>Transposed</summary>
        LeftTop = 4,
        /// <summary>Rotated 90 degrees clockwise (or 270 anti-clockwise)</summary>
        LeftBottom = 5,
        /// <summary>Rotated 90 degrees anti-clockwise</summary>
        RightTop = 6,
        /// <summary>Anti-transposed</summary>
        RightBottom = 7
    }

    /// <summary>
    /// Video projection
    /// </summary>
    [Flags]
    public enum VideoProjection
    {
        /// <summary>
        /// Rectangular
        /// </summary>
        Rectangular = 0,
        /// <summary>360 spherical</summary>
        Equirectangular = 1,

        /// <summary>
        /// Cubemap layout standard
        /// </summary>
        CubemapLayoutStandard = 256
    }

    /// <summary>Type of a media slave: subtitle or generic (audio/video).</summary>
    public enum MediaSlaveType
    {
        /// <summary>
        /// Subtitle
        /// </summary>
        Subtitle = 0,

        /// <summary>
        /// Generic (audio or video)
        /// </summary>
        Generic = 1,

        /// <summary>
        /// Audio
        /// </summary>
        Audio = Generic
    }

    /// <summary>
    /// Meta data types
    /// </summary>
    public enum MetadataType
    {
        /// <summary>
        /// Title metadata
        /// </summary>
        Title = 0,

        /// <summary>
        /// Artist metadata
        /// </summary>
        Artist = 1,

        /// <summary>
        /// Genre metadata
        /// </summary>
        Genre = 2,

        /// <summary>
        /// Copyright metadata
        /// </summary>
        Copyright = 3,

        /// <summary>
        /// Album metadata
        /// </summary>
        Album = 4,

        /// <summary>
        /// Track number metadata
        /// </summary>
        TrackNumber = 5,

        /// <summary>
        /// Description metadata
        /// </summary>
        Description = 6,

        /// <summary>
        /// Rating metadata
        /// </summary>
        Rating = 7,

        /// <summary>
        /// Date metadata
        /// </summary>
        Date = 8,

        /// <summary>
        /// Setting metadata
        /// </summary>
        Setting = 9,

        /// <summary>
        /// URL metadata
        /// </summary>
        URL = 10,

        /// <summary>
        /// Language metadata
        /// </summary>
        Language = 11,

        /// <summary>
        /// Now playing metadata
        /// </summary>
        NowPlaying = 12,

        /// <summary>
        /// Publisher metadata
        /// </summary>
        Publisher = 13,

        /// <summary>
        /// Encoded by metadata
        /// </summary>
        EncodedBy = 14,

        /// <summary>
        /// Artwork URL metadata
        /// </summary>
        ArtworkURL = 15,

        /// <summary>
        /// Track ID metadata
        /// </summary>
        TrackID = 16,

        /// <summary>
        /// Total track metadata
        /// </summary>
        TrackTotal = 17,

        /// <summary>
        /// Director metadata
        /// </summary>
        Director = 18,

        /// <summary>
        /// Season metadata
        /// </summary>
        Season = 19,

        /// <summary>
        /// Episode metadata
        /// </summary>
        Episode = 20,

        /// <summary>
        /// Show name metadata
        /// </summary>
        ShowName = 21,

        /// <summary>
        /// Actors metadata
        /// </summary>
        Actors = 22,

        /// <summary>
        /// Album artist metadata
        /// </summary>
        AlbumArtist = 23,

        /// <summary>
        /// Disc number metadata
        /// </summary>
        DiscNumber = 24,

        /// <summary>
        /// Disc total metadata
        /// </summary>
        DiscTotal = 25
    }

    /// <summary>
    /// The FromType enum is used to drive the media creation.
    /// A media is usually created using a string, which can represent one of 3 things: FromPath, FromLocation, AsNode.
    /// </summary>
    public enum FromType
    {
        /// <summary>
        /// Create a media for a certain file path.
        /// </summary>
        FromPath,
        /// <summary>
        /// Create a media with a certain given media resource location,
        /// for instance a valid URL.
        /// note To refer to a local file with this function,
        /// the file://... URI syntax <b>must</b> be used (see IETF RFC3986).
        /// We recommend using FromPath instead when dealing with
        ///local files.
        /// </summary>
        FromLocation,
        /// <summary>
        /// Create a media as an empty node with a given name.
        /// </summary>
        AsNode
    }

    /// <summary>
    /// Parse flags used by libvlc_media_parse_request()
    /// </summary>
    [Flags]
    public enum MediaParseOptions
    {
        /// <summary>Parse media if it's a local file</summary>
        ParseLocal = 0x01,
        /// <summary>Parse media even if it's a network file</summary>
        ParseNetwork = 0x02,
        /// <summary>Force parsing the media even if it would be skipped.</summary>
        ParseForced = 0x04,
        /// <summary>Fetch meta and covert art using local resources</summary>
        FetchLocal = 0x08,
        /// <summary>Fetch meta and covert art using network resources</summary>
        FetchNetwork = 0x10,
        /// <summary>
        /// Interact with the user (via libvlc_dialog_cbs) when preparsing this item
        /// (and not its sub items). Set this flag in order to receive a callback
        /// when the input is asking for credentials.
        /// </summary>
        DoInteract = 0x20
    }

    /// <summary>
    /// Parse status used sent by libvlc_media_parse_request() or returned by
    /// libvlc_media_get_parsed_status()
    /// </summary>
    public enum MediaParsedStatus
    {
        /// <summary>
        /// Default unparsed status
        /// </summary>
        None,

        /// <summary>
        /// Parsing is currently processing
        /// </summary>
        Pending,

        /// <summary>
        /// Parsing was skipped
        /// </summary>
        Skipped,

        /// <summary>
        /// Parsing failed
        /// </summary>
        Failed,

        /// <summary>
        /// Parsing timed out
        /// </summary>
        Timeout,

        /// <summary>
        /// Parsing cancelled
        /// </summary>
        Cancelled,

        /// <summary>
        /// Parsing completed successfully
        /// </summary>
        Done
    }

    /// <summary>Media type</summary>
    /// <remarks>libvlc_media_get_type</remarks>
    public enum MediaType
    {
        /// <summary>
        /// Unknown media type
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// File type
        /// </summary>
        File = 1,

        /// <summary>
        /// Directory type
        /// </summary>
        Directory = 2,

        /// <summary>
        /// Disc type
        /// </summary>
        Disc = 3,

        /// <summary>
        /// Stream type
        /// </summary>
        Stream = 4,

        /// <summary>
        /// Playlist type
        /// </summary>
        Playlist = 5
    }

    /// <summary>
    /// Thumbnailer seeking speed configuration
    /// </summary>
    public enum ThumbnailerSeekSpeed
    {
        /// <summary>
        /// Precise seek
        /// </summary>
        Precise,

        /// <summary>
        /// Fast seek
        /// </summary>
        Fast
    }

    /// <summary>
    /// Type of stat that can be requested from FileStat
    /// </summary>
    public enum FileStat : uint
    {
        /// <summary>
        /// The modified timestamp
        /// </summary>
        Mtime = 0,

        /// <summary>
        /// The file size
        /// </summary>
        Size = 1
    }

    /// <summary>
    /// Thumbnailer request holder
    /// </summary>
    internal readonly struct ThumbnailerRequest
    {
        internal readonly IntPtr NativeReference;
        internal readonly bool Valid => NativeReference != IntPtr.Zero;

        internal ThumbnailerRequest(IntPtr nativeReference)
        {
            NativeReference = nativeReference;
        }
    }

#endregion
}
