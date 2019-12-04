using System;
using System.Linq;
using MatchThree.Interfaces;
using MatchThree.Objects;
using MatchThree.Components;

namespace MatchThree.Controllers
{
    public class GameController : BaseComponent
    {
        #region Enum

        private enum GameState
        {
            Empty = 0,
            WaitForInput,
            ShuffleCheck,
            ShuffleBack,
            ClearMatches,
            VerticalMove,
            DiagonalMove
        }

        #endregion

        #region Controllers

        private IUserInputController _input;

        private IDrawController _draw;

        #endregion

        #region PrivateVariables

        private IField<ItemDescription> _restartCopyField;

        private IField<ItemDescription> _gameField;

        private IMatchHandler _matchHandler;

        private IMotionHandler _motionHandler;

        private GameState _curentState; 
        
        private bool _fieldIsLoaded = false;

        private bool _isReadyToInput = false;

        private int _firstCellRow = -1;
        private int _firstCellColumn = -1;
        private int _secondCellRow = -1;
        private int _secondCellColumn = -1;

        #endregion

        #region StandartMethodEvents

        private void Awake()
        {
            ServiceContainer.RegisterService(this, true);

            _matchHandler = GetComponentInChildren<IMatchHandler>();

            if (_matchHandler == null)
                throw new NullReferenceException("[GameController] MatchHandler not found");

            _motionHandler = GetComponentInChildren<IMotionHandler>();

            if (_motionHandler == null)
                throw new NullReferenceException("[GameController] MotionHandler not found");
        }

        
        private void Start()
        {
            if (ServiceContainer.GetService(out _input))
            {
                _input.OnLoadClickEvent += LoadButtonHandler;
                _input.OnRestartClickEvent += RestartButtonHandler;
                _input.OnMouseClickEvent += MouseClickHandler;
            }
            else
            {
                throw new NullReferenceException("[GameController] InputController not found");
            }

            if (ServiceContainer.GetService(out _draw))
            {
                _draw.OnAnimationsComplete += StateHandler;
            }
            else
            {
                throw new NullReferenceException("[GameController] DrawController not found");
            }
        }

        private void OnDestroy()
        {
            _input.OnLoadClickEvent -= LoadButtonHandler;
            _input.OnRestartClickEvent -= RestartButtonHandler;
            _input.OnMouseClickEvent -= MouseClickHandler;

            _draw.OnAnimationsComplete -= StateHandler;
        }

        #endregion

        #region PrivateMethodsHandlers

        private void LoadButtonHandler()
        {
            if (ServiceContainer.GetService<IDataController<SerializableField>>(out var loader))
            {
                if (ServiceContainer.GetService<IDatabase<ItemDescription>>(out var database))
                {
                    if (loader.Load(out var data))
                    {
                        _restartCopyField = data.ToField(database);
                        _gameField = (IField<ItemDescription>)_restartCopyField.Clone();
                        
                        _fieldIsLoaded = true;

                        RandomFieldFilling(database);

                        _draw.Initialization(_gameField);
                        
                        _isReadyToInput = true;
                    }
                }
            }
        }

        private void RestartButtonHandler()
        {
            if (!_fieldIsLoaded) return;

            var restartCells = _restartCopyField.GetAll().ToList();
            var curentCells = _gameField.GetAll().ToList();

            for (int i = 0; i < restartCells.Count; i++)
            {
                if (restartCells[i].IsActive && !restartCells[i].Item.IsEmpty)
                {
                    curentCells[i].Item.Set(restartCells[i].Item.ID, restartCells[i].Item.Description);
                }
                else
                {
                    curentCells[i].Item.Clear();
                }
            }

            if (ServiceContainer.GetService<IDatabase<ItemDescription>>(out var database))
            {
                RandomFieldFilling(database);
            }

            _draw.Initialization(_gameField);
        }

        private void MouseClickHandler(int firstRowPos, int firstColumnPos, int secondRowPos, int secondColumnPos)
        {
            if (!_isReadyToInput) return;

            _isReadyToInput = false;

            _firstCellRow = firstRowPos;
            _firstCellColumn = firstColumnPos;
            _secondCellRow = secondRowPos;
            _secondCellColumn = secondColumnPos;

            _curentState = GameState.ShuffleCheck;
            StateHandler();
        }
        
        private void StateHandler()
        {
            switch (_curentState)
            {
                case GameState.WaitForInput:

                    _firstCellRow = -1;
                    _firstCellColumn = -1;
                    _secondCellRow = -1;
                    _secondCellColumn = -1;

                    _isReadyToInput = true;
                    _curentState = GameState.Empty;
                    break;

                case GameState.ShuffleCheck:
                    var status = _motionHandler.Shuffle(_gameField, _firstCellRow, _firstCellColumn, _secondCellRow, _secondCellColumn);

                    if (status)
                    {
                        var checkSecond = _matchHandler.CheckMatchWithCell(_secondCellRow, _secondCellColumn, _gameField);
                        var checkFirst = _matchHandler.CheckMatchWithCell(_firstCellRow, _firstCellColumn, _gameField);

                        if (!checkSecond && !checkFirst)
                            _curentState = GameState.ShuffleBack;
                        else
                            _curentState = GameState.ClearMatches;
                    }
                    else
                    {
                        _curentState = GameState.WaitForInput;
                        StateHandler();
                    }
                    break;
                    
                case GameState.ShuffleBack:
                    _motionHandler.Shuffle(_gameField, _firstCellRow, _firstCellColumn, _secondCellRow, _secondCellColumn);
                    _curentState = GameState.WaitForInput;
                    break;

                case GameState.ClearMatches:
                    if (_matchHandler.ClearAllMatches(_gameField))
                    {
                        _curentState = GameState.VerticalMove;
                    }
                    else
                    {
                        _curentState = GameState.WaitForInput;
                        StateHandler();
                    }
                    break;

                case GameState.VerticalMove:
                    _curentState = GameState.DiagonalMove;
                    if (!_motionHandler.VerticalMovement(_gameField))
                    {
                        StateHandler();
                    }
                    break;

                case GameState.DiagonalMove:
                    if (_motionHandler.DiagonalMovement(_gameField))
                    {
                        _curentState = GameState.VerticalMove;
                    }
                    else
                    {
                        _curentState = GameState.ClearMatches;
                        StateHandler();
                    }
                    break;

                default:

                    break;
            }
        }

        #endregion

        #region PrivateMethods

        private void RandomFieldFilling(IDatabase<ItemDescription> data)
        {
            var emitter = new Emitter<ItemDescription>(data);

            foreach(var cell in _gameField.GetAll())
            {
                if (cell.IsActive && cell.Item.IsEmpty)
                {
                    if (emitter.Generate(out var item))
                        cell.Item.Set(item.ID, item);
                }
            }
        }

        #endregion
    }
}