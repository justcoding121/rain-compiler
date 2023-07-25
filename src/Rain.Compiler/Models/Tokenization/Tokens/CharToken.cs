
using Rain.Compiler.Models.Tokenization.Enums;

namespace Rain.Compiler.Models.Tokenization.Tokens;

public class CharToken : Token
{
    public CharToken()
    {
        Kind = TokenKind.Char;
    }
}
