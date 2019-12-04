using System;
using MatchThree.Objects;

namespace MatchThree.Interfaces
{
    public interface IDrawController
    {
        event Action OnAnimationsComplete;

        void ClearCell(ICell<ItemDescription> cell, bool withAnimation);
        void Initialization(IField<ItemDescription> gameField);
        void MoveItemFromTo(ICell<ItemDescription> from, ICell<ItemDescription> to, bool withAnimation);
        void Shuffle(ICell<ItemDescription> first, ICell<ItemDescription> second, bool withAnimation);
    }
}