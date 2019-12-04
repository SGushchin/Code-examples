using System;
using System.Collections.Generic;

namespace MatchThree.Interfaces
{
    public interface IDatabase<T>
    {
        int Size { get; }

        bool Add(T item);
        bool AddRange(IEnumerable<T> items);
        bool Update(T item);
        bool Remove(T item);
        void RemoveAll();
        bool Get(Predicate<T> match, out T item);
        bool GetByIndex(int id, out T item);
        IEnumerable<T> GetAll(Predicate<T> match);
        IEnumerable<T> GetAll();
    }
}