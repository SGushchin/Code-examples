using MatchThree.Objects;

namespace MatchThree.Interfaces
{
    public interface IFieldCreator
    {
        ICellUI[,] CreateField(IField<ItemDescription> gameField);
    }
}