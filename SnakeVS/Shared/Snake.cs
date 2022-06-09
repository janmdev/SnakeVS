

using MessagePack;

namespace SnakeVS.Shared
{
    [MessagePackObject]
    public class Snake
    {
        [Key(0)]
        public int PlayerNumber { get; set; }
        [Key(1)]
        public List<SnakeNode> Nodes { get; set; }
        [Key(2)]
        public Direction Dir { get; set; }

        public Snake()
        {

        }

        public Snake(int playerNumber, List<SnakeNode> nodes, Direction dir)
        {
            PlayerNumber = playerNumber;
            Nodes = nodes;
            Dir = dir;
        }
        public Task MoveTail()
        {
            for (int j = Nodes.Count - 1; j > 0; j--)
            {
                Nodes[j].PositionX = Nodes[j - 1].PositionX;
                Nodes[j].PositionY = Nodes[j - 1].PositionY;
            }

            return Task.CompletedTask;
        }
        public async Task MoveDown()
        {
            await MoveTail();
            Nodes[0].PositionY++;
        }
        public async Task MoveUp()
        {
            await MoveTail();
            Nodes[0].PositionY--;
        }

        public async Task MoveLeft()
        {
            await MoveTail();
            Nodes[0].PositionX--;
        }

        public async Task MoveRight()
        {
            await MoveTail();
            Nodes[0].PositionX++;
        }

        public void QueueDirection(Direction toDir)
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

        public async Task Move()
        {
            switch (Dir)
            {
                case Direction.Up:
                    await MoveUp();
                    break;
                case Direction.Down:
                    await MoveDown();
                    break;
                case Direction.Right:
                    await MoveRight();
                    break;
                case Direction.Left:
                    await MoveLeft();
                    break;
            }
        }

        [IgnoreMember]
        public SnakeNode Head => Nodes.First(p => p.IsHead);
        public void Consume(SnakeFood sf)
        {
            Nodes.Insert(0, new SnakeNode(true, sf.PositionX, sf.PositionY));
            Nodes[1].IsHead = false;
        }
    }

    [MessagePackObject]
    public class SnakeNode
    {
        public SnakeNode() {}
        public SnakeNode(bool isHead, int positionX, int positionY)
        {
            IsHead = isHead;
            PositionX = positionX;
            PositionY = positionY;
        }
        [Key(0)]
        public bool IsHead { get; set; }
        private int positionX;
        [Key(1)]
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
        [Key(2)]
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
        Up = 0, Down = 1, Left = 2, Right = 3
    }
}
