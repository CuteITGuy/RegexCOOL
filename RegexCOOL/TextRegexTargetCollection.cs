using System.Text.RegularExpressions;

namespace CB.RegexCOOL
{
    public class TextRegexTargetCollection : RegexTargetCollection
    {
        #region Properties
        public override string Input
        {
            get { return base.Input; }
            set
            {
                base.Input = value;
                ApplyRegex();
            }
        }
        #endregion


        #region Implementation
        protected override void ApplyRegex()
        {
            Clear();

            if (regex == null || string.IsNullOrEmpty(input))
            {
                return;
            }

            foreach (Match match in regex.Matches(input))
            {
                Add(new RegexMatchTarget(match));
            }
        }
        #endregion
    }
}