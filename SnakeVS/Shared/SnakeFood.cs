using MessagePack;

namespace SnakeVS.Shared
{
    [MessagePackObject]
    public class SnakeFood
    {
        [Key(0)]
        public sbyte PositionX { get; set; }
        [Key(1)]
        public sbyte PositionY { get; set; }
    }
}
