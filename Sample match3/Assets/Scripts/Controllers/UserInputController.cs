using System;
using MatchThree.Interfaces;
using MatchThree.Components;
using MatchThree.UI;

namespace MatchThree.Controllers
{
    public sealed class UserInputController : BaseComponent, IUserInputController
    {
        #region Events

        public event Action OnLoadClickEvent;

        public event Action OnRestartClickEvent;

        public event Action<int, int, int, int> OnMouseClickEvent;

        #endregion

        #region ExternalComponents

        private IButtonHandler[] _buttonHandlers;

        private IMouseInputHandler _mouseInputHandler;

        #endregion

        #region PrivateVariables

        private bool _mouseInputHandlerIsNull = true;

        #endregion

        #region StandartMethodEvents

        private void Awake()
        {
            _buttonHandlers = GetComponentsInChildren<IButtonHandler>();

            if (_buttonHandlers.Length == 0) throw new Exception("[UserInputController] Button handlers not found");

            _mouseInputHandler = GetComponentInChildren<IMouseInputHandler>();

            if (_mouseInputHandler == null) throw new NullReferenceException("[UserInputController] Mouse input handler not found");

            ServiceContainer.RegisterService<IUserInputController>(this);
        }

        private void OnEnable()
        {
            foreach (var button in _buttonHandlers)
            {
                button.OnClickEvent += ButtonInvokeHandler;
            }

            _mouseInputHandler.OnMoveItems += MouseInvokeHandler;
        }
        
        private void OnDisable()
        {
            foreach (var button in _buttonHandlers)
            {
                button.OnClickEvent -= ButtonInvokeHandler;
            }

            _mouseInputHandler.OnMoveItems -= MouseInvokeHandler;
        }

        #endregion

        #region PrivateMethods

        private void ButtonInvokeHandler(ButtonTypes type)
        {
            switch (type)
            {
                case ButtonTypes.Load:
                    if (OnLoadClickEvent != null) OnLoadClickEvent.Invoke();
                    break;

                case ButtonTypes.Restart:
                    if (OnRestartClickEvent != null) OnRestartClickEvent.Invoke();
                    break;

                default:
                    break;
            }
        }
        
        private void MouseInvokeHandler(ISelectable first, ISelectable second)
        {
            if (OnMouseClickEvent == null) return;

            OnMouseClickEvent.Invoke(first.Row, first.Column, second.Row, second.Column);
        }

        #endregion
    }
}
