using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blazored.SessionStorage
{
    public interface ISessionStorageService
    {
        ValueTask ClearAsync();

        ValueTask<T> GetItemAsync<T>(string key);

        IAsyncEnumerable<T> GetValuesAsync<T>(CancellationToken cancellationToken = default);

        IAsyncEnumerable<string> GetKeysAsync(CancellationToken cancellationToken = default);

        ValueTask<string> KeyAsync(int index);

        ValueTask<int> LengthAsync();

        ValueTask RemoveItemAsync(string key);

        ValueTask SetItemAsync<T>(string key, T data);

        event EventHandler<ChangingEventArgs> Changing;
        event EventHandler<ChangedEventArgs> Changed;
    }
}
