using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GC.Data;

namespace CB.RegexCOOL
{
    public class RegexMatchTarget : ObservableObject
    {
        #region Fields & Properties
        private IEnumerable<Group> groups;

        public IEnumerable<Group> Groups
        {
            get { return groups; }
            set { SetProperty(ref groups, value); }
        }

        private string input;

        public string Input
        {
            get { return input; }
            set { SetProperty(ref input, value); }
        }

        private bool? isMatch;

        public bool? IsMatch
        {
            get { return isMatch; }
            set { SetProperty(ref isMatch, value); }
        }
        #endregion


        #region Constructors
        public RegexMatchTarget(Match match)
            : this(match.Value, !string.IsNullOrEmpty(match.Value), match.Groups.Cast<Group>())
        {
        }

        public RegexMatchTarget(string input, bool? isMatch, IEnumerable<Group> groups)
        {
            this.groups = groups;
            this.input = input;
            this.isMatch = isMatch;
        }

        public RegexMatchTarget(string text)
            : this(text, null, null)
        {
        }

        public RegexMatchTarget()
        {
        }
        #endregion


        #region Methods
        public bool? ApplyRegex(Regex regex)
        {
            if (regex == null)
            {
                IsMatch = null;
                Groups = null;
            }
            else
            {
                var match = regex.Match(input);
                IsMatch = !string.IsNullOrEmpty(match.Value);
                Groups = match.Groups.Cast<Group>();
                return isMatch;
            }
            return IsMatch;
        }

        public override string ToString()
        {
            return string.Format("({0}: {1})", input, isMatch);
        }
        #endregion
    }
}