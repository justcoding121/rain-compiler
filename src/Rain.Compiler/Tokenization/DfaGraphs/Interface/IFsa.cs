using Rain.Compiler.Models.Enums;

namespace Rain.Compiler.Tokenization.DfaGraphs.Interface;

public interface IFsa
{
    void Read(char @char);
    FsaStatus Status { get; } 
}
