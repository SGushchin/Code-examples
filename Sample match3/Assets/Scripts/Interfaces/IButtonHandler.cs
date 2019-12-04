using System;
using MatchThree.UI;

namespace MatchThree.Interfaces
{
    public interface IButtonHandler
    {
        event Action<ButtonTypes> OnClickEvent;
    }
}