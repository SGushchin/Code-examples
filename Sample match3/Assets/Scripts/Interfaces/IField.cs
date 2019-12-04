using System;
using System.Collections.Generic;

namespace MatchThree.Interfaces
{
    public interface IField<T> : ICloneable
    {
        int Columns { get; }
        int Rows { get; }

        IEnumerable<ICell<T>> Get(Predicate<ICell<T>> match);
        IEnumerable<ICell<T>> GetAll();
        bool Set(int rows, int columns, List<ICell<T>> field);
    }
}