using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rain.Compiler.Models.Tokenization;

internal record FsaState
{
    internal FsaState(string currentVertex, FsaGraphEdge? lastEdge, string charsRead)
    {
        CurrentVertex = currentVertex;
        LastEdge = lastEdge;
        CharsRead = charsRead;
    }

    internal string CurrentVertex { get; set; }
    internal FsaGraphEdge? LastEdge { get; set; }
    internal bool CanEnd => LastEdge!= null && LastEdge.CanEnd;
    internal string CharsRead { get; set; }
}
