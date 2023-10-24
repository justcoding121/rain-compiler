using Advanced.Algorithms.DataStructures.Graph.AdjacencyList;
using Rain.Compiler.Models.Tokenization;
using Rain.Compiler.Models.Tokenization.Constants;
using Rain.Compiler.Models.Tokenization.Constants.Fsa;
using Rain.Compiler.Models.Tokenization.Enums;
using Rain.Compiler.Models.Tokenization.Tokens;
using Rain.Compiler.Tokenization.DfaGraphs;
using Rain.Compiler.Tokenization.DfaGraphs.Interface;

namespace Rain.Compiler.Tokenization.DFAGraphs;

/// <summary>
/// This FSA only allows a valid single c character.
/// A valid c char is exactly 8 bits in length (2^8 = 255 possible values).
///
/// Three types of valid c chars are a regular character, 
/// an octat (1-3 digits) or a hex (1-2 digits).
/// 
/// References:
/// https://en.wikipedia.org/wiki/Escape_sequences_in_C
/// </summary>
internal class CharFsa : Fsa, IFsa
{
    private readonly FsaGraphNode _backslash;
    private readonly FsaGraphNode _character;

    private readonly FsaGraphNode _oneDigitOctal;
    private readonly FsaGraphNode _twoDigitOctal;
    private readonly FsaGraphNode _threeDigitOctal;

    private readonly FsaGraphNode _hexPrefix;
    private readonly FsaGraphNode _oneDigitHex;
    private readonly FsaGraphNode _twoDigitHex;

    internal CharFsa() : base()
    {    
        _backslash = new FsaGraphNode(CharFsaStateNames.Backslash);
        _character = new FsaGraphNode(CharFsaStateNames.Character, true, true);

        _oneDigitOctal = new FsaGraphNode(CharFsaStateNames.OneDigitOctal, true);
        _twoDigitOctal = new FsaGraphNode(CharFsaStateNames.TwoDigitOctal, true);
        _threeDigitOctal = new FsaGraphNode(CharFsaStateNames.ThreeDigitOctal, true, true);

        _hexPrefix = new FsaGraphNode(CharFsaStateNames.HexPrefix);
        _oneDigitHex = new FsaGraphNode(CharFsaStateNames.OneDigitHex, true);
        _twoDigitHex = new FsaGraphNode(CharFsaStateNames.TwoDigitHex, true, true);

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
    }
}
