using MatchThree.Interfaces;

namespace MatchThree.Objects
{
    public class Cell<T> : ICell<T> where T : class
    {
        #region Properties

        public bool IsActive { get; private set; }

        public bool IsEmitter { get; private set; }

        public int RowPosition { get; private set; }

        public int ColumnPosition { get; private set; }

        public IItem<T> Item { get; private set; }

        #endregion

        #region Constructor

        public Cell(int row, int column, IItem<T> item = null, bool isActive = false , bool isEmitter = false)
        {
            RowPosition = row;

            ColumnPosition = column;

            IsActive = isActive;

            IsEmitter = IsActive == true ? isEmitter : false;

            if (IsActive && item == null) throw new System.NullReferenceException("[Cell object] cel is active but item is null");

            Item = item;
        }

        #endregion

        #region InterfaceImplementation

        public object Clone()
        {
            return new Cell<T>(RowPosition, ColumnPosition, (IItem<T>)Item.Clone(), IsActive, IsEmitter);
        }

        #endregion
    }
}
