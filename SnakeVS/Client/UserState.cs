using Blazored.LocalStorage;

namespace SnakeVS.Client
{
    public class UserState
    {
        private readonly ILocalStorageService _localStorage;
        public UserState(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            UserName = "tester";
            LastRoom = Guid.NewGuid();
        }
        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                _ = setStorage();
            }
        }
        public Guid LastRoom { get; set; }

        private async Task setStorage() => await _localStorage.SetItemAsync("state", this);
    }
}
