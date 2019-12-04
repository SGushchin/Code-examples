using UnityEngine;
using UnityEngine.UI;
using MatchThree.Interfaces;
using MatchThree.Components;
using MatchThree.Objects;

namespace MatchThree.UI
{
    public class CellUI : BaseComponent, ISelectable, ICellUI
    {
        #region PrivateVariables

        private Image _icon;

        private bool _iconIsNull = true;

        #endregion

        #region Propeties

        public int Row { get; private set; }

        public int Column { get; private set; }

        public RectTransform GetRectTransform { get; private set; }

        #endregion

        #region ExternalComponents

        private IEmitterUI _emitter;
        private bool _emitterIsNull = true;

        private IItemUI _itemUI;
        private bool _itemUIIsNull = true;

        private IOutlineComponent _outline;
        private bool _outlineIsNull = true;

        #endregion


        private void Awake()
        {
            _iconIsNull = !TryGetComponent(out _icon);

            _emitter = GetComponentInChildren<IEmitterUI>();
            if (_emitter != null) _emitterIsNull = false;
            
            _itemUI = GetComponentInChildren<IItemUI>();
            if (_itemUI != null) _itemUIIsNull = false;
            
            _outline = GetComponentInChildren<IOutlineComponent>(true);
            if (_outline != null) _outlineIsNull = false;

            GetRectTransform = GetComponent<RectTransform>();
        }

        #region PublicMethods

        /// <summary>
        /// Начальная конфигурация представления ячейки поля
        /// </summary>
        /// <param name="cell"> Ячейка, для которой конфигурируется представление </param>
        public void ConfigCell(ICell<ItemDescription> cell)
        {
            if (cell == null) throw new System.NullReferenceException("[Cell UI] config failed");

            Row = cell.RowPosition;
            Column = cell.ColumnPosition;

            if (!_iconIsNull) _icon.enabled = cell.IsActive;

            if (!_emitterIsNull) _emitter.EnableStatus = cell.IsEmitter;

            if (!_itemUIIsNull)
            {
                if (cell.IsActive) SetItemToCell(cell.Item);
                else ClearCell();
            }
        }

        /// <summary>
        /// Установить предмет в ячейку
        /// </summary>
        /// <param name="item"> Предмет, который нужно представить на поле </param>
        public void SetItemToCell(IItem<ItemDescription> item)
        {
            if (_itemUIIsNull) return;

            if (item.IsEmpty)
            {
                _itemUI.Clear();
                _itemUI.EnableStatus = false;
            }
            else
            {
                _itemUI.SetItem(item.Description);
                _itemUI.EnableStatus = true;
            }
        }

        /// <summary>
        /// Очистить ячейку
        /// </summary>
        public void ClearCell()
        {
            _itemUI.Clear();
        }

        /// <summary>
        /// Установить отображение предмета в ячейке
        /// </summary>
        /// <param name="state"> true - предмет отображается</param>
        public void SetItemVisibleStatus(bool state)
        {
            if (_itemUIIsNull) return;

            _itemUI.EnableStatus = state;
        }

        #endregion

        #region IntervaceInheritance

        /// <summary>
        /// Выделить ячейку
        /// </summary>
        public void Select()
        {
            if (_outlineIsNull) return;
            _outline.Enable();
        }

        /// <summary>
        /// Убрать выделение
        /// </summary>
        public void Deselect()
        {
            if (_outlineIsNull) return;
            _outline.Disable();
        }

        #endregion
    }
}
