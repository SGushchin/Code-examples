using System;

namespace MatchThree.Interfaces
{
    public interface IUserInputController
    {
        event Action OnLoadClickEvent;
        event Action<int, int, int, int> OnMouseClickEvent;
        event Action OnRestartClickEvent;
    }
}