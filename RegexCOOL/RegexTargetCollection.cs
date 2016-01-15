using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;


namespace CB.RegexCOOL
{
    public abstract class RegexTargetCollection : ObservableCollection<RegexMatchTarget>, IRegexTarget
    {
        #region Fields
        protected Regex regex;
        #endregion


        #region Properties
        protected string error;

        public virtual string Error
        {
            get { return error; }
            set { SetProperty(ref error, value); }
        }

        protected string input;

        public virtual string Input
        {
            get { return input; }
            set { SetProperty(ref input, value); }
        }

        protected RegexOptions options = RegexOptions.None;

        public virtual RegexOptions Options
        {
            get { return options; }
            set { SetProperty(ref options, value); }
        }

        protected string pattern;

        public virtual string Pattern
        {
            get { return pattern; }
            set
            {
                SetProperty(ref pattern, value);
                ApplyPattern();
            }
        }
        #endregion


        #region Events
        public new event PropertyChangedEventHandler PropertyChanged
        {
            add { base.PropertyChanged += value; }
            remove { base.PropertyChanged -= value; }
        }
        #endregion


        #region Methods
        #endregion


        #region Implementation
        protected virtual void ApplyPattern()
        {
            regex = null;

            if (!string.IsNullOrEmpty(pattern))
            {
                try
                {
                    regex = new Regex(pattern);
                    Error = null;
                }
                catch (Exception exception)
                {
                    Error = exception.Message;
                }
            }

            ApplyRegex();
        }

        protected abstract void ApplyRegex();

        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }
        #endregion
    }
}