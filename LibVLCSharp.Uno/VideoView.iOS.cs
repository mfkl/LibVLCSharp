﻿namespace LibVLCSharp.Platforms.Uno
{
    /// <summary>
    /// Video view
    /// </summary>
    public partial class VideoView
    {
        private iOS.VideoView? UnderlyingVideoView
        {
            get;
            set;
        }

        private iOS.VideoView CreateVideoView()
        {
            return new iOS.VideoView();
        }
    }
}
