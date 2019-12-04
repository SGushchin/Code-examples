using System;
using System.Collections.Generic;
using System.Linq;
using MatchThree.Interfaces;

namespace MatchThree.Database
{
    public sealed class DataContainer<T> : IDatabase<T> where T : class
    {
        #region PrivateVariables

        private List<T> _objs;

        #endregion

        #region Properties

        public int Size { get => _objs.Count; }

        #endregion

        #region Constructors

        public DataContainer()
        {
            _objs = new List<T>();
        }

        public DataContainer(int size)
        {
            _objs = new List<T>(size);
        }

        public DataContainer(List<T> items)
        {
            _objs = items;
        }

        public DataContainer(T[] items)
        {
            _objs = items.ToList();
        }

        #endregion

        #region InterfaceImplementation

        /// <summary>
        /// Добавить объект в базу данный.
        /// </summary>
        /// <param name="obj"> Объект </param>
        /// <returns> true - если добавление прошло успешно. </returns>
        public bool Add(T obj)
        {
            if (obj == null) return false;

            _objs.Add(obj);

            return true;
        }

        /// <summary>
        /// Добавить набор объектов в базу данных.
        /// </summary>
        /// <param name="objs"> Набор объектов </param>
        /// <returns> true - если добавление прошло успешно. </returns>
        public bool AddRange(IEnumerable<T> objs)
        {
            if (objs == null) return false;

            _objs.AddRange(objs);

            return true;
        }

        /// <summary>
        /// Получить объект.
        /// </summary>
        /// <param name="match"> Параметр поиска </param>
        /// <param name="obj"> Найденный объект </param>
        /// <returns> true - если объект найден </returns>
        public bool Get(Predicate<T> match, out T obj)
        {
            var index = _objs.FindIndex(match);

            if (index == -1)
            {
                obj = null;
                return false;
            }

            obj = _objs[index];

            return true;
        }
        
        /// <summary>
        /// Получить набор объектов, соответствующий параметру поиска.
        /// </summary>
        /// <param name="match"> Параметр поиска </param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(Predicate<T> match) => _objs.FindAll(match);

        /// <summary>
        /// Получить все объекты из базы данных
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll() =>_objs;
        
        /// <summary>
        /// Удалить объект
        /// </summary>
        /// <param name="obj"> Объект для удаления </param>
        /// <returns> екгу - если удалени прошло успешно </returns>
        public bool Remove(T obj)
        {
            if (obj == null) return false;

            var index = _objs.FindIndex(findedItem => findedItem == obj);

            if (index == -1) return false;

            return _objs.Remove(_objs[index]);
        }

        /// <summary>
        /// Очистить базу данных
        /// </summary>
        public void RemoveAll() => _objs.Clear();
        
        /// <summary>
        /// Перезаписать объект в базе данных
        /// </summary>
        /// <param name="obj"> Перезаписываемый объект </param>
        /// <returns> true - если перезапись прошла успешно </returns>
        public bool Update(T obj)
        {
            if (obj == null) return false;

            var index = _objs.FindIndex(findedItem => findedItem == obj);

            if (index == -1) return false;

            _objs[index] = obj;

            return true;
        }

        /// <summary>
        /// Получить данные по индексу в БД
        /// </summary>
        /// <param name="id"> Индекс </param>
        /// <param name="obj"> Объект </param>
        /// <returns></returns>
        public bool GetByIndex(int id, out T obj)
        {
            if (id < 0 && id >= Size)
            {
                obj = null;
                return false;
            }

            obj = _objs[id];

            return true;
        }

        #endregion
    }
}