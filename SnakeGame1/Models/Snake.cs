using SnakeGame1.ViewModels;

namespace SnakeGame1.Models
{
    internal class Snake
    {
        public Queue<CellVM> SnakeCells { get; } = new Queue<CellVM>();

        public List<List<CellVM>> Cells { get; }

        private CellVM _start;
        private Action _createFood;

        public Snake(CellVM start, List<List<CellVM>> cells, Action createFood)
        {
            _start = start;
            _start.CellType = CellType.Snake;
             Cells= cells;
            _createFood = createFood;
            SnakeCells.Enqueue(_start);
        }       

        public void Move(MoveSnakeDirection direction)
        {
            var leaderCell = SnakeCells.Last();

            var nextRow = leaderCell.Row;
            var nextColumn = leaderCell.Column;

            switch (direction)
            {
                case MoveSnakeDirection.Left:
                    nextColumn--;
                    break;
                case MoveSnakeDirection.Right:
                    nextColumn++;
                    break;
                case MoveSnakeDirection.Up:
                    nextRow--;
                    break;
                case MoveSnakeDirection.Down:
                    nextRow++;
                    break;
                default:
                    break;
            }
                
            var nextCell = Cells[nextRow][nextColumn];           

            switch (nextCell?.CellType)
            {
                    case CellType.None:
                        nextCell.CellType = CellType.Snake;
                        SnakeCells.Dequeue().CellType = CellType.None;
                        SnakeCells.Enqueue(nextCell);
                        break;
                    case CellType.Food:
                        nextCell.CellType = CellType.Snake;
                        SnakeCells.Enqueue(nextCell);
                        _createFood?.Invoke();
                        break;
                    default:
                    throw new Exception("The game is over");
                
            }           
        }

        public void Restart()
        {
            foreach (var snake in SnakeCells)
            {
                snake.CellType = CellType.None;
            }

            SnakeCells.Clear();
            _start.CellType = CellType.Snake;
            SnakeCells.Enqueue(_start);
        }
    }
}
