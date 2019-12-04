using System;
using MatchThree.Interfaces;
using MatchThree.Components;
using MatchThree.Objects;

namespace MatchThree.Controllers
{
    public class DrawController : BaseComponent, IDrawController
    {
        public event Action OnAnimationsComplete;

        #region ExternalComponents

        private IGameAnimations _anim;

        private IFieldCreator _fieldCreator;

        #endregion

        #region PrivateVariables

        private ICellUI[,] _gameFieldUI;

        private bool _isInitialized = false;

        private bool _animIsNull = true;

        #endregion

        #region StandartMethodEvents

        private void Awake()
        {
            _anim = GetComponent<IGameAnimations>();

            if (_anim != null) _animIsNull = false;

            _fieldCreator = GetComponentInChildren<IFieldCreator>();

            if (_fieldCreator == null) throw new System.NullReferenceException("[DrawController] FieldCreator not found");

            ServiceContainer.RegisterService<IDrawController>(this, true);
        }

        private void OnEnable()
        {
            if (!_animIsNull) _anim.OnCompleteAllAnimations += AnimationsComplete;
        }
        
        private void OnDisable()
        {
            if (!_animIsNull) _anim.OnCompleteAllAnimations -= AnimationsComplete;
        }

        #endregion

        #region PublicMethods

        /// <summary>
        /// Инициализатор контроллера отрисовки игрового поля
        /// </summary>
        /// <param name="gameField">Игровое поле</param>
        public void Initialization(IField<ItemDescription> gameField)
        {
            if (gameField == null) return;

            if (!_animIsNull) _anim.Initialization(gameField.Columns * gameField.Rows);

            _gameFieldUI = _fieldCreator.CreateField(gameField);

            if (_gameFieldUI != null) _isInitialized = true;
        }

        /// <summary>
        /// Перемещение предмета из одной ячейки в другую
        /// </summary>
        /// <param name="from">Ячейка, откуда перемещается предмет</param>
        /// <param name="to">Ячейка, куда перемещается предмет</param>
        /// <param name="withAnimation">если true - проигрывается анимация</param>
        public void MoveItemFromTo (ICell<ItemDescription> from, ICell<ItemDescription> to, bool withAnimation)
        {
            if (!_isInitialized) return;

            var fromCell = _gameFieldUI[from.RowPosition, from.ColumnPosition];
            var toCell = _gameFieldUI[to.RowPosition, to.ColumnPosition];

            if (!withAnimation || _animIsNull)
            {
                fromCell.ClearCell();
                toCell.SetItemToCell(to.Item);
            }
            else
            {
                _anim.MoveItem(to, fromCell, toCell);
            }
        }

        /// <summary>
        /// Обмен позициями предметов
        /// </summary>
        /// <param name="first">Ячейка, куда был помещен предмет из второй</param>
        /// <param name="second">Ячейка, куда был помещен предмет из первой</param>
        /// <param name="withAnimation">если true - проигрывается анимация</param>
        public void Shuffle (ICell<ItemDescription> first, ICell<ItemDescription> second, bool withAnimation)
        {
            if (!_isInitialized) return;

            var firstCell = _gameFieldUI[first.RowPosition, first.ColumnPosition];
            var secondCell = _gameFieldUI[second.RowPosition, second.ColumnPosition];

            if (!withAnimation || _animIsNull)
            {
                firstCell.SetItemToCell(first.Item);
                secondCell.SetItemToCell(second.Item);
            }
            else
            {
                _anim.Shuffle(first, second, firstCell, secondCell);
            }
        }

        /// <summary>
        /// Очистка ячейки от описания предмета
        /// </summary>
        /// <param name="cell">Ячейки, в которой очищается предмет</param>
        /// <param name="withAnimation">если true - проигрывается анимация</param>
        public void ClearCell (ICell<ItemDescription> cell, bool withAnimation)
        {
            if (!_isInitialized) return;

            if (!withAnimation || _animIsNull)
                _gameFieldUI[cell.RowPosition, cell.ColumnPosition].ClearCell();
            else
            {
                _anim.ClearMatch(cell, _gameFieldUI[cell.RowPosition, cell.ColumnPosition]);
            }
        }

        #endregion

        #region PrivateMethods

        private void AnimationsComplete()
        {
            if (OnAnimationsComplete == null) return;

            OnAnimationsComplete.Invoke();
        }

        #endregion
    }
}
