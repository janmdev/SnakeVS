using Blazored.LocalStorage;

namespace SnakeVS.Client
{
    public class LocalStorageService
    {
        private readonly ILocalStorageService localStorage;

        public LocalStorageService(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
        }

        public async Task<string> GetUserName()
        {
            return await localStorage.GetItemAsStringAsync("userName");
        }

        public async Task SetUserName(string userName)
        {
            await localStorage.SetItemAsStringAsync("userName", userName);
        }

        public async Task<Guid> GetLastRoom()
        {
            return Guid.Parse(await localStorage.GetItemAsStringAsync("lastRoom"));
        }

        public async Task SetLastRoom(Guid roomGuid)
        {
            await localStorage.SetItemAsStringAsync("lastRoom", roomGuid.ToString());
        }
    }
}
