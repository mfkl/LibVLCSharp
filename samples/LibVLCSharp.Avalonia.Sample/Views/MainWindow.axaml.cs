using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using LibVLCSharp.Avalonia.Sample.ViewModels;

namespace LibVLCSharp.Avalonia.Sample.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void VisibilityButton_OnClick(object? sender, RoutedEventArgs e)
        {
            var isVisible = VisibilityButton?.IsChecked!.Value ?? false;
            VideoControl.VideoView.IsVisible = isVisible;
            VideoControl.ModifiedVideoView.IsVisible = isVisible;
        }

        private void ContentButton_OnClickButton_OnClick(object? sender, RoutedEventArgs e)
        {
            VideoControl.VideoView.Content = ContentButton?.IsChecked!.Value ?? false
            ? new Border() { Background = new SolidColorBrush(Colors.Gold), Margin = new Thickness(0d, 25d) }
            : new Border() { Background = new SolidColorBrush(Colors.Plum), Margin = new Thickness(0d, 25d) };

            VideoControl.ModifiedVideoView.Content = ContentButton?.IsChecked!.Value ?? false
                ? new Border() { Background = new SolidColorBrush(Colors.Green), Margin = new Thickness(0d, 25d) }
                : new Border() { Background = new SolidColorBrush(Colors.DarkOrange), Margin = new Thickness(0d, 25d) };

        }

    }
}