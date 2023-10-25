using Rain.Compiler.Models.Tokenization.Constants.Fsa;
using Rain.Compiler.Models.Tokenization;
using Rain.Compiler.Tokenization.DfaGraphs.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rain.Compiler.Tokenization.DfaGraphs;

internal class IntFsa : Fsa, IFsa
{
    private readonly FsaGraphNode _zero;
    private readonly FsaGraphNode _digit;
    private readonly FsaGraphNode _hexPrefix;
    private readonly FsaGraphNode _hex;
    private readonly FsaGraphNode _octat;
    private readonly FsaGraphNode _long;
    private readonly FsaGraphNode _unsigned;
    private readonly FsaGraphNode _unsignedLong;

    internal IntFsa() : base()
    {
        _zero = new FsaGraphNode(IntFsaStateNames.Zero);
        _digit = new FsaGraphNode(IntFsaStateNames.Digit);
        _hexPrefix = new FsaGraphNode(IntFsaStateNames.HexPrefix);
        _hex = new FsaGraphNode(IntFsaStateNames.Hex);
        _octat = new FsaGraphNode(IntFsaStateNames.Octat);
        _long = new FsaGraphNode(IntFsaStateNames.Long);
        _unsigned = new FsaGraphNode(IntFsaStateNames.Unsigned);
        _unsignedLong = new FsaGraphNode(IntFsaStateNames.UnsignedLong);

        AddVertex(_start);

        AddEdge(_start, _zero, new("0"));

        AddEdge(_zero, _long, new("[Ll]"));
        AddEdge(_long, _unsignedLong, new("[Uu]"));

        AddEdge(_zero, _unsigned, new("[Uu]"));
        AddEdge(_unsigned, _unsignedLong, new("[Ll]"));

        AddEdge(_zero, _hexPrefix, new("[Xx]"));
        AddEdge(_hexPrefix, _hex, new("[a-fA-F0-9]"));
        AddEdge(_hex, _hex, new("[a-fA-F0-9]"));
        AddEdge(_hex, _long, new("[Ll]"));
        AddEdge(_hex, _unsigned, new("[Uu]"));

        AddEdge(_zero, _octat, new("[0-7]"));
        AddEdge(_zero, _unsigned, new("[Uu]"));

        AddEdge(_start, _digit, new("[1-9]"));
    }
}
