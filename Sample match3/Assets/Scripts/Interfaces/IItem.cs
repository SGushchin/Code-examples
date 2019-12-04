using System;

namespace MatchThree.Interfaces
{
    public interface IItem<T> : ICloneable
    {
        T Description { get; }
        int ID { get; }
        bool IsEmpty { get; }
        int MatchID { get; set; }

        void Set(int id, T description);
        void Clear();
    }
}