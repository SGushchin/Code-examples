using UnityEngine.UI;
using MatchThree.Components;
using MatchThree.Objects;
using MatchThree.Interfaces;


namespace MatchThree.UI
{
    public sealed class ItemUI : BaseComponent, IItemUI
    {
        #region PrivateVariables
        
        private bool _iconIsNull = true;

        private Image _icon;

        public bool EnableStatus
        {
            get
            {
                if (_iconIsNull) return false;

                return _icon.enabled;
            }
            set
            {
                if (!_iconIsNull) _icon.enabled = value;
            }
        }

        #endregion

        #region StandartMethodEvents

        private void Awake()
        {
            _iconIsNull = !TryGetComponent(out _icon);
        }

        #endregion

        #region PublicMethods

        public void SetItem(ItemDescription item)
        {
            if (_iconIsNull) return;

            _icon.sprite = item.Icon;
        }

        public void Clear()
        {
            if (_iconIsNull) return;

            _icon.enabled = false;

            _icon.sprite = null;
        }

        #endregion
    }
}
