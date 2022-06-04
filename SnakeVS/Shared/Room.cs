using MessagePack;
namespace SnakeVS.Shared
{
    [MessagePackObject]
    public class Room
    {
        [Key(0)]
        public Guid Guid { get; set; }
        [Key(1)]
        public string? Name { get; set; }
        [Key(2)]
        public string? BlueName { get; set; }

        [Key(3)]
        public string RedName { get; set; }
        [Key(4)]
        public Snake RedSnake { get; set; }
        [Key(5)]
        public Snake BlueSnake { get; set; }
        [Key(6)]
        public List<SnakeFood> foodList { get; set; }
    }
}
