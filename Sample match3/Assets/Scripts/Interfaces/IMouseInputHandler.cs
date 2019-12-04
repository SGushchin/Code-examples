using System;

namespace MatchThree.Interfaces
{
    public interface IMouseInputHandler
    {
        event Action<ISelectable, ISelectable> OnMoveItems;
    }
}