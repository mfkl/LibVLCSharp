using System;
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

        private void OnDataContextChanged(object sender, EventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                vm.Play();
            }
        }

        //private void InitializeComponent()
        //{
        //    AvaloniaXamlLoader.Load(this);
        //}

        private void VisibilityButton_OnClick(object? sender, RoutedEventArgs e)
        {
            var isVisible = VisibilityButton?.IsChecked!.Value ?? false;
            VideoView.IsVisible = isVisible;
            ModifiedVideoView.IsVisible = isVisible;
        }

        private void ContentButton_OnClickButton_OnClick(object? sender, RoutedEventArgs e)
        {
            VideoView.Content = ContentButton?.IsChecked!.Value ?? false
                ? new Border() { Background = new SolidColorBrush(Colors.Gold) }
                : new Border() { Background = new SolidColorBrush(Colors.Plum) };

            ModifiedVideoView.Content = ContentButton?.IsChecked!.Value ?? false
                ? new Border() { Background = new SolidColorBrush(Colors.Green) }
                : new Border() { Background = new SolidColorBrush(Colors.DarkOrange) };
        }

    }
}