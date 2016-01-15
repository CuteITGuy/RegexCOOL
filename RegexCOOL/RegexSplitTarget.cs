using System;
using System.Text.RegularExpressions;
using GC.Data;


namespace CB.RegexCOOL
{
    public class RegexSplitTarget : ObservableObject, IRegexTarget
    {
        #region Fields & Properties
        private Regex regex;

        private string error;

        public string Error
        {
            get { return error; }
            private set { SetProperty(ref error, value); }
        }

        private string input;

        public string Input
        {
            get { return input; }
            set
            {
                SetProperty(ref input, value);
                ApplyRegex();
            }
        }

        private RegexOptions options = RegexOptions.None;

        public RegexOptions Options
        {
            get { return options; }
            set
            {
                SetProperty(ref options, value);
                CreateRegex();
                ApplyRegex();
            }
        }

        private string pattern;

        public string Pattern
        {
            get { return pattern; }
            set
            {
                SetProperty(ref pattern, value);
                CreateRegex();
                ApplyRegex();
            }
        }

        private string[] result;

        public string[] Result
        {
            get { return result; }
            private set { SetProperty(ref result, value); }
        }
        #endregion


        #region Constructors
        public RegexSplitTarget()
        {
        }

        public RegexSplitTarget(string input)
        {
            this.input = input;
        }

        public RegexSplitTarget(string input, string pattern)
            : this(input, pattern, RegexOptions.None)
        {
        }

        public RegexSplitTarget(string input, string pattern, RegexOptions options)
            : this(input)
        {
            this.pattern = pattern;
            this.options = options;
            CreateRegex();
            ApplyRegex();
        }
        #endregion


        #region Implementation
        private void ApplyRegex()
        {
            if (regex == null || string.IsNullOrEmpty(input))
            {
                Result = null;
                return;
            }

            DoActionAndSetError(() => Result = regex.Split(input));
        }

        private void CreateRegex()
        {
            regex = null;

            DoActionAndSetError(() =>
            {
                if (!string.IsNullOrWhiteSpace(pattern))
                {
                    regex = new Regex(pattern, options);
                }
            });
        }

        private void DoActionAndSetError(Action action)
        {
            try
            {
                action();
                Error = "";
            }
            catch (Exception exception)
            {
                Error = exception.Message;
                Result = null;
            }
        }
        #endregion
    }
}