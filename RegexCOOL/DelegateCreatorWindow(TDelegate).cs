using System;
using System.Windows;


namespace CB.RegexCOOL
{
    public class DelegateCreatorWindow<TDelegate>: DelegateCreatorWindow where TDelegate: class
    {
        #region  Constructors & Destructor
        static DelegateCreatorWindow()
        {
            if (!(typeof(TDelegate).IsSubclassOf(typeof(Delegate)))) throw new InvalidOperationException();
        }

        public DelegateCreatorWindow()
        {
            buttonCreate.Click += ButtonCreate_Click;
        }
        #endregion


        #region Dependency Properties
        public static readonly DependencyPropertyKey DelegateDependencyPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "DelegateDependencyKey", typeof(TDelegate), typeof(DelegateCreatorWindow<TDelegate>),
                new FrameworkPropertyMetadata(null, OnDelegateChanged));

        public TDelegate Delegate
            => GetValue(DelegateDependencyPropertyKey.DependencyProperty) as TDelegate;
        #endregion


        #region Override
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            textBoxEditor.Text = DynamicDelegateGenerator<TDelegate>.GenerateCode();
            listBoxReferences.ItemsSource =
                DynamicDelegateGenerator<TDelegate>.GetDefaultReferenceAssemblies();
        }
        #endregion


        #region Event Handlers
        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var del =
                    DynamicDelegateGenerator<TDelegate>.CreateDelegateFromSource(textBoxEditor.Text);
                SetValue(DelegateDependencyPropertyKey, del);
            }
            catch (Exception exception)
            {
                textBoxError.Text = exception.Message;
                SetValue(DelegateDependencyPropertyKey, null);
            }
        }
        #endregion


        #region Implementation
        private static void OnDelegateChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as DelegateCreatorWindow<TDelegate>;
            element?.OnDelegateChanged();
        }
        #endregion
    }
}