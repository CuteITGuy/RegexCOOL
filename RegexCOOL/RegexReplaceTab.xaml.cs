using System;
using System.Text.RegularExpressions;
using System.Windows;


namespace CB.RegexCOOL
{
    public partial class RegexReplaceTab
    {
        #region Fields
        private DelegateCreatorWindow<MatchEvaluator> _delegateCreatorWindow;
        #endregion


        #region  Constructors & Destructors
        public RegexReplaceTab()
        {
            InitializeComponent();
        }
        #endregion


        #region Event Handlers
        private void ButtonMatchEvaluator_OnClick(object sender, RoutedEventArgs e)
        {
            if (_delegateCreatorWindow == null)
            {
                _delegateCreatorWindow = new DelegateCreatorWindow<MatchEvaluator>();
                _delegateCreatorWindow.DelegateChanged += DelegateCreatorWindow_DelegateChanged;
            }
            _delegateCreatorWindow.Show();
        }

        private void DelegateCreatorWindow_DelegateChanged(object sender, RoutedEventArgs e)
        {
            var matchEvaluator = _delegateCreatorWindow.Delegate;
            if (matchEvaluator == null)
            {
                return;
            }

            try
            {
                textBoxOutput.Text = Regex.Replace(textBoxInput.Text, Pattern, matchEvaluator);
                Window.GetWindow(this)?.Activate();
            }
            catch (Exception exception)
            {
                SetValue(ErrorProperty, exception.Message);
            }
        }
        #endregion
    }
}