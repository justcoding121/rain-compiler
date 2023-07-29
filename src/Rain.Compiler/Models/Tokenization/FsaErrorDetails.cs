namespace Rain.Compiler.Models.Tokenization;

public class FsaErrorDetails
{
    public FsaErrorDetails(string errorCode, string message)
    {
        ErrorCode = errorCode;
        Message = message;
    }

    internal string ErrorCode { get; set; }
    internal string Message { get; set; }
    internal int Row { get; set; }
    internal int Column { get; set; }
}
