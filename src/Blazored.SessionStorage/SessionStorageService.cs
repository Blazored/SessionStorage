using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Blazored.SessionStorage.StorageOptions;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Blazored.SessionStorage
{
    public class SessionStorageService : ISessionStorageService, ISyncSessionStorageService
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly IJSInProcessRuntime _jSInProcessRuntime;
        private readonly JsonSerializerOptions _jsonOptions;

        public event EventHandler<ChangingEventArgs> Changing;
        public event EventHandler<ChangedEventArgs> Changed;

        public SessionStorageService(IJSRuntime jSRuntime, IOptions<SessionStorageOptions> options)
        {
            _jSRuntime = jSRuntime;
            _jsonOptions = options.Value.JsonSerializerOptions;
            _jSInProcessRuntime = jSRuntime as IJSInProcessRuntime;
        }

        public async ValueTask SetItemAsync<T>(string key, T data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var e = await RaiseOnChangingAsync(key, data);

            if (e.Cancel)
                return;

            var serialisedData = JsonSerializer.Serialize(data, _jsonOptions);

            await _jSRuntime.InvokeVoidAsync("sessionStorage.setItem", key, serialisedData);

            RaiseOnChanged(key, e.OldValue, data);
        }

        public async ValueTask<T> GetItemAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = await _jSRuntime.InvokeAsync<string>("sessionStorage.getItem", key);

            if (serialisedData == null)
                return default;

            return JsonSerializer.Deserialize<T>(serialisedData, _jsonOptions);
        }

        public ValueTask RemoveItemAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return _jSRuntime.InvokeVoidAsync("sessionStorage.removeItem", key);
        }

        public ValueTask ClearAsync() => _jSRuntime.InvokeVoidAsync("sessionStorage.clear");

        public ValueTask<int> LengthAsync() => _jSRuntime.InvokeAsync<int>("eval", "sessionStorage.length");

        public ValueTask<string> KeyAsync(int index) => _jSRuntime.InvokeAsync<string>("sessionStorage.key", index);

        public void SetItem<T>(string key, T data)
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var e = RaiseOnChangingSync(key, data);

            if (e.Cancel)
                return;

            var serialisedData = JsonSerializer.Serialize(data, _jsonOptions);

            _jSInProcessRuntime.InvokeVoid("sessionStorage.setItem", key, serialisedData);

            RaiseOnChanged(key, e.OldValue, data);
        }

        public T GetItem<T>(string key)
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = _jSInProcessRuntime.Invoke<string>("sessionStorage.getItem", key);

            if (serialisedData == null)
                return default;

            return JsonSerializer.Deserialize<T>(serialisedData, _jsonOptions);
        }

        public void RemoveItem(string key)
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            _jSInProcessRuntime.InvokeVoid("sessionStorage.removeItem", key);
        }

        public void Clear()
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            _jSInProcessRuntime.InvokeVoid("sessionStorage.clear");
        }

        public int Length()
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            return _jSInProcessRuntime.Invoke<int>("eval", "sessionStorage.length");
        }

        public string Key(int index)
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            return _jSInProcessRuntime.Invoke<string>("sessionStorage.key", index);
        }

        public IEnumerable<T> GetItems<T>()
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            var index = 0;

            for (var key = Key(index++); key != default; key = Key(index++))
            {
                var serialisedData = _jSInProcessRuntime.Invoke<string>("sessionStorage.getItem", key);

                if (!string.IsNullOrWhiteSpace(serialisedData))
                {
                    yield return JsonSerializer.Deserialize<T>(serialisedData, _jsonOptions);
                }
            }
        }

        public IEnumerable<string> GetKeys()
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            var index = 0;

            for (var key = Key(index++); key != default; key = Key(index++))
            {
                yield return key;
            }
        }

        public async IAsyncEnumerable<T> GetValuesAsync<T>([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var index = 0;

            for (var key = await KeyAsync(index++); key != default; key = await KeyAsync(index++))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new TaskCanceledException();
                }

                var serialisedData = await _jSRuntime.InvokeAsync<string>("sessionStorage.getItem", key);

                if (cancellationToken.IsCancellationRequested)
                {
                    throw new TaskCanceledException();
                }

                if (!string.IsNullOrWhiteSpace(serialisedData))
                {
                    yield return JsonSerializer.Deserialize<T>(serialisedData, _jsonOptions);
                }
            }
        }

        public async IAsyncEnumerable<string> GetKeysAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var index = 0;

            for (var key = await KeyAsync(index++); key != default; key = await KeyAsync(index++))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new TaskCanceledException();
                }

                yield return key;
            }
        }

        private async Task<ChangingEventArgs> RaiseOnChangingAsync(string key, object data)
        {
            var e = new ChangingEventArgs
            {
                Key = key,
                OldValue = await GetItemAsync<object>(key),
                NewValue = data
            };

            Changing?.Invoke(this, e);

            return e;
        }

        private ChangingEventArgs RaiseOnChangingSync(string key, object data)
        {
            var e = new ChangingEventArgs
            {
                Key = key,
                OldValue = ((ISyncSessionStorageService)this).GetItem<object>(key),
                NewValue = data
            };

            Changing?.Invoke(this, e);

            return e;
        }

        private void RaiseOnChanged(string key, object oldValue, object data)
        {
            var e = new ChangedEventArgs
            {
                Key = key,
                OldValue = oldValue,
                NewValue = data
            };

            Changed?.Invoke(this, e);
        }
    }
}
