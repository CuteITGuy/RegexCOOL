using System.Windows;


namespace CB.RegexCOOL
{
    public partial class DelegateCreatorWindow
    {
        #region Fields
        public static readonly RoutedEvent DelegateChangedEvent = EventManager.RegisterRoutedEvent(
            "DelegateChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler),
            typeof(DelegateCreatorWindow));
        #endregion


        #region  Constructors & Destructors
        public DelegateCreatorWindow()
        {
            InitializeComponent();
        }
        #endregion


        #region Events
        public event RoutedEventHandler DelegateChanged
        {
            add { AddHandler(DelegateChangedEvent, value); }
            remove { RemoveHandler(DelegateChangedEvent, value); }
        }
        #endregion


        #region Implementation
        protected virtual void OnDelegateChanged()
        {
            RaiseEvent(new RoutedEventArgs(DelegateChangedEvent, this));
        }
        #endregion
    }
}