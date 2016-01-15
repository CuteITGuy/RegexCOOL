using System;
using System.Collections.Generic;
using System.Linq;

namespace CB.RegexCOOL
{
    public class LinesRegexTargetCollection : RegexTargetCollection
    {
        #region Properties
        public override string Input
        {
            get { return base.Input; }
            set
            {
                base.Input = value;
                GenerateItems();
            }
        }

        protected int matchCount;

        public virtual int UnmatchCount
        {
            get { return unmatchCount; }
            set { SetProperty(ref unmatchCount, value); }
        }

        protected int unmatchCount;

        public virtual int MatchCount
        {
            get { return matchCount; }
            set { SetProperty(ref matchCount, value); }
        }
        #endregion


        #region Implementation
        protected override void ApplyRegex()
        {
            ApplyRegex(Items);
            /*int matches = 0, unmatches = 0;

            foreach (var isMatch in Items.Select(regexTarget => regexTarget.ApplyRegex(regex)))
            {
                if (isMatch == true)
                {
                    ++matches;
                }
                else if (isMatch == false)
                {
                    ++unmatches;
                }
            }

            MatchCount = matches;
            UnmatchCount = unmatches;*/
        }

        private void ApplyRegex(IEnumerable<RegexMatchTarget> regexTargets)
        {
            int matches = 0, unmatches = 0;

            foreach (var isMatch in regexTargets.Select(regexTarget => regexTarget.ApplyRegex(regex)))
            {
                if (isMatch == true)
                {
                    ++matches;
                }
                else if (isMatch == false)
                {
                    ++unmatches;
                }
            }

            MatchCount = matches;
            UnmatchCount = unmatches;
        }

        private void GenerateItems()
        {
            Clear();

            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            var lines = input.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            var regexTargets = lines.Select(line => new RegexMatchTarget(line)).ToArray();
            
            ApplyRegex(regexTargets);

            foreach (var regexTarget in regexTargets)
            {
                Add(regexTarget);
            }
        }
        #endregion
    }
}