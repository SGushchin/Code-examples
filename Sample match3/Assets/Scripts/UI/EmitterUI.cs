using UnityEngine.UI;
using MatchThree.Interfaces;
using MatchThree.Components;

namespace MatchThree.UI
{
    public class EmitterUI : BaseComponent, IEmitterUI
    {
        #region PrivateVariables

        private Image _icon;
        private bool _iconIsNull = true;

        #endregion

        #region Properties

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

        #region StandartMathodEvents

        private void Awake()
        {
            _iconIsNull = !TryGetComponent(out _icon);
        }

        #endregion
    }
}
