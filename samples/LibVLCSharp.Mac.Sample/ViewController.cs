﻿using System;

using AppKit;
using Foundation;

using LibVLCSharp.Platforms.Mac;
using LibVLCSharp.Shared;

namespace LibVLCSharp.Mac.Sample
{
    public partial class ViewController : NSViewController
    {
        VideoView _videoView;
        LibVLC _libVLC;
        Shared.MediaPlayer _mediaPlayer;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Core.Initialize();

            _libVLC = new LibVLC("--verbose=2");
            _mediaPlayer = new Shared.MediaPlayer(_libVLC);

            _videoView = new VideoView { MediaPlayer = _mediaPlayer };

            View = _videoView;

            _videoView.MediaPlayer.Play(new Media(_libVLC, "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4", FromType.FromLocation));
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
   }
}