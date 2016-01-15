using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CB.RegexCOOL
{
    public class CapturesStackPanel : StackPanel
    {
        #region Fields
        private static readonly Brush captureChildBackground = Brushes.SpringGreen;
        private static readonly Brush captureChildForeground = Brushes.Blue;
        private static readonly Brush captureChildBorder = Brushes.DarkCyan;
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty CapturesProperty = DependencyProperty.Register(
            "Captures", typeof(IEnumerable<Capture>), typeof(CapturesStackPanel),
            new PropertyMetadata(default(IEnumerable<Capture>), CapturesPropertyChanged));

        public IEnumerable<Capture> Captures
        {
            get { return (IEnumerable<Capture>)GetValue(CapturesProperty); }
            set { SetValue(CapturesProperty, value); }
        }
        #endregion


        #region Implementation
        private void AddCaptureChildren(IEnumerable<Capture> captures)
        {
            if (captures == null)
            {
                return;
            }

            foreach (var capture in captures)
            {
                Children.Add(CreateCaptureChild(capture.Value));
            }
        }

        private static void CapturesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = d as CapturesStackPanel;
            var captures = e.NewValue as IEnumerable<Capture>;

            if (panel == null)
            {
                return;
            }

            panel.Children.Clear();
            panel.AddCaptureChildren(captures);
        }

        private static UIElement CreateCaptureChild(string text)
        {
            var border = new Border
            {
                Background = captureChildBackground,
                BorderBrush = captureChildBorder,
                BorderThickness = new Thickness(1),
                Child = CreateTextBlock(text),
                CornerRadius = new CornerRadius(6),
                Margin = new Thickness(2, 0, 2, 0),
                Padding = new Thickness(6, 0, 6, 0),
                SnapsToDevicePixels = true,
                VerticalAlignment = VerticalAlignment.Center
            };

            return border;
        }

        private static UIElement CreateTextBlock(string text)
        {
            var textBlock = new TextBlock
            {
                Foreground = captureChildForeground,
                SnapsToDevicePixels = true,
                Text = text
            };
            return textBlock;
        }
        #endregion
    }
}