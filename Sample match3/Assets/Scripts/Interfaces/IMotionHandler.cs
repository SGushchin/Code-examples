using MatchThree.Objects;

namespace MatchThree.Interfaces
{
    public interface IMotionHandler
    {
        bool DiagonalMovement(IField<ItemDescription> field);
        bool Shuffle(IField<ItemDescription> field, int firstRowPos, int firstColumnPos, int secondRowPos, int secondColumnPos);
        bool VerticalMovement(IField<ItemDescription> field);
    }
}