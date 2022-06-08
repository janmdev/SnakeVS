using MessagePack;

namespace SnakeVS.Shared
{
    [MessagePackObject]
    public class SnakeFood
    {
        [Key(0)]
        public int PositionX { get; set; }
        [Key(1)]
        public int PositionY { get; set; }
    }
}
