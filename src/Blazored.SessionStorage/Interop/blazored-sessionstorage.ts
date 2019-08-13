namespace Blazored.SessionStorage {
    class SessionStorage {
        public SetItem(key: string, data: string): void {
            window.sessionStorage.setItem(key, data);
        }

        public GetItem(key: string): string {
            return window.sessionStorage.getItem(key);
        }

        public RemoveItem(key: string): void {
            window.sessionStorage.removeItem(key);
        }

        public Clear(): void {
            window.sessionStorage.clear();
        }

        public Length(): number {
            return window.sessionStorage.length;
        }

        public Key(index: number): string {
            return window.sessionStorage.key(index);
        }
    }

    export function Load(): void {
        const sessionStorage = {
            SessionStorage: new SessionStorage()
        };

        if (window['Blazored']) {
            window['Blazored'] = {
                ...window['Blazored'],
                ...sessionStorage
            }
        } else {
            window['Blazored'] = {
                ...sessionStorage
            }
        }
    }
}

Blazored.SessionStorage.Load();
