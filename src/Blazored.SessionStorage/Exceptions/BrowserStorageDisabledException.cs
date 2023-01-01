using System;

namespace Blazored.SessionStorage.Exceptions;

public class BrowserStorageDisabledException : Exception
{
    public BrowserStorageDisabledException()
    {
    }

    public BrowserStorageDisabledException(string message) : base(message)
    {
    }

    public BrowserStorageDisabledException(string message, Exception inner) : base(message, inner)
    {
    }
}
