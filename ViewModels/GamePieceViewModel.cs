using ShisenSho.Common;
using System.Windows.Input;

namespace ShisenSho.ViewModels
{
    public partial class GamePieceViewModel : BaseViewModel
    {
        public GamePieceViewModel(Coordinate coordinate, int value, GameBoardViewModel parent)
        {
            _coordinate = coordinate;
            _value = value;
            _parent = parent;
            _isSelected = false;
        }
        private readonly GameBoardViewModel _parent;
        private readonly Coordinate _coordinate;
        public Coordinate Coordinate
        {
            get { return _coordinate; }
        }
        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
                OnPropertyChanged(Emoji);
                OnPropertyChanged(nameof(IsTile));
                OnPropertyChanged(nameof(IsEmpty));
                OnPropertyChanged(nameof(IsWall));
            }
        }
        public string Emoji
        {
            get
            {
                int emojiValue = Value - TileValue.TileOffset;
                string returnStr;
                if (emojiValue < 0)
                {
                    returnStr = "";
                }
                else
                {
                    returnStr = _parent.CurrentEmojis[Value - TileValue.TileOffset];
                }
                return returnStr;
            }
        }
        private ICommand? _handleSelection;
        public ICommand HandleSelection
        {
            get
            {
                if (_handleSelection == null)
                {
                    _handleSelection = new Command(() =>
                    {
                        // Only do something if you touch a real piece (ignore wall / empty)
                        if(Value >= TileValue.TileOffset)
                        {
                            _parent.HandleSelection(Coordinate);
                        }
                    });
                }
                return _handleSelection;
            }
        }
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
        public bool IsTile
        {
            get
            {
                return Value >= TileValue.TileOffset;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return Value == TileValue.Empty || Value == TileValue.ForceEmpty;
            }
        }

        public bool IsWall
        {
            get
            {
                return Value == TileValue.Wall;
            }
        }


        private Direction? _pathInDirection;
        public int? PathInDirection
        {
            get
            {
                return (int?)_pathInDirection;
            }
            set
            {
                _pathInDirection = (Direction?)value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsPath));
                OnPropertyChanged(nameof(IsPathVertical));
                OnPropertyChanged(nameof(IsPathHorizontal));
                OnPropertyChanged(nameof(IsPathNorthEast));
                OnPropertyChanged(nameof(IsPathNorthWest));
                OnPropertyChanged(nameof(IsPathSouthEast));
                OnPropertyChanged(nameof(IsPathSouthWest));
            }
        }
        private Direction? _pathOutDirection;
        public int? PathOutDirection
        {
            get
            {
                return (int?)_pathOutDirection;
            }
            set
            {
                _pathOutDirection = (Direction?)value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsPath));
                OnPropertyChanged(nameof(IsPathVertical));
                OnPropertyChanged(nameof(IsPathHorizontal));
                OnPropertyChanged(nameof(IsPathNorthEast));
                OnPropertyChanged(nameof(IsPathNorthWest));
                OnPropertyChanged(nameof(IsPathSouthEast));
                OnPropertyChanged(nameof(IsPathSouthWest));
            }
        }

        public bool IsPath
        {
            get
            {
                return PathInDirection != null && PathOutDirection != null;
            }
        }

        public bool IsPathVertical
        {
            get
            {
                if(!IsPath)
                {
                    return false;
                }
                Direction indir = (Direction)PathInDirection;
                Direction outdir = (Direction)PathOutDirection;
                return (indir == outdir && (indir == Direction.North || indir == Direction.South));
            }
        }
        public bool IsPathHorizontal
        {
            get
            {
                if (!IsPath)
                {
                    return false;
                }
                Direction indir = (Direction)PathInDirection;
                Direction outdir = (Direction)PathOutDirection;
                return (indir == outdir && (indir == Direction.East || indir == Direction.West));
            }
        }
        public bool IsPathNorthEast
        {
            get
            {
                if (!IsPath)
                {
                    return false;
                }
                Direction indir = (Direction)PathInDirection;
                Direction outdir = (Direction)PathOutDirection;
                return ((indir == Direction.South && outdir == Direction.East) || (indir == Direction.West && outdir == Direction.North));
            }
        }
        public bool IsPathNorthWest
        {
            get
            {
                if (!IsPath)
                {
                    return false;
                }
                Direction indir = (Direction)PathInDirection;
                Direction outdir = (Direction)PathOutDirection;
                return ((indir == Direction.South && outdir == Direction.West) || (indir == Direction.East && outdir == Direction.North));
            }
        }
        public bool IsPathSouthEast
        {
            get
            {
                if (!IsPath)
                {
                    return false;
                }
                Direction indir = (Direction)PathInDirection;
                Direction outdir = (Direction)PathOutDirection;
                return ((indir == Direction.North && outdir == Direction.East) || (indir == Direction.West && outdir == Direction.South));
            }
        }
        public bool IsPathSouthWest
        {
            get
            {
                if (!IsPath)
                {
                    return false;
                }
                Direction indir = (Direction)PathInDirection;
                Direction outdir = (Direction)PathOutDirection;
                return ((indir == Direction.North && outdir == Direction.West) || (indir == Direction.East && outdir == Direction.South));
            }
        }
    }
}
