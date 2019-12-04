using System;

namespace MatchThree.Interfaces
{
    public interface ICell<T> : ICloneable
    {
        bool IsActive { get; }
        bool IsEmitter { get; }
        IItem<T> Item { get; }
        int RowPosition { get; }
        int ColumnPosition { get; }
    }
}