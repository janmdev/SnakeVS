using Microsoft.AspNetCore.SignalR;

namespace SnakeVS.Server
{
    public class PlayerConnection
    {
        public string BlueClient { get; set; }
        public string BlueName { get; set; }
        public string RedClient { get; set; }
        public string RedName { get; set; }

        public bool Contains(string client)
        {
            return (BlueClient == client || RedClient == client);
        }
    }
}
