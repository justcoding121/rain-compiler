using Rain.Compiler.Models.Tokenization;
using Rain.Compiler.Models.Tokenization.Enums;
using Rain.Compiler.Models.Tokenization.Tokens;

namespace Rain.Compiler.Tokenization.DfaGraphs.Interface;

public interface IFsa
{
    void Read(int position, char @char);
    FsaStatus Status { get; }
    void Reset();
    void ReadEndOfCode();
    Token GetToken();
    FsaErrorDetails? FsaErrorDetails { get; }
}
