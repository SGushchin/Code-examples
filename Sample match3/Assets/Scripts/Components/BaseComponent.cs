using UnityEngine;

namespace MatchThree.Components
{
    public abstract class BaseComponent : MonoBehaviour
    {
        public bool IsActive { get; private set; }

        public virtual void On()
        {
            IsActive = true;
        }

        public virtual void Off()
        {
            IsActive = false;
        }

        public void Switch()
        {
            if (!IsActive)
            {
                On();
            }
            else
            {
                Off();
            }
        }
    }
}
