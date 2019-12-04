using UnityEngine.UI;
using MatchThree.Interfaces;

namespace MatchThree.Components
{
    public class OutlineComponent : BaseComponent, IOutlineComponent
    {
        #region PrivateVariables

        private Image _outlineImage;
        
        private bool _outlineImageIsNull = true;

        #endregion

        #region StandartMethodEvents

        public void Awake() => _outlineImageIsNull = !TryGetComponent(out _outlineImage);

        #endregion

        #region PublicMethods

        public void Enable()
        {
            if (_outlineImageIsNull) return;

            _outlineImage.enabled = true;
        }

        public void Disable()
        {
            if (_outlineImageIsNull) return;
            
            _outlineImage.enabled = false;
        }

        #endregion
    }
}
