using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using SnakeGame1.Models;

namespace SnakeGame1.ViewModels
{
    internal class MainWindowVM : BindableBase
    {
		private bool _continueGame = false;

		public bool ContinueGame
		{
			get => _continueGame;
			private set
			{
				_continueGame = value;
				RaisePropertyChanged(nameof(ContinueGame));

				if (_continueGame)
				{
					SnakeMovementAsync();
				}
			}
		}

		private double _cellId = 50;

		public double CellId
		{
			get => _cellId;
			set
			{
				_cellId = value;
				RaisePropertyChanged(nameof(CellId));
			}
		}

		public List<List<CellVM>> AllCells { get; } = new List<List<CellVM>>();

		public DelegateCommand StartStopCommand { get;  }
		
		private MainWindow _mainWindow;
		private Snake _snake;
		private MoveSnakeDirection _currentMoveDirection = MoveSnakeDirection.Right;
		private CellVM _lastFood;
		
		private const int _startSpeed = 100;
				
		private int _rowCount = 20;
		private int _columnCount = 20;		
		private int _speed = _startSpeed;

        public MainWindowVM(MainWindow mainWindow)
        {
			_mainWindow = mainWindow;
			_speed = _startSpeed;
			StartStopCommand = new DelegateCommand(() => ContinueGame = !ContinueGame);

			for(int row = 0;  row < _rowCount; row++)
			{
                var rowCells = new List<CellVM>();
                
				for (int column = 0; column < _columnCount; column++)
				{
					var newCell = new CellVM(row, column, Models.CellType.None);
					rowCells.Add(newCell);	
				}

				AllCells.Add(rowCells);
			}

			_snake = new Snake(AllCells[_rowCount / 2][_columnCount / 2], AllCells, CreateRandomFood);
			CreateRandomFood();
			_mainWindow.KeyDown += KeyClick;

			_mainWindow.Loaded += (s, e) => UpdateCell();
			_mainWindow.SizeChanged += (s, e) => UpdateCell();
		}

		private void UpdateCell()
		{
			if (_mainWindow.IsLoaded)
				CellId = (_mainWindow.Width - 150) / _columnCount;
		}

		private void KeyClick(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.A:
					if (_currentMoveDirection != MoveSnakeDirection.Right) 
					_currentMoveDirection = MoveSnakeDirection.Left;
					break;
				case Key.D:
                    if (_currentMoveDirection != MoveSnakeDirection.Left)
                        _currentMoveDirection = MoveSnakeDirection.Right;
					break;
				case Key.W:
                    if (_currentMoveDirection != MoveSnakeDirection.Down)
                        _currentMoveDirection = MoveSnakeDirection.Up;
					break;
				case Key.S:
                    if (_currentMoveDirection != MoveSnakeDirection.Up)
                        _currentMoveDirection = MoveSnakeDirection.Down;
					break;
				default:
					break;
			}
		}
		private async Task SnakeMovementAsync()
		{
			while (ContinueGame)
			{
				await Task.Delay(_speed);

				try
				{
					_snake.Move(_currentMoveDirection);
				}
				catch
				{
					ContinueGame = false;
					_lastFood.CellType = CellType.None;
					MessageBox.Show("The game is over");
					_snake.Restart();
					_speed = _startSpeed;					
					CreateRandomFood();
				}
			}
		}	

		private void CreateRandomFood()
		{
			var noneCells = AllCells
				.SelectMany(x => x.Where(c => c.CellType == CellType.None))
				.ToArray();

			var random = new Random();
			int randomIndex = random.Next(noneCells.Count());

			_lastFood = noneCells[randomIndex];

			_lastFood.CellType = CellType.Food;
			_speed = (int)(_speed * 0.95);
		}
	}
}
