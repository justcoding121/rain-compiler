
using Rain.Compiler.Models.Tokenization.Enums;

namespace Rain.Compiler.Models.Tokenization.Tokens;

public class NoneToken : Token
{
    public NoneToken()
    {
        Kind = TokenKind.None;
    }
}
