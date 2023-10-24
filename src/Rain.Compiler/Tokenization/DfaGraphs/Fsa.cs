using Rain.Compiler.Models.Tokenization.Constants;
using Rain.Compiler.Models.Tokenization.Enums;
using Rain.Compiler.Models.Tokenization.Tokens;
using Rain.Compiler.Models.Tokenization;
using Advanced.Algorithms.DataStructures.Graph.AdjacencyList;
using Rain.Compiler.Models.Tokenization.Constants.Fsa;

namespace Rain.Compiler.Tokenization.DfaGraphs;

internal class Fsa : WeightedDiGraph<FsaGraphNode, FsaGraphEdge>
{
    protected readonly FsaGraphNode _start;
    protected FsaState CurrentState { get; set; } 

    public FsaStatus Status { get; private set; } = FsaStatus.Initial;

    public FsaErrorDetails? FsaErrorDetails { get; private set; }

    internal Fsa()
    {
        _start = new FsaGraphNode(CommonFsaStateNames.Start);
        CurrentState = new FsaState(_start, string.Empty);
    }

    public virtual Token GetToken()
    {
        return new CharToken()
        {
            Raw = CurrentState.CharsRead
        };
    }

    public virtual void Read(int position, char @char)
    {
        if (Status == FsaStatus.Error || Status == FsaStatus.Final)
        {
            throw new InvalidOperationException(ExceptionMessages.InvalidState);
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

    public virtual void Reset()
    {
        CurrentState = new FsaState(_start, string.Empty);
        Status = FsaStatus.Initial;
    }

    public virtual void ReadEndOfCode()
    {
        if (Status == FsaStatus.Error)
        {
            throw new InvalidOperationException(ExceptionMessages.InvalidState);
        }

        Status = CurrentState.CanEnd ? FsaStatus.Final : FsaStatus.Error;
    }
}
