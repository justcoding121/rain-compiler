using System.Text.RegularExpressions;

namespace Rain.Compiler.Models.Tokenization;

internal record FsaGraphEdge : IComparable
{
    internal FsaGraphEdge(string matchingRegexPattern, bool canEnd = false)
    {
        MatchingRegex = new Regex(matchingRegexPattern);
        CanEnd = canEnd;
    }

    internal Regex MatchingRegex { get; set; }
    internal bool CanEnd { get; set; }

    public int CompareTo(object? obj)
    {
        var that = obj as FsaGraphEdge;
        return MatchingRegex == that?.MatchingRegex && CanEnd == that.CanEnd ? 0 : -1;
    }
}
