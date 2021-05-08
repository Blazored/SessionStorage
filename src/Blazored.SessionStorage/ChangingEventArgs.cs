using System;
using System.Diagnostics.CodeAnalysis;

namespace Blazored.SessionStorage
{
    [ExcludeFromCodeCoverage]
    public class ChangingEventArgs : ChangedEventArgs
    {
        public bool Cancel { get; set; }
    }
}
