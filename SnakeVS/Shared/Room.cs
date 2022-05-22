namespace SnakeVS.Shared
{
    public class Room
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string BlueName { get; set; }
        public string RedName { get; set; }
        public Snake RedSnake { get; set; }
        public Snake BlueSnake { get; set; }
        public List<SnakeFood> foodList { get; set; }
    }
}
