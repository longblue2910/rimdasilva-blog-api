using System.Globalization;

namespace Contracts.Exceptions;

/// <summary>
/// Exception type for not found exceptions
/// </summary>
public class NotFoundException : Exception
{
    public const string ErrorCode = "error_code";

    public NotFoundException()
    { }

    public NotFoundException(string message)
        : base(message)
    { }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    { }

    public NotFoundException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture,
       message, args))
    {
    }

    public NotFoundException(string message, int code) : base(message)
    {
        Data.Add(ErrorCode, code);
    }
}
