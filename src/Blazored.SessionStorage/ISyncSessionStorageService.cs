﻿using System;
using System.Collections.Generic;

namespace Blazored.SessionStorage
{
    public interface ISyncSessionStorageService
    {
        void Clear();

        T GetItem<T>(string key);

        string Key(int index);

        int Length();

        void RemoveItem(string key);

        void SetItem<T>(string key, T data);

        IEnumerable<string> GetKeys();

        event EventHandler<ChangingEventArgs> Changing;
        event EventHandler<ChangedEventArgs> Changed;
    }
}