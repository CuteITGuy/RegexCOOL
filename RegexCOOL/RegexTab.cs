using System.Windows;
using System.Windows.Controls;


namespace CB.RegexCOOL
{
    public class RegexTab: UserControl
    {
        #region Fields
        public static readonly DependencyProperty ErrorProperty = DependencyProperty.Register(
            "Error", typeof(string), typeof(RegexTab),
            new PropertyMetadata(default(string), OnErrorChanged));

        public static readonly DependencyProperty PatternProperty = DependencyProperty.Register(
            "Pattern", typeof(string), typeof(RegexTab),
            new PropertyMetadata(default(string), OnPatternChanged));

        public static readonly RoutedEvent ErrorChangedEvent = EventManager.RegisterRoutedEvent(
            "ErrorChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(RegexTab));

        public static readonly RoutedEvent PatternChangedEvent = EventManager.RegisterRoutedEvent(
            "PatternChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(RegexTab));
        #endregion


        #region Dependency Properties
        private static void OnErrorChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var thisElement = dependencyObject as RegexTab;
            if (thisElement != null)
            {
                thisElement.OnErrorChanged();
            }
        }

        public string Error
        {
            get { return (string)GetValue(ErrorProperty); }
            set { SetValue(ErrorProperty, value); }
        }

        private static void OnPatternChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var thisElement = d as RegexTab;
            if (thisElement != null)
            {
                thisElement.OnPatternChanged();
            }
        }

        public string Pattern
        {
            get { return (string)GetValue(PatternProperty); }
            set { SetValue(PatternProperty, value); }
        }
        #endregion


        #region Events
        public event RoutedEventHandler ErrorChanged
        {
            add { AddHandler(ErrorChangedEvent, value); }
            remove { RemoveHandler(ErrorChangedEvent, value); }
        }

        public event RoutedEventHandler PatternChanged
        {
            add { AddHandler(PatternChangedEvent, value); }
            remove { RemoveHandler(PatternChangedEvent, value); }
        }
        #endregion


        #region Implementation
        protected virtual void OnErrorChanged()
        {
            RaiseEvent(new RoutedEventArgs(ErrorChangedEvent, this));
        }

        protected virtual void OnPatternChanged()
        {
            RaiseEvent(new RoutedEventArgs(PatternChangedEvent, this));
        }
        #endregion
    }
}