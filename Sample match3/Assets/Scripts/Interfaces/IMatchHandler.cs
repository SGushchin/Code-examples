using MatchThree.Objects;

namespace MatchThree.Interfaces
{
    public interface IMatchHandler
    {
        bool CheckMatchWithCell(int rowPosition, int columnPosition, IField<ItemDescription> source);
        bool ClearAllMatches(IField<ItemDescription> source);
    }
}