using System.ComponentModel;
using System.Diagnostics;


namespace CB.RegexCOOL
{
    public partial class RegexSplitTab
    {
        private RegexSplitTarget regexSplitTarget;

        public RegexSplitTab()
        {
            InitializeComponent();
            GetResouces();
        }

        private void GetResouces()
        {
            regexSplitTarget = FindResource("regexSplitTarget") as RegexSplitTarget;
            Debug.Assert(regexSplitTarget != null, "regexSplitTarget != null");
            regexSplitTarget.PropertyChanged += RegexSplitTarget_OnPropertyChanged;
        }

        private void RegexSplitTarget_OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Error":
                    SetValue(ErrorProperty, regexSplitTarget.Error);
                    break;
            }
        }

        protected override void OnPatternChanged()
        {
            ChangeTargetPattern();
            base.OnPatternChanged();
        }

        private void ChangeTargetPattern()
        {
            var pattern = GetValue(PatternProperty) as string;
            regexSplitTarget.Pattern = pattern;
        }
    }
}