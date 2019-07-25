var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var Blazored;
(function (Blazored) {
    var SessionStorage;
    (function (SessionStorage_1) {
        var SessionStorage = /** @class */ (function () {
            function SessionStorage() {
            }
            SessionStorage.prototype.SetItem = function (key, data) {
                window.sessionStorage.setItem(key, data);
            };
            SessionStorage.prototype.GetItem = function (key) {
                return window.sessionStorage.getItem(key);
            };
            SessionStorage.prototype.RemoveItem = function (key) {
                window.sessionStorage.removeItem(key);
            };
            SessionStorage.prototype.Clear = function () {
                window.sessionStorage.clear();
            };
            SessionStorage.prototype.Length = function () {
                return window.sessionStorage.length;
            };
            SessionStorage.prototype.Key = function (index) {
                return window.sessionStorage.key(index);
            };
            return SessionStorage;
        }());
        function Load() {
            var sessionStorage = {
                SessionStorage: new SessionStorage()
            };
            if (window['Blazored']) {
                window['Blazored'] = __assign({}, window['Blazored'], sessionStorage);
            }
            else {
                window['Blazored'] = __assign({}, sessionStorage);
            }
        }
        SessionStorage_1.Load = Load;
    })(SessionStorage = Blazored.SessionStorage || (Blazored.SessionStorage = {}));
})(Blazored || (Blazored = {}));
Blazored.SessionStorage.Load();
//# sourceMappingURL=blazored-sessionstorage.js.map