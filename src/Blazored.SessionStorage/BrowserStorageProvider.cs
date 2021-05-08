using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazored.SessionStorage
{
    internal class BrowserStorageProvider : IStorageProvider
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly IJSInProcessRuntime _jSInProcessRuntime;

        public BrowserStorageProvider(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
            _jSInProcessRuntime = jSRuntime as IJSInProcessRuntime;
        }

        public ValueTask ClearAsync()
            => _jSRuntime.InvokeVoidAsync("sessionStorage.clear");

        public ValueTask<string> GetItemAsync(string key)
            => _jSRuntime.InvokeAsync<string>("sessionStorage.getItem", key);

        public ValueTask<string> KeyAsync(int index)
            => _jSRuntime.InvokeAsync<string>("sessionStorage.key", index);

        public ValueTask<bool> ContainKeyAsync(string key)
            => _jSRuntime.InvokeAsync<bool>("sessionStorage.hasOwnProperty", key);

        public ValueTask<int> LengthAsync()
            => _jSRuntime.InvokeAsync<int>("eval", "sessionStorage.length");

        public ValueTask RemoveItemAsync(string key)
            => _jSRuntime.InvokeVoidAsync("sessionStorage.removeItem", key);

        public ValueTask SetItemAsync(string key, string data)
            => _jSRuntime.InvokeVoidAsync("sessionStorage.setItem", key, data);

        public void Clear()
        {
            CheckForInProcessRuntime();
            _jSInProcessRuntime.InvokeVoid("sessionStorage.clear");
        }

        public string GetItem(string key)
        {
            CheckForInProcessRuntime();
            return _jSInProcessRuntime.Invoke<string>("sessionStorage.getItem", key);
        }

        public string Key(int index)
        {
            CheckForInProcessRuntime();
            return _jSInProcessRuntime.Invoke<string>("sessionStorage.key", index);
        }

        public bool ContainKey(string key)
        {
            CheckForInProcessRuntime();
            return _jSInProcessRuntime.Invoke<bool>("sessionStorage.hasOwnProperty", key);
        }

        public int Length()
        {
            CheckForInProcessRuntime();
            return _jSInProcessRuntime.Invoke<int>("eval", "sessionStorage.length");
        }

        public void RemoveItem(string key)
        {
            CheckForInProcessRuntime();
            _jSInProcessRuntime.InvokeVoidAsync("sessionStorage.removeItem", key);
        }

        public void SetItem(string key, string data)
        {
            CheckForInProcessRuntime();
            _jSInProcessRuntime.InvokeVoid("sessionStorage.setItem", key, data);
        }

        private void CheckForInProcessRuntime()
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");
        }
    }
}
