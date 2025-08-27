using ShisenSho.Common;
using System.Windows.Input;

namespace ShisenSho.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {

        public MainViewModel()
        {
            CurrentScreen = Screen.Title;
            _difficulty = Difficulty.None;
            Score = 0;
        }

        #region Properties
        public enum Difficulty
        {
            None,
            Easy,
            Medium,
            Hard
        }

        private Difficulty _difficulty;

        public enum Screen
        {
            Title,
            Selection,
            Game,
            End
        }

        private Screen _currentScreen;
        public Screen CurrentScreen
        {
            get { return _currentScreen; }
            set { 
                _currentScreen = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsTitleScreen));
                OnPropertyChanged(nameof(IsSelectionScreen));
                OnPropertyChanged(nameof(IsGameScreen));
                OnPropertyChanged(nameof(IsEndScreen));
            }
        }

        private GameBoardViewModel? _currentGame;
        public GameBoardViewModel? CurrentGame
        {
            get { return _currentGame; }
            set
            {
                _currentGame = value;
                OnPropertyChanged();
            }
        }

        public enum GameStatus
        {
            Active,
            Win,
            Loss
        }
        private GameStatus _status;
        public GameStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsWon));
                OnPropertyChanged(nameof(IsLost));
            }
        }

        private void NewGame()
        {
            Coordinate dimensions;
            int tileCount;
            switch(_difficulty)
            {
                case Difficulty.Easy:
                    dimensions = new Coordinate(10, 10);
                    tileCount = 9;
                    break;
                case Difficulty.Medium:
                    dimensions = new Coordinate(13, 13);
                    tileCount = 16;
                    break;
                case Difficulty.Hard:
                    dimensions = new Coordinate(18, 18);
                    tileCount = 20;
                    break;
                case Difficulty.None:
                default:
                    throw new Exception("Unknown difficulty setting");
            }

            Score = 0;
            Status = GameStatus.Active;
            CurrentGame = new GameBoardViewModel(dimensions, tileCount, this);
        }
        
        public int HighScore
        {
            get
            {
                // TODO: Add a permanent storage for high score
                return 0;
            }
        }
        private int _score;
        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Screens and Buttons
        // Title
        public bool IsTitleScreen
        {
            get
            {
                return _currentScreen == Screen.Title;
            }
        }
        private ICommand? _onPlayClicked;
        public ICommand OnPlayClicked
        {
            get
            {
                if( _onPlayClicked == null )
                {
                    _onPlayClicked = new Command(() =>
                    {
                        CurrentScreen = Screen.Selection;
                    });
                }
                return _onPlayClicked;
            }
        }
        // Selection
        public bool IsSelectionScreen
        {
            get
            {
                return _currentScreen == Screen.Selection;
            }
        }
        private ICommand? _onEasyClicked;
        public ICommand OnEasyClicked
        {
            get
            {
                if (_onEasyClicked == null)
                {
                    _onEasyClicked = new Command(() =>
                    {
                        _difficulty = Difficulty.Easy;
                        NewGame();
                        CurrentScreen = Screen.Game;
                    });
                }
                return _onEasyClicked;
            }
        }
        private ICommand? _onMediumClicked;
        public ICommand OnMediumClicked
        {
            get
            {
                if (_onMediumClicked == null)
                {
                    _onMediumClicked = new Command(() =>
                    {
                        _difficulty = Difficulty.Medium;
                        NewGame();
                        CurrentScreen = Screen.Game;
                    });
                }
                return _onMediumClicked;
            }
        }
        private ICommand? _onHardClicked;
        public ICommand OnHardClicked
        {
            get
            {
                if (_onHardClicked == null)
                {
                    _onHardClicked = new Command(() =>
                    {
                        _difficulty = Difficulty.Hard;
                        NewGame();
                        CurrentScreen = Screen.Game;
                    });
                }
                return _onHardClicked;
            }
        }
        // Game
        public bool IsGameScreen
        {
            get
            {
                return _currentScreen == Screen.Game;
            }
        }
        // End
        public bool IsEndScreen
        {
            get
            {
                return _currentScreen == Screen.End;
            }
        }
        public bool IsWon
        {
            get { return _status == GameStatus.Win; }
        }
        public bool IsLost
        {
            get { return _status == GameStatus.Loss; }
        }
        private ICommand? _onPlayAgainClicked;
        public ICommand OnPlayAgainClicked
        {
            get
            {
                if (_onPlayAgainClicked == null)
                {
                    _onPlayAgainClicked = new Command(() =>
                    {
                        NewGame();
                        CurrentScreen = Screen.Game;
                    });
                }
                return _onPlayAgainClicked;
            }
        }
        private ICommand? _onMainMenuClicked;
        public ICommand OnMainMenuClicked
        {
            get
            {
                if (_onMainMenuClicked == null)
                {
                    _onMainMenuClicked = new Command(() =>
                    {
                        CurrentScreen = Screen.Title;
                    });
                }
                return _onMainMenuClicked;
            }
        }
        #endregion
    }
}
