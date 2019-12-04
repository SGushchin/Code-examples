using MatchThree.Interfaces;

namespace MatchThree.Objects
{
    public class Item<T> : IItem<T> where T : class
    {
        private const int EMPTY = 0;

        #region Properties

        public T Description { get; private set; }

        public int ID { get; private set; }

        public int MatchID { get; set; }

        public bool IsEmpty { get; private set; } = true;

        #endregion

        #region PublicMethods

        /// <summary>
        /// Поместить опсание предмета в ячейку
        /// </summary>
        /// <param name="id"> ID описания предмета (необходимо для сохранения) </param>
        /// <param name="description"> Описание предмета </param>
        public void Set(int id, T description)
        {
            if (id < 0 || description == null) return;

            ID = id;
            Description = description;
            IsEmpty = false;
            MatchID = EMPTY;
        }

        /// <summary>
        /// Очистить описание
        /// </summary>
        public void Clear()
        {
            ID = EMPTY;
            Description = null;
            IsEmpty = true;
            MatchID = EMPTY;
        }

        #endregion

        #region InterfaceImplementation

        public object Clone()
        {
            var newItem = new Item<T>();
            newItem.Set(ID, Description);

            return newItem;
        }

        #endregion
    }
}
