using MatchThree.Objects;

namespace MatchThree.Interfaces
{
    public interface IItemUI
    {
        bool EnableStatus { get; set; }

        void Clear();
        void SetItem(ItemDescription item);
    }
}