using ShisenSho.Common;
using System.Collections.ObjectModel;

namespace ShisenSho.ViewModels
{
    public partial class GameBoardViewModel : BaseViewModel
    {
        public GameBoardViewModel(Coordinate dimensions, int tileCount, MainViewModel parent)
        {
            _dimensions = dimensions;
            _board = new ObservableCollection<ObservableCollection<GamePieceViewModel>>();
            _tileCount = tileCount;
            _parent = parent;
            _currentEmojis = new List<string>();
            SelectEmojis();
            InitBoard();
        }

        private readonly List<string> _currentEmojis;
        public List<string> CurrentEmojis
        {
            get { return _currentEmojis; }
        }

        private MainViewModel _parent;

        private readonly Coordinate _dimensions;
        public Coordinate Dimensions
        {
            get { return _dimensions; }
        }

        private readonly ObservableCollection<ObservableCollection<GamePieceViewModel>> _board;
        public ObservableCollection<ObservableCollection<GamePieceViewModel>> Board
        {
            get { return _board; }
        }

        private readonly int _tileCount;
        public int TileCount
        {
            get { return _tileCount; }
        }

        public GamePieceViewModel GetGamePiece(Coordinate coordinate)
        {
            return Board[coordinate.x][coordinate.y];
        }

        private void SelectEmojis()
        {
            int count = Emojis.AvailableEmojis.Count();

            if (TileCount > count)
            {
                throw new Exception("Tile count too high not enough emojis");
            }
            else if (TileCount < count / 2)
            {
                for (int i = 0; i < TileCount; i++)
                {
                    string emoji;
                    do
                    {
                        emoji = RandomInstance.RandomObject(Emojis.AvailableEmojis);
                    } while (CurrentEmojis.Contains(emoji));
                    CurrentEmojis.Add(emoji);
                }
            }
            else if (TileCount < count)
            {
                List<string> excludeList = new List<string>();
                for (int i = 0; i < count - TileCount; i++)
                {
                    string emoji;
                    do
                    {
                        emoji = RandomInstance.RandomObject(Emojis.AvailableEmojis);
                    } while (excludeList.Contains(emoji));
                    excludeList.Add(emoji);
                }
                foreach (string emoji in Emojis.AvailableEmojis)
                {
                    if (!excludeList.Contains(emoji))
                    {
                        CurrentEmojis.Add(emoji);
                    }
                }
            }
            else
            {
                foreach (string emoji in Emojis.AvailableEmojis)
                {
                    CurrentEmojis.Add(emoji);
                }
            }
        }

        private void InitBoard()
        {
            var openSpots = new List<Coordinate>();
            for (int x = 0; x < Dimensions.x; x++)
            {
                ObservableCollection<GamePieceViewModel> col = new ObservableCollection<GamePieceViewModel>();
                for (int y = 0; y < Dimensions.y; y++)
                {
                    Coordinate coordinate = new Coordinate(x, y);
                    GamePieceViewModel newPiece;
                    if (x == 0 || y == 0 || x == Dimensions.x - 1 || y == Dimensions.y - 1)
                    {
                        newPiece = new GamePieceViewModel(coordinate, TileValue.Border, this);
                    }
                    else if (x == 1 || y == 1 || x == Dimensions.x - 2 || y == Dimensions.y - 2)
                    {
                        newPiece = new GamePieceViewModel(coordinate, TileValue.ForceEmpty, this);
                    }
                    else
                    {
                        newPiece = new GamePieceViewModel(coordinate, TileValue.Uninitialized, this);
                        openSpots.Add(coordinate);
                    }
                    col.Add(newPiece);
                }
                Board.Add(col);
            }

            // Add a random wall to uneven boards
            if (openSpots.Count % 2 == 1)
            {
                Coordinate randCoord = RandomInstance.RandomObject(openSpots);
                GetGamePiece(randCoord).Value = TileValue.Wall;
                openSpots.Remove(randCoord);
            }

            // TODO: Actually add walls

            // TODO: Make this actually generate solvable puzzles
            while(openSpots.Count > 0)
            {
                int randTile = RandomInstance.RandomInt(TileValue.TileOffset, TileValue.TileOffset + TileCount - 1);
                Coordinate randCoord1 = RandomInstance.RandomObject(openSpots);
                openSpots.Remove(randCoord1);
                Coordinate randCoord2 = RandomInstance.RandomObject(openSpots);
                openSpots.Remove(randCoord2);

                // TODO: This made it NP time
                //if(!CheckMatch(rand1, rand2)
                //{
                //    continue;
                //}

                GetGamePiece(randCoord1).Value = randTile;
                GetGamePiece(randCoord2).Value = randTile;
            }
            OnPropertyChanged(nameof(Board));
        }

        public bool IsBoardEmpty()
        {
            for (int x = 2; x < Dimensions.x - 2; x++)
            {
                for (int y = 2; y < Dimensions.y - 2; y++)
                {
                    if (GetGamePiece(new Coordinate(x, y)).IsTile)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private Coordinate? _selected;
        public Coordinate? Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != null)
                {
                    GetGamePiece((Coordinate)_selected).IsSelected = false;
                }
                if (value != null)
                {
                    GetGamePiece((Coordinate)value).IsSelected = true;
                }
                _selected = value;
                OnPropertyChanged();
            }
        }

        public void HandleSelection(Coordinate coord)
        {
            if(Selected == null)
            {
                Selected = coord;
            }
            else
            {
                if (CheckMatch((Coordinate)Selected, coord))
                {
                    // TODO: Play pop animation here somehow
                    GetGamePiece(coord).Value = TileValue.Empty;
                    GetGamePiece((Coordinate)Selected).Value = TileValue.Empty;
                    Selected = null;
                    if(IsBoardEmpty())
                    {
                        _parent.Status = MainViewModel.GameStatus.Win;
                        _parent.CurrentScreen = MainViewModel.Screen.End;
                    }
                }
                else
                {
                    // TODO: Play shake animation here somehow
                    Selected = null;
                }
            }
        }

        public bool CheckMatch(Coordinate coord1, Coordinate coord2)
        {
            if (GetGamePiece(coord1).Value != GetGamePiece(coord2).Value)
            {
                return false;
            }
            else if (coord1 == coord2)
            {
                return false;
            }
            else if (CheckMatchHelper(coord1, coord2, 0, Direction.North, Direction.North))
            {
                return true;
            }
            else if (CheckMatchHelper(coord1, coord2, 0, Direction.East, Direction.East))
            {
                return true;
            }
            else if (CheckMatchHelper(coord1, coord2, 0, Direction.South, Direction.South))
            {
                return true;
            }
            else if (CheckMatchHelper(coord1, coord2, 0, Direction.West, Direction.West))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckMatchHelper(Coordinate coord1, Coordinate coord2, int turns, Direction prevDir, Direction currDir)
        {
            if (prevDir != currDir)
            {
                turns += 1;
            }
            if (turns > 2)
            {
                return false;
            }

            // Get new point in this direction
            Coordinate newCoord;
            if(currDir == Direction.North)
            {
                newCoord = new Coordinate(coord1.x, coord1.y - 1);
            }
            else if (currDir == Direction.East)
            {
                newCoord = new Coordinate(coord1.x + 1, coord1.y);
            }
            else if (currDir == Direction.South)
            {
                newCoord = new Coordinate(coord1.x, coord1.y + 1);
            }
            else if (currDir == Direction.West)
            {
                newCoord = new Coordinate(coord1.x - 1, coord1.y);
            }
            else
            {
                throw new Exception("Unknown error, 5th direction?");
            }

            if (newCoord == coord2)
            {
                return true;
            }

            bool found = false;
            int newValue = GetGamePiece(newCoord).Value;
            switch (newValue)
            {
                // For empty cells keep travelling
                case TileValue.Empty:
                case TileValue.ForceEmpty:
                    // Don't make 180degree turns
                    if (currDir != Direction.South)
                    {
                        found = found || CheckMatchHelper(newCoord, coord2, turns, currDir, Direction.North);
                    }
                    if (currDir != Direction.West)
                    {
                        found = found || CheckMatchHelper(newCoord, coord2, turns, currDir, Direction.East);
                    }
                    if(currDir != Direction.North)
                    {
                        found = found || CheckMatchHelper(newCoord, coord2, turns, currDir, Direction.South);
                    }
                    if(currDir != Direction.East)
                    {
                        found = found || CheckMatchHelper(newCoord, coord2, turns, currDir, Direction.West);
                    }
                    break;
                case TileValue.OutOfBounds:
                case TileValue.Wall:
                case TileValue.Border:
                default:
                    break;
            }

            return found;
        }
    }
}
