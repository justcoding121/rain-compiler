using Rain.Compiler.Models.Tokenization.Enums;

namespace Rain.Compiler.Models.Tokenization.Tokens;

public abstract class Token
{
    internal string? RawContent { get; set; }
    internal TokenKind Kind { get; set; }
}
