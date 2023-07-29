using Advanced.Algorithms.DataStructures.Graph.AdjacencyList;
using Rain.Compiler.Models.Tokenization;
using Rain.Compiler.Models.Tokenization.Enums;
using Rain.Compiler.Models.Tokenization.Tokens;
using Rain.Compiler.Tokenization.DfaGraphs.Interface;

namespace Rain.Compiler.Tokenization.DFAGraphs;

/// <summary>
/// This FSA only allows a valid single c character\
/// // A valid c char is exactly 8 bits in length.
///
/// Three types of valid c chars that are 8-bit in length are a regular character, 
/// an one to three digit octat or an one to two digit hex.
/// 
/// References:
/// https://en.wikipedia.org/wiki/Escape_sequences_in_C
/// </summary>
internal class CharFsa : WeightedDiGraph<FsaGraphNode, FsaGraphEdge>, IFsa
{
    private readonly FsaGraphNode _start;

    private readonly FsaGraphNode _backslash;
    private readonly FsaGraphNode _character;

    private readonly FsaGraphNode _oneDigitOctal;
    private readonly FsaGraphNode _twoDigitOctal;
    private readonly FsaGraphNode _threeDigitOctal;

    private readonly FsaGraphNode _hexPrefix;
    private readonly FsaGraphNode _oneDigitHex;
    private readonly FsaGraphNode _twoDigitHex;

    internal CharFsa()
    {
        _start = new FsaGraphNode("Start");

        _backslash = new FsaGraphNode("Backslash");
        _character = new FsaGraphNode("Character", true, true);

        _oneDigitOctal = new FsaGraphNode("OneDigitOctal", true);
        _twoDigitOctal = new FsaGraphNode("TwoDigitOctal", true);
        _threeDigitOctal = new FsaGraphNode("ThreeDigitOctal", true, true);

        _hexPrefix = new FsaGraphNode("HexPrefix");
        _oneDigitHex = new FsaGraphNode("OneDigitHex", true);
        _twoDigitHex = new FsaGraphNode("TwoDigitHex", true, true);

        AddVertex(_start);
        AddVertex(_backslash);
        AddVertex(_character);
        AddVertex(_oneDigitOctal);
        AddVertex(_twoDigitOctal);
        AddVertex(_threeDigitOctal);
        AddVertex(_hexPrefix);
        AddVertex(_oneDigitHex);
        AddVertex(_twoDigitHex);

        AddEdge(_start, _character, new(@"[^\n\\\']"));

        AddEdge(_start, _backslash, new(@"\\"));
        AddEdge(_backslash, _character, new(@"[abefnrtv\\\'\" + "?]"));

        AddEdge(_backslash, _oneDigitOctal, new("[0-7]"));
        AddEdge(_oneDigitOctal, _twoDigitOctal, new("[0-7]"));
        AddEdge(_twoDigitOctal, _threeDigitOctal, new("[0-7]"));

        AddEdge(_backslash, _hexPrefix, new("[xX]"));
        AddEdge(_hexPrefix, _oneDigitHex, new("[a-fA-F0-9]"));
        AddEdge(_oneDigitHex, _twoDigitHex, new("[a-fA-F0-9]"));

        CurrentState = new FsaState(_start, string.Empty);
    }

    private FsaState CurrentState { get; set; }

    public FsaStatus Status { get; private set; } = FsaStatus.Initial;

    public Token GetToken()
    {
        return new CharToken()
        {
            Raw = CurrentState.CharsRead
        };
    }

    public void Read(char @char)
    {
        if (Status == FsaStatus.Error || Status == FsaStatus.Final)
        {
            throw new InvalidOperationException("Fsa is in invalid state");
        }

        var currentVertex = GetVertex(CurrentState.CurrentVertex);
        var possibleNextVertices = currentVertex.OutEdges;

        bool stateChanged = false;
        foreach (var possibleVertex in possibleNextVertices)
        {
            var matchRegex = possibleVertex.Weight<FsaGraphEdge>().MatchingRegex;
            if (matchRegex.IsMatch(@char.ToString()))
            {
                CurrentState = new FsaState(possibleVertex.TargetVertexKey, CurrentState.CharsRead + @char.ToString());
                stateChanged = true;
                break;
            }
        }

        if (!stateChanged)
        {
            Status = FsaStatus.Error;
            return;
        }

        Status = CurrentState.IsEnd ? FsaStatus.Final : FsaStatus.Running;

    }

    public void Reset()
    {
        CurrentState = new FsaState(_start, string.Empty);
        Status = FsaStatus.Initial;
    }

    public void ReadEndOfCode()
    {
        if (Status == FsaStatus.Error || Status == FsaStatus.Final)
        {
            throw new InvalidOperationException("Fsa is in invalid state");
        }

        Status = CurrentState.CanEnd ? FsaStatus.Final : FsaStatus.Error;
    }
}
