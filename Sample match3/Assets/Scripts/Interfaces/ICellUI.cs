using UnityEngine;
using MatchThree.Objects;

namespace MatchThree.Interfaces
{
    public interface ICellUI
    {
        RectTransform GetRectTransform { get; }

        void ClearCell();
        void ConfigCell(ICell<ItemDescription> cell);
        void SetItemVisibleStatus(bool state);
        void SetItemToCell(IItem<ItemDescription> item);
    }
}