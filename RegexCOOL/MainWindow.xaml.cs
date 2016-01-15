using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace CB.RegexCOOL
{
    public partial class MainWindow
    {
        #region Fields
        private RegexTargetCollection collectionNegative;
        private RegexTargetCollection collectionPositve;
        private TextRegexTargetCollection collectionResults;
        private static Brush brushBorder;
        private static Brush brushText;
        private static readonly Brush brushError = Brushes.Red;
        private static readonly Brush brushFull = Brushes.White;
        #endregion


        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            FetchResources();
        }
        #endregion


        #region Event Handlers
        private void ButtonGo_OnClick(object sender, RoutedEventArgs e)
        {
            DoMatchJob();
        }

        private void Collections_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Error":
                    var collection = sender as RegexTargetCollection;
                    if (collection != null)
                    {
                        InformError(collection.Error);
                    }
                    break;
            }
        }

        private void CommandBindingTrim_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var target = GetCommandTrimTarget();
            e.CanExecute = target != null && !string.IsNullOrEmpty(target.Text);
        }

        private void CommandBindingTrim_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            TrimText(TrimmingPosition.TrimAll);
        }

        private void CommandBindingTrimEnd_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            TrimText(TrimmingPosition.TrimEnd);
        }

        private void CommandBindingTrimStart_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            TrimText(TrimmingPosition.TrimStart);
        }

        private void ListBoxes_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScrollSelectedItemIntoView(sender as ListBox);
        }

        private void ListBoxMatch_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BringToBack(e.Source as UIElement);
        }

        private void TextBoxInput_OnLostFocus(object sender, RoutedEventArgs e)
        {
            BringToBack(e.Source as UIElement);
        }

        private void TextBoxInput_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxBackground(sender as TextBox);
        }

        private void TextBoxPattern_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            DoMatchJob();
        }
        #endregion


        #region Implementation
        private static void BringToBack(UIElement element)
        {
            if (element != null)
            {
                var zIndex = Panel.GetZIndex(element);
                Panel.SetZIndex(element, zIndex - 1);
            }
        }

        private static void ChangeTextBoxBackground(TextBox textBox)
        {
            if (textBox != null)
            {
                textBox.Background = textBox.Text.Length > 0 ? brushFull : brushText;
            }
        }

        private void DoMatchJob()
        {
            collectionResults.Pattern = collectionPositve.Pattern = collectionNegative.Pattern = TextBoxPattern.Text;
        }

        private void FetchResources()
        {
            collectionNegative = FindResource("CollectionNegative") as LinesRegexTargetCollection;
            collectionPositve = FindResource("CollectionPositive") as LinesRegexTargetCollection;
            collectionResults = FindResource("CollectionResults") as TextRegexTargetCollection;

            Debug.Assert(collectionNegative != null, "collectionNegative != null");
            Debug.Assert(collectionPositve != null, "collectionPositve != null");
            Debug.Assert(collectionResults != null, "collectionResults != null");

            collectionNegative.PropertyChanged += Collections_OnPropertyChanged;
            collectionPositve.PropertyChanged += Collections_OnPropertyChanged;
            collectionResults.PropertyChanged += Collections_OnPropertyChanged;

            brushBorder = FindResource("BrushBorder") as Brush;
            brushText = FindResource("BrushText") as Brush;
        }

        private TextBox GetCommandTrimTarget()
        {
            return FocusManager.GetFocusedElement(this) as TextBox;
        }

        private void InformError(string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                TextBoxPattern.BorderBrush = brushBorder;
                TextBlockStatus.Visibility = Visibility.Hidden;
            }
            else
            {
                TextBoxPattern.BorderBrush = brushError;
                TextBlockStatus.Text = error;
                TextBlockStatus.Visibility = Visibility.Visible;
            }
        }

        private static void ScrollSelectedItemIntoView(ListBox listBox)
        {
            if (listBox == null)
            {
                return;
            }

            listBox.ScrollIntoView(listBox.SelectedItem);
        }

        private void TrimText(TrimmingPosition trimmingPosition)
        {
            var target = GetCommandTrimTarget();

            if (target != null)
            {
                target.Text = TextProcessor.TrimAllLines(target.Text, trimmingPosition);
            }
        }
        #endregion
    }
}