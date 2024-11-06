using System;
using Avalonia.Controls;
using LibVLCSharp.Shared;

namespace LibVLCSharp.Avalonia.Sample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        private readonly LibVLC _libVlc = new LibVLC();

        public MediaPlayer MediaPlayer1 { get; }
        public MediaPlayer MediaPlayer2 { get; }

        public MainWindowViewModel()
        {
            MediaPlayer1 = new MediaPlayer(_libVlc);
            MediaPlayer2 = new MediaPlayer(_libVlc);
        }

        public void Play()
        {
            if (Design.IsDesignMode)
            {
                return;
            }

            using var media = new Media(_libVlc, new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"));
            MediaPlayer1.Play(media);
            MediaPlayer2.Play(media);
        }

        public void Stop()
        {
            MediaPlayer1.Stop();
            MediaPlayer2.Stop();
        }

        public void Dispose()
        {
            MediaPlayer1?.Dispose();
            MediaPlayer1?.Dispose();
            _libVlc?.Dispose();
        }
    }
}
