using System.Text.RegularExpressions;

namespace Rain.Compiler.Models.Tokenization;

internal record FsaGraphEdge : IComparable
{
    internal FsaGraphEdge(string matchingRegexPattern)
    {
        MatchingRegex = new Regex(matchingRegexPattern);
    }

    internal Regex MatchingRegex { get; set; }

    public int CompareTo(object? obj)
    {
        throw new InvalidOperationException();  
    }
}
