using MatchThree.Objects;

namespace MatchThree.Interfaces
{
    public interface IGameAnimations
    {
        event System.Action OnCompleteAllAnimations;

        void ClearMatch(ICell<ItemDescription> cell, ICellUI present);
        void Initialization(int fieldSize);
        void MoveItem(ICell<ItemDescription> destinationCell, ICellUI presentFrom, ICellUI presentTo);
        void Shuffle(ICell<ItemDescription> first, ICell<ItemDescription> second, ICellUI presentFirst, ICellUI presentSecond);
    }
}