

namespace SnakeVS.Shared
{

    public class Snake
    {
        public uint PlayerNumber { get; set; }
        public List<SnakeNode> Nodes { get; set; }
        public Direction Dir { get; set; }

        public Snake(uint playerNumber, List<SnakeNode> nodes)
        {
            PlayerNumber = playerNumber;
            Nodes = nodes;
        }
        public Task Move()
        {
            for (int j = Nodes.Count - 1; j > 0; j--)
            {
                Nodes[j].PositionX = Nodes[j - 1].PositionX;
                Nodes[j].PositionY = Nodes[j - 1].PositionY;
                Console.WriteLine($" {j} - {Nodes[j].PositionX} : {Nodes[j].PositionY}");
            }

            return Task.CompletedTask;
        }
        public async Task MoveDown()
        {
            await Move();
            Nodes[0].PositionY++;
        }
        public async Task MoveUp()
        {
            await Move();
            Nodes[0].PositionY--;
        }

        public async Task MoveLeft()
        {
            await Move();
            Nodes[0].PositionX--;
        }

        public async Task MoveRight()
        {
            await Move();
            Nodes[0].PositionX++;
        }

        public void changeDirection(Direction toDir)
        {
            switch (toDir)
            {
                case Direction.Left:
                    switch (this.Dir)
                    {
                        case Direction.Up:
                            this.Dir = Direction.Left;
                            break;
                        case Direction.Down:
                            this.Dir = Direction.Right;
                            break;
                        case Direction.Left:
                            this.Dir = Direction.Down;
                            break;
                        case Direction.Right:
                            this.Dir = Direction.Up;
                            break;
                    }
                    break;
                case Direction.Right:
                    switch (this.Dir)
                    {
                        case Direction.Up:
                            this.Dir = Direction.Right;
                            break;
                        case Direction.Down:
                            this.Dir = Direction.Left;
                            break;
                        case Direction.Left:
                            this.Dir = Direction.Up;
                            break;
                        case Direction.Right:
                            this.Dir = Direction.Down;
                            break;
                    }
                    break;
            }
        }

        public SnakeNode Head => Nodes.First(p => p.IsHead);
        public void Consume(SnakeFood sf)
        {
            Nodes.Insert(0, new SnakeNode(true, sf.PositionX, sf.PositionY));
            Nodes[1].IsHead = false;
        }
    }

    public class SnakeNode
    {
        public SnakeNode(bool isHead, int positionX, int positionY)
        {
            IsHead = isHead;
            PositionX = positionX;
            PositionY = positionY;
        }
        public bool IsHead { get; set; }
        private int positionX;
        public int PositionX
        {
            get
            {
                return positionX;
            }
            set
            {
                if (value > 11)
                {
                    positionX = 11;
                }
                else if (value < -1)
                {
                    positionX = -1;
                }
                else positionX = value;
            }
        }

        private int positionY;
        public int PositionY
        {
            get
            {
                return positionY;
            }
            set
            {
                if (value > 11)
                {
                    positionY = 11;
                }
                else if (value < -1)
                {
                    positionY = -1;
                }
                else positionY = value;
            }
        }
    }
    public enum Direction
    {
        Up, Down, Left, Right
    }
}
