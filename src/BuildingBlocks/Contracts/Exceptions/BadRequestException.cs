using System.Globalization;

namespace Contracts.Exceptions;

public class BadRequestException : Exception
{
    public const string ErrorCode = "error_code";
    public BadRequestException()
    {

    }
    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture,
        message, args))
    {
    }

    public BadRequestException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public BadRequestException(string message, int code) : base(message)
    {
        Data.Add(ErrorCode, code);
    }
}
