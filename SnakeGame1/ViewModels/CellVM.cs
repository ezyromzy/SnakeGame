using SnakeGame1.Models;
using Prism.Mvvm;

namespace SnakeGame1.ViewModels
{
    internal class CellVM : BindableBase
    {
        public int Row { get; }
        public int Column { get; }

        private CellType _celltype = CellType.None;

        public CellType CellType
        {
            get => _celltype;
            set
            {
                _celltype = value;
                RaisePropertyChanged(nameof(CellType));
            }
        }

        public CellVM(int row, int column, CellType cellType)
        {
            Row = row;
            Column = column; 
            CellType = cellType;
        }
    }
}
