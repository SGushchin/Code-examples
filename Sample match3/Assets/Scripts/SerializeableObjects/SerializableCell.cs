using System;

namespace MatchThree.Objects
{
    [Serializable]
    public class SerializableCell
    {
        public SerializableItem Item;

        public bool IsActive;

        public bool IsEmitter;

        public int Row;

        public int Column;

        public SerializableCell()
        {

        }
    }
}
