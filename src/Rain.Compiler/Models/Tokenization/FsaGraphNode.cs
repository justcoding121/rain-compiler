namespace Rain.Compiler.Models.Tokenization;
internal class FsaGraphNode
{
    internal FsaGraphNode(string name, bool canEnd = false, bool isEnd = false)
    {
        Name = name;
        CanEnd = canEnd;
        IsEnd = isEnd;
    }

    internal string Name { get; set; }
    internal bool CanEnd { get; set; }
    internal bool IsEnd { get; set; }
}
