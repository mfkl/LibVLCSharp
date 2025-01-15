﻿using Microsoft.Maui.Handlers;
#if ANDROID
using VideoViewImpl = LibVLCSharp.VideoView;
#elif IOS
using VideoViewImpl = LibVLCSharp.VideoView;
#elif WINUI
using VideoViewImpl = LibVLCSharp.Platforms.Windows.VideoView;
#endif 

namespace LibVLCSharp.MAUI
{
    /// <summary>
    /// MAUI view handler for the VideoView
    /// </summary>
    public partial class VideoViewHandler : ViewHandler<VideoView, VideoViewImpl>
    {
        /// <inheritdoc />
        protected override void ConnectHandler(VideoViewImpl platformView) => base.ConnectHandler(platformView);

        /// <inheritdoc />
        protected override void DisconnectHandler(VideoViewImpl platformView) => base.DisconnectHandler(platformView);

        /// <inheritdoc />
        protected override VideoViewImpl CreatePlatformView()
        {
#if ANDROID
            return new VideoViewImpl(Context);
#else
            return new VideoViewImpl();
#endif
        }
    }
}
