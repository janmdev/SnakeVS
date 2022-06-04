using MessagePack;

namespace SnakeVS.Shared
{
    public class SnakeFood
    {
        [Key(7)]
        public int PositionX { get; set; }
        [Key(8)]
        public int PositionY { get; set; }
    }
}
