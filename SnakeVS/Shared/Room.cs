using MessagePack;
namespace SnakeVS.Shared
{
    [MessagePackObject]
    public class Room
    {
        public Room()
        {
            FoodList = new List<SnakeFood>();
            BlueSnake = new Snake(1, new List<SnakeNode>()
            {
                new SnakeNode(true, 0, 2), 
                new SnakeNode(false, 0, 1), 
                new SnakeNode(false, 0, 0)
            }, Direction.Down);
            RedSnake = new Snake(2, new List<SnakeNode>()
            {
                new SnakeNode(true, 10, 8), 
                new SnakeNode(false, 10, 9), 
                new SnakeNode(false, 10, 10)
            }, Direction.Up);
        }
        [Key(0)]
        public Guid Guid { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public string BlueName { get; set; }
        [Key(3)]
        public string RedName { get; set; }
        [Key(4)]
        public Snake RedSnake { get; set; }
        [Key(5)]
        public Snake BlueSnake { get; set; }
        [Key(6)] 
        public List<SnakeFood> FoodList { get; set; }

        [Key(7)]
        public GameState State { get; set; }


        public void SpawnFood()
        {
            int limit = 12 * 12 - FoodList.Count - BlueSnake.Nodes.Count - RedSnake.Nodes.Count;
            Random rng = new Random();
            int foodIndex = rng.Next(limit);
            int k = 0;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    if (!FoodList.Any(p => p.PositionX == i && p.PositionY == j) &&
                        !BlueSnake.Nodes.Any(p => p.PositionX == i && p.PositionY == j) &&
                        !RedSnake.Nodes.Any(p => p.PositionX == i && p.PositionY == j))
                    {
                        if (k == foodIndex)
                        {
                            FoodList.Add(new SnakeFood()
                            {
                                PositionX = (sbyte)i,
                                PositionY = (sbyte)j
                            });
                            return;
                        }
                        else k++;
                    }
                }
            }
        }

        public void Reset()
        {
            FoodList = new List<SnakeFood>();
            BlueSnake = new Snake(1, new List<SnakeNode>()
            {
                new SnakeNode(true, 0, 2),
                new SnakeNode(false, 0, 1),
                new SnakeNode(false, 0, 0)
            }, Direction.Down);
            RedSnake = new Snake(2, new List<SnakeNode>()
            {
                new SnakeNode(true, 10, 8),
                new SnakeNode(false, 10, 9),
                new SnakeNode(false, 10, 10)
            }, Direction.Up);
            State = GameState.Init;
        }
    }

    public enum GameState
    {
        Init = 0,
        Paused = 1,
        Playing = 2,
        BlueWon = 3,
        RedWon = 4
    }

    public enum Player
    {
        Blue = 0,
        Red = 1
    }
    

}
