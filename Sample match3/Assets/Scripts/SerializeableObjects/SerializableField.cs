using System;
using System.Collections.Generic;

namespace MatchThree.Objects
{
    [Serializable]
    public class SerializableField
    {
        public int Rows;

        public int Columns;

        public List<SerializableCell> Cells;

        public SerializableField()
        {

        }
    }
}
