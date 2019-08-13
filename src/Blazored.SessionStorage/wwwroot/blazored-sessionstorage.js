var Blazored;
(function (Blazored) {
    var SessionStorage;
    (function (SessionStorage_1) {
        class SessionStorage {
            SetItem(key, data) {
                window.sessionStorage.setItem(key, data);
            }
            GetItem(key) {
                return window.sessionStorage.getItem(key);
            }
            RemoveItem(key) {
                window.sessionStorage.removeItem(key);
            }
            Clear() {
                window.sessionStorage.clear();
            }
            Length() {
                return window.sessionStorage.length;
            }
            Key(index) {
                return window.sessionStorage.key(index);
            }
        }
        function Load() {
            const sessionStorage = {
                SessionStorage: new SessionStorage()
            };
            if (window['Blazored']) {
                window['Blazored'] = Object.assign({}, window['Blazored'], sessionStorage);
            }
            else {
                window['Blazored'] = Object.assign({}, sessionStorage);
            }
        }
        SessionStorage_1.Load = Load;
    })(SessionStorage = Blazored.SessionStorage || (Blazored.SessionStorage = {}));
})(Blazored || (Blazored = {}));
Blazored.SessionStorage.Load();
//# sourceMappingURL=blazored-sessionstorage.js.map