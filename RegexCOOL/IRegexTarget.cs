using System.Text.RegularExpressions;


namespace CB.RegexCOOL
{
    public interface IRegexTarget
    {
        string Error { get; }

        string Input { get; set; }

        RegexOptions Options { get; set; }

        string Pattern { get; set; }
    }
}