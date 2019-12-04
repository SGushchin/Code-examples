using System;
using System.Collections.Generic;
using MatchThree.Interfaces;

namespace MatchThree.Objects
{
    public class Field<T> : IField<T> where T : class
    {
        #region PrivateVariables

        private List<ICell<T>> _field;

        #endregion

        #region Properties

        public int Rows { get; private set; }

        public int Columns { get; private set; }

        #endregion

        #region Constructor

        public Field(int rows, int columns, List<ICell<T>> field)
        {
            Rows = rows;
            Columns = columns;
            _field = field;
        }

        #endregion

        #region PublicMethods

        /// <summary>
        /// Получить все поля соответствующие условию
        /// </summary>
        /// <param name="match"> Условие </param>
        /// <returns></returns>
        public IEnumerable<ICell<T>> Get(Predicate<ICell<T>> match) => _field.FindAll(match);

        /// <summary>
        /// Получить все поля
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ICell<T>> GetAll() => _field;

        /// <summary>
        /// Установить поле
        /// </summary>
        /// <param name="rows"> Количество строк </param>
        /// <param name="columns">  Количество столбцов </param>
        /// <param name="field"> Список ячеек поля </param>
        /// <returns> true - если поле задано успешно </returns>
        public bool Set(int rows, int columns, List<ICell<T>> field)
        {
            if (rows < 1 || columns < 1 || field == null) return false;

            Rows = rows;
            Columns = columns;
            _field = field;

            return true;
        }

        #endregion

        #region InterfaceImplementation

        public object Clone()
        {
            var cells = new List<ICell<T>>(Rows * Columns);

            foreach (var cell in _field)
            {
                var newCell = (ICell<T>)cell.Clone();
                cells.Add(newCell);
            }

            return new Field<T>(Rows, Columns, cells);
        }

        #endregion
    }
}