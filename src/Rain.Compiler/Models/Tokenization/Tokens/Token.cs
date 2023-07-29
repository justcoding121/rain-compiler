using Rain.Compiler.Models.Tokenization.Enums;

namespace Rain.Compiler.Models.Tokenization.Tokens;

public abstract class Token
{
    internal TokenKind Kind { get; set; }
    internal string? Raw { get; set; }
}
