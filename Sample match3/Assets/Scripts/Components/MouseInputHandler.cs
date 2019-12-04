using UnityEngine.EventSystems;
using MatchThree.Interfaces;

namespace MatchThree.Components
{
    public class MouseInputHandler : BaseComponent, IMouseInputHandler, IPointerDownHandler, IPointerUpHandler
    {
        public event System.Action<ISelectable, ISelectable> OnMoveItems;

        #region PrivateVariables

        private ISelectable _firstSelectedCell;

        private ISelectable _secondSelectedCell;

        private bool OnMoveItemsInvoke = false;

        #endregion

        #region IntetrfaceImplementation

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.pointerPressRaycast.gameObject.TryGetComponent<ISelectable>(out var cell))
            {
                if (_firstSelectedCell == null)
                {
                    cell.Select();
                    _firstSelectedCell = cell;

                    return;
                }

                if (cell.Equals(_firstSelectedCell))
                {
                    _firstSelectedCell.Deselect();
                    _firstSelectedCell = null;
                }
                else
                {
                    cell.Select();
                    _secondSelectedCell = cell;
                    OnMoveItemsInvoke = true;
                }
            }
            
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (OnMoveItemsInvoke)
            {
                if (OnMoveItems != null) OnMoveItems.Invoke(_firstSelectedCell, _secondSelectedCell);
                
                OnMoveItemsInvoke = false;

                _firstSelectedCell.Deselect();
                _secondSelectedCell.Deselect();

                _firstSelectedCell = null;
                _secondSelectedCell = null;

                return;
            }

            if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent<ISelectable>(out var cell))
            {
                if (_firstSelectedCell != null && !cell.Equals(_firstSelectedCell))
                {
                    if (OnMoveItems != null) OnMoveItems.Invoke(_firstSelectedCell, cell);
                    
                    _firstSelectedCell.Deselect();

                    _firstSelectedCell = null;
                }
                    
            }
        }

        #endregion
    }
}
