using Advanced.Algorithms.DataStructures.Graph.AdjacencyList;
using Rain.Compiler.Models.Tokenization;
using Rain.Compiler.Models.Tokenization.Enums;
using Rain.Compiler.Models.Tokenization.Tokens;
using Rain.Compiler.Tokenization.DfaGraphs.Interface;

namespace Rain.Compiler.Tokenization.DFAGraphs;

/// <summary>
/// This FSA only allows a valid c character.
/// A valid c char is exactly 8 bits in length.
/// References:
/// https://en.wikipedia.org/wiki/Escape_sequences_in_C
/// </summary>
internal class CharFsa : WeightedDiGraph<string, FsaGraphEdge>, IFsa
{
    internal static class CharFsaStates
    {
        internal const string Start = "Start";

        internal const string Backslash = "Backslash";
        internal const string Character = "Character";

        //Max length is three digits for octal char
        internal const string OneDigitOctal = "OneDigitOctal";
        internal const string TwoDigitOctal = "TwoDigitOctal";
        internal const string ThreeDigitOctal = "ThreeDigitOctal";

        //Max length is two digits for hex char
        internal const string HexPrefix = "HexPrefix";
        internal const string OneDigitHex = "OneDigitHex";
        internal const string TwoDigitHex = "TwoDigitHex";
    }

    internal CharFsa()
    {
        AddVertex(CharFsaStates.Start);

        AddVertex(CharFsaStates.Backslash);
        AddVertex(CharFsaStates.Character);

        AddVertex(CharFsaStates.OneDigitOctal);
        AddVertex(CharFsaStates.TwoDigitOctal);
        AddVertex(CharFsaStates.ThreeDigitOctal);

        AddVertex(CharFsaStates.HexPrefix);
        AddVertex(CharFsaStates.OneDigitHex);
        AddVertex(CharFsaStates.TwoDigitHex);

        AddEdge(CharFsaStates.Start, CharFsaStates.Backslash, new FsaGraphEdge("\\"));

        AddEdge(CharFsaStates.Backslash, CharFsaStates.Character, new FsaGraphEdge("[abefnrtv\\\'\"?]", true));

        AddEdge(CharFsaStates.Start, CharFsaStates.Character, new FsaGraphEdge("[^\n\\\']"));

        AddEdge(CharFsaStates.Backslash, CharFsaStates.OneDigitOctal, new FsaGraphEdge("[0-7]", true));
        AddEdge(CharFsaStates.OneDigitOctal, CharFsaStates.TwoDigitOctal, new FsaGraphEdge("[0-7]", true));
        AddEdge(CharFsaStates.TwoDigitOctal, CharFsaStates.ThreeDigitOctal, new FsaGraphEdge("[0-7]", true));

        AddEdge(CharFsaStates.Backslash, CharFsaStates.HexPrefix, new FsaGraphEdge("[xX]"));
        AddEdge(CharFsaStates.HexPrefix, CharFsaStates.OneDigitHex, new FsaGraphEdge("[a-fA-F0-9]", true));
        AddEdge(CharFsaStates.OneDigitHex, CharFsaStates.TwoDigitHex, new FsaGraphEdge("[a-fA-F0-9]", true));
    }

    private FsaState CurrentState { get; set; } = new FsaState(CharFsaStates.Start, null, string.Empty)
    {
        CurrentVertex = CharFsaStates.Start
    };

    public FsaStatus Status { get; set; } = FsaStatus.Initial;

    public Token GetToken()
    {
        return new NoneToken()
        {
            Content = CurrentState.CharsRead
        };
    }

    public void Read(char @char)
    {
        Status = FsaStatus.Running;

        var currentVertex = GetVertex(CurrentState.CurrentVertex);
        var possibleNextVertices = currentVertex.OutEdges;

        bool stateChanged = false;
        foreach (var possibleVertex in possibleNextVertices)
        {
            var edge = possibleVertex.Weight<FsaGraphEdge>();
            if (edge.MatchingRegex.IsMatch(@char.ToString()))
            {
                CurrentState = new FsaState(possibleVertex.TargetVertexKey, edge, CurrentState.CharsRead + @char.ToString());
                stateChanged = true;
                break;
            }
        }

        if (!stateChanged)
        {
            if (CurrentState.CanEnd)
                Status = FsaStatus.Final;
            else
                Status = FsaStatus.Error;
        }
    }

    public void Reset()
    {
        CurrentState = new FsaState(CharFsaStates.Start, null, string.Empty);
    }
}
