using System;
using UnityEngine;
using UnityEngine.UI;
using MatchThree.Components;
using MatchThree.Interfaces;

namespace MatchThree.UI
{
    public class ButtonHandler : BaseComponent, IButtonHandler
    {
        #region SerializeFields

        [SerializeField]
        private ButtonTypes _type = ButtonTypes.Empty;

        [Space(10)]
        [Header("Отладочные параметры")]
        [Tooltip("Для удобства отладки")]
        [SerializeField]
        private Button _clickButton;

        [Tooltip("Для удобства отладки")]
        [SerializeField]
        private bool _clickButtonIsNull = true;

        #endregion

        public event Action<ButtonTypes> OnClickEvent;

        #region StandartMethodEvents

        private void Awake()
        {
            _clickButtonIsNull = !TryGetComponent(out _clickButton);
        }

        private void OnEnable()
        {
            if (_clickButtonIsNull) return;
            
            _clickButton.onClick.AddListener(Click);
        }

        private void OnDisable()
        {
            if (_clickButtonIsNull) return;

            _clickButton.onClick.RemoveListener(Click);
        }

        #endregion

        #region PrivateMethods

        private void Click()
        {
            if (OnClickEvent == null) return;

            OnClickEvent.Invoke(_type);
        }

        #endregion
    }
}
