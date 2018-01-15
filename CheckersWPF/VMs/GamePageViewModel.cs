﻿using CheckersWPF.Command;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CheckersWPF.Facade;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CheckersWPF.Enums;
using System.Windows;
using System.Windows.Controls;
using CheckersWPF.Properties;

namespace CheckersWPF.VMs
{
    public class GamePageViewModel : NavigationViewModel, INotifyPropertyChanged
    {
        public GamePageViewModel()
        {
            Controller = GameController.FromVariant(Variant.AmericanCheckers);

            GameCancelled = false;
            BlackOpponent = Opponent.Human;
            WhiteOpponent = Opponent.Computer;
            Variant = Variant.AmericanCheckers;
            Level = 9;

            SetupOption = Setup.Default;

            PlayerTurn += HandlePlayerTurnAsync;
        }

        private static CancellationTokenSource _cancelComputerMoveTokenSource;
        private async void HandlePlayerTurnAsync(object sender, Player e)
        {
            if ((e == Player.Black && BlackOpponent == Opponent.Computer ||
                e == Player.White && WhiteOpponent == Opponent.Computer) &&
                e == Controller.CurrentPlayer &&
                Controller.GetWinningPlayer() == null)
            {
                _cancelComputerMoveTokenSource?.Dispose();
                _cancelComputerMoveTokenSource = new CancellationTokenSource();

                List<Coord> move;
                await Task.Run(async () =>
                {
                    move = Controller.GetMove(Level, _cancelComputerMoveTokenSource.Token).ToList();
                    if (move.Any())
                    {
                        await Application.Current.Dispatcher.InvokeAsync(() => MovePiece(move), System.Windows.Threading.DispatcherPriority.Render);
                    }
                });

                if (!_cancelComputerMoveTokenSource.IsCancellationRequested)
                {
                    OnPlayerTurn(OtherPlayer(e));
                }
            }
        }

        private static Player OtherPlayer(Player player) =>
            player == Player.Black ? Player.White : Player.Black;

        private GameController _controller;
        public GameController Controller
        {
            get { return _controller; }
            set
            {
                _controller = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(IsGameInProgress));
                OnPropertyChanged(nameof(UndoMoveCommand));
                UndoMoveCommand.RaiseCanExecuteChanged();

                WinningPlayer = value.GetWinningPlayer();
            }
        }

        private void PlayEffect()
        {
            var mediaElement = new MediaElement
            {
                Source = new Uri("pack://application:,,,/Assets/WoodTheme/CheckerClick.png"),
                LoadedBehavior = MediaState.Manual
            };
            mediaElement.Play();
        }

        private Task MovePiece(List<Coord> move)
        {
            if (!IsGameInProgress)
            {
                return Task.CompletedTask;
            }

            if (bool.Parse(Settings.Default.EnableSoundEffects))
            {
                PlayEffect();
            }
            
            Controller = Controller.WithBoard(LastMove()).Move(move);
            OnPropertyChanged(nameof(LastTurn));

            return Task.CompletedTask;
        }

        private bool IsFenLastMove(string fen)
        {
            if (!Controller.MoveHistory.Any())
            {
                return true;
            }

            if (Controller.CurrentPlayer == Player.Black)
            {
                var isContinuedMove = Controller.MoveHistory.Last().WhiteMove == null;
                return isContinuedMove
                    ? Controller.MoveHistory.Last().BlackMove.ResultingFen == fen
                    : Controller.MoveHistory.Last().WhiteMove.ResultingFen == fen;
            }
            else
            {
                var isContinuedMove = Controller.MoveHistory.Last().WhiteMove != null;
                return isContinuedMove
                    ? Controller.MoveHistory.Last().WhiteMove.ResultingFen == fen
                    : Controller.MoveHistory.Last().BlackMove.ResultingFen == fen;
            }
        }

        private Variant _variant;
        public Variant Variant
        {
            get { return _variant; }
            set
            {
                _variant = value;
                OnPropertyChanged();
            }
        }

        private int _level;
        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;
                OnPropertyChanged();
            }
        }

        private string LastMove()
        {
            var pdnMove = Controller.MoveHistory.LastOrDefault();
            if (pdnMove == null)
            {
                return Controller.InitialPosition;
            }

            return pdnMove.WhiteMove?.ResultingFen ?? pdnMove.BlackMove.ResultingFen;
        }

        public PdnTurn LastTurn => Controller.MoveHistory.LastOrDefault();

        public string Status
        {
            get
            {
                var winningPlayer = Controller.WithBoard(LastMove()).GetWinningPlayer();
                return winningPlayer.HasValue && winningPlayer.Value != Controller.CurrentPlayer
                       ? $"{winningPlayer.Value} Won!"
                       : $"{Controller.CurrentPlayer}'s turn";
            }
        }
        
        public override string NavigationElement
        {
            get { return "Game Page"; }
            set
            {
                if (value != "Game Page")
                {
                    OnNavigationRequest(value);
                }
            }
        }

        private bool _displaySettingsGrid;
        public bool DisplaySettingsGrid
        {
            get { return _displaySettingsGrid; }
            set
            {
                _displaySettingsGrid = value;
                OnPropertyChanged();
            }
        }

        private bool _displayCreateGameGrid;
        public bool DisplayCreateGameGrid
        {
            get { return _displayCreateGameGrid; }
            set
            {
                _displayCreateGameGrid = value;
                OnPropertyChanged();
            }
        }

        private Setup _setupOption;
        public Setup SetupOption
        {
            get { return _setupOption; }
            set
            {
                if (_setupOption != value)
                {
                    _setupOption = value;
                    OnPropertyChanged();
                }
            }
        }

        public Player BoardOrientation =>
            BlackOpponent == Opponent.Human ? Player.Black : Player.White;

        private Player? _winningPlayer;
        public Player? WinningPlayer
        {
            get { return _winningPlayer; }
            set
            {
                _winningPlayer = value;
                OnPropertyChanged();
            }
        }

        public List<Opponent> Opponents =>
            Enum.GetValues(typeof(Opponent)).Cast<Opponent>().ToList();

        public List<Variant> Variants =>
            Enum.GetValues(typeof(Variant)).Cast<Variant>().ToList();

        public List<Setup> SetupOptions =>
            Enum.GetValues(typeof(Setup)).Cast<Setup>().ToList();

        private string GetOpponentText(Opponent opponent) =>
            opponent == Opponent.Human
            ? opponent.ToString()
            : opponent.ToString() + " Level " + Level;

        private Opponent _whiteOpponent;
        public Opponent WhiteOpponent
        {
            get { return _whiteOpponent; }
            set
            {
                if (_whiteOpponent != value)
                {
                    _whiteOpponent = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AreBothOpponentsHuman));
                }
            }
        }

        public string WhiteOpponentText =>
            GetOpponentText(WhiteOpponent);

        private Opponent _blackOpponent;
        public Opponent BlackOpponent
        {
            get { return _blackOpponent; }
            set
            {
                if (_blackOpponent != value)
                {
                    _blackOpponent = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AreBothOpponentsHuman));
                }
            }
        }

        public string BlackOpponentText =>
            GetOpponentText(BlackOpponent);

        public bool AreBothOpponentsHuman =>
            WhiteOpponent == Opponent.Human && BlackOpponent == Opponent.Human;

        private bool _gameCancelled;
        private bool GameCancelled
        {
            get { return _gameCancelled; }
            set
            {
                _gameCancelled = value;
                _cancelComputerMoveTokenSource?.Cancel();
            }
        }

        public bool IsGameInProgress
        {
            get
            {
                var winningPlayer = Controller.WithBoard(LastMove()).GetWinningPlayer();
                return !GameCancelled && !(winningPlayer.HasValue && winningPlayer.Value != Controller.CurrentPlayer);
            }
        }
        
        public bool CanTakeback()
        {
            var isHumanPlaying = BlackOpponent == Opponent.Human || WhiteOpponent == Opponent.Human;

            bool hasMoveHistory;
            if (BlackOpponent == Opponent.Human && WhiteOpponent == Opponent.Human)
            {
                hasMoveHistory = Controller.MoveHistory.Any();
            }
            else
            {
                hasMoveHistory = Controller.MoveHistory.Count >= 2;
            }

            return isHumanPlaying && hasMoveHistory && !GameCancelled && Controller.GetWinningPlayer() == null;
        }

        private DelegateCommand _displaySettingsCommand;
        public DelegateCommand DisplaySettingsCommand
        {
            get
            {
                if (_displaySettingsCommand != null)
                {
                    return _displaySettingsCommand;
                }

                _displaySettingsCommand = new DelegateCommand(sender => DisplaySettingsGrid = true);
                return _displaySettingsCommand;
            }
        }

        private DelegateCommand _hideSettingsCommand;
        public DelegateCommand HideSettingsCommand
        {
            get
            {
                if (_hideSettingsCommand != null)
                {
                    return _hideSettingsCommand;
                }

                _hideSettingsCommand = new DelegateCommand(sender => DisplaySettingsGrid = false);
                return _hideSettingsCommand;
            }
        }

        private DelegateCommand _displayCreateGameCommand;
        public DelegateCommand DisplayCreateGameCommand
        {
            get
            {
                if (_displayCreateGameCommand != null)
                {
                    return _displayCreateGameCommand;
                }

                _displayCreateGameCommand = new DelegateCommand(sender => DisplayCreateGameGrid = true);
                return _displayCreateGameCommand;
            }
        }

        private DelegateCommand _hideCreateGameCommand;
        public DelegateCommand HideCreateGameCommand
        {
            get
            {
                if (_hideCreateGameCommand != null)
                {
                    return _hideCreateGameCommand;
                }

                _hideCreateGameCommand = new DelegateCommand(sender => DisplayCreateGameGrid = false);
                return _hideCreateGameCommand;
            }
        }

        private DelegateCommand _createGameCommand;
        public DelegateCommand CreateGameCommand
        {
            get
            {
                if (_createGameCommand != null)
                {
                    return _createGameCommand;
                }
                
                _createGameCommand = new DelegateCommand(param => CreateGame(SetupOption == Setup.FromPosition ? (string)param : string.Empty));
                return _createGameCommand;
            }
        }

        private DelegateCommand _undoMoveCommand;
        public DelegateCommand UndoMoveCommand
        {
            get
            {
                if (_undoMoveCommand != null)
                {
                    return _undoMoveCommand;
                }

                _undoMoveCommand = new DelegateCommand(param => TakebackMove(), param => CanTakeback());
                return _undoMoveCommand;
            }
        }

        private DelegateCommand _cancelGameCommand;
        public DelegateCommand CancelGameCommand
        {
            get
            {
                if (_cancelGameCommand != null)
                {
                    return _cancelGameCommand;
                }

                _cancelGameCommand = new DelegateCommand(sender =>
                {
                    GameCancelled = true;
                    WinningPlayer = Controller.CurrentPlayer == Player.Black ? Player.White : Player.Black;
                    OnPropertyChanged(nameof(IsGameInProgress));
                });
                return _cancelGameCommand;
            }
        }

        private DelegateCommand _moveHistoryCommand;
        public DelegateCommand MoveHistoryCommand
        {
            get
            {
                if (_moveHistoryCommand != null)
                {
                    return _moveHistoryCommand;
                }

                _moveHistoryCommand = new DelegateCommand(param => Controller = Controller.WithBoard((string)param));
                return _moveHistoryCommand;
            }
        }

        private DelegateCommand _moveCommand;
        public DelegateCommand MoveCommand
        {
            get
            {
                if (_moveCommand != null)
                {
                    return _moveCommand;
                }

                _moveCommand = new DelegateCommand(param =>
                {
                    if (Controller.CurrentPlayer == Player.Black && BlackOpponent == Opponent.Computer ||
                        Controller.CurrentPlayer == Player.White && WhiteOpponent == Opponent.Computer)
                    {
                        return;
                    }

                    if (!IsFenLastMove(Controller.Fen)) { return; }

                    var fromCoord = ((dynamic) param).fromCoord;
                    var toCoord = ((dynamic) param).toCoord;

                    if (fromCoord != null && Controller.IsValidMove(fromCoord, toCoord))
                    {
                        MovePiece(new List<Coord> { fromCoord, toCoord });

                        var piece = Controller.Board[toCoord];
                        OnPlayerTurn(OtherPlayer(piece.Player));
                    }
                },
                param => {
                    if (Controller.CurrentPlayer == Player.Black && BlackOpponent == Opponent.Computer ||
                        Controller.CurrentPlayer == Player.White && WhiteOpponent == Opponent.Computer)
                    {
                        return false;
                    }

                    if (!IsFenLastMove(Controller.Fen)) { return false; }

                    var fromCoord = ((dynamic) param).fromCoord;
                    var toCoord = ((dynamic) param).toCoord;

                    return fromCoord != null && Controller.IsValidMove(fromCoord, toCoord);
                });
                return _moveCommand;
            }
        }

        private DelegateCommand _copyFenCommand;
        public DelegateCommand CopyFenCommand
        {
            get
            {
                if (_copyFenCommand != null)
                {
                    return _copyFenCommand;
                }

                _copyFenCommand = new DelegateCommand(param =>
                {
                    var move = (PdnMove)param;
                    SetClipboardContent(move.ResultingFen);
                });
                return _copyFenCommand;
            }
        }

        private void CreateGame(string param)
        {
            DisplayCreateGameGrid = false;
            GameCancelled = false;

            Controller = string.IsNullOrEmpty(param) ? GameController.FromVariant(Variant) : GameController.FromPosition(Variant, param);

            OnPropertyChanged(nameof(BoardOrientation));
            OnPropertyChanged(nameof(BlackOpponentText));
            OnPropertyChanged(nameof(WhiteOpponentText));
            OnPropertyChanged(nameof(IsGameInProgress));

            OnPlayerTurn(Controller.CurrentPlayer);
        }

        private void TakebackMove()
        {
            _cancelComputerMoveTokenSource.Cancel();
            Controller = Controller.TakebackMove();

            if (Controller.CurrentPlayer == Player.Black && BlackOpponent == Opponent.Computer ||
                Controller.CurrentPlayer == Player.White && WhiteOpponent == Opponent.Computer)
            {
                Controller = Controller.TakebackMove();
            }

            OnMoveUndone();
            OnPlayerTurn(Controller.CurrentPlayer);
        }

        private void SetClipboardContent(string content)
        {
            Clipboard.SetText(content);
        }

        public event EventHandler<Player> PlayerTurn;
        protected virtual void OnPlayerTurn(Player e) =>
            PlayerTurn?.Invoke(this, e);

        public event EventHandler MoveUndone;
        protected virtual void OnMoveUndone() =>
            MoveUndone?.Invoke(this, EventArgs.Empty);

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}