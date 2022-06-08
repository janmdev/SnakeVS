using Blazored.LocalStorage;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SnakeVS.Client
{
    public class UserState
    {
        private readonly ILocalStorageService _localStorage;
        public UserState(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            userName = "tester";
            lastRoom = Guid.NewGuid();
        }

        public UserState()
        {
        }

        private string userName;
        private Guid lastRoom;

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                _ = setStorage();
            }
        }

        public Guid LastRoom
        {
            get => lastRoom;
            set => lastRoom = value;
        }

        private async Task setStorage() => await _localStorage.SetItemAsync("state", this);

        public async Task SetupStateFromStorage()
        {
            try
            {
                var storageState = await _localStorage.GetItemAsync<UserState>("state");
                if (storageState == null) return;
                userName = storageState.UserName;
                lastRoom = storageState.LastRoom;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
