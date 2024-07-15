﻿using System.Globalization;

namespace Contracts.Exceptions;

public class UnauthorizedException : Exception
{
    public const string ErrorCode = "error_code";
    public UnauthorizedException()
    { }

    public UnauthorizedException(string message)
        : base(message)
    { }

    public UnauthorizedException(string message, Exception innerException)
        : base(message, innerException)
    { }


    public UnauthorizedException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture,
       message, args))
    {
    }


    public UnauthorizedException(string message, int code) : base(message)
    {
        Data.Add(ErrorCode, code);
    }
}
