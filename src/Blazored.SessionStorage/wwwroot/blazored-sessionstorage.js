window.blazoredSessionStorage = {
    setItem: function (key, data) {
        sessionStorage.setItem(key, data);
    },
    getItem: function (key) {
        return sessionStorage.getItem(key);
    },
    removeItem: function (key) {
        sessionStorage.removeItem(key);
    },
    clear: function () {
        sessionStorage.clear();
    },
    length: function () {
        return sessionStorage.length;
    },
    key: function (index) {
        return sessionStorage.key(index);
    }
};