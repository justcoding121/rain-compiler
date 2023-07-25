namespace Rain.Compiler.Models.Tokenization;

internal record FsaState
{
    internal FsaState(FsaGraphNode currentVertex, string charsRead)
    {
        CurrentVertex = currentVertex;
        CharsRead = charsRead;
    }

    internal FsaGraphNode CurrentVertex { get; set; }
    internal bool CanEnd => CurrentVertex.CanEnd;
    internal bool IsEnd => CurrentVertex.IsEnd;
    internal string CharsRead { get; set; }
}
