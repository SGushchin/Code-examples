using System.Collections.Generic;
using UnityEngine;

namespace MatchThree.Objects
{
    public class PoolObjects
    {
        #region PrivateVariables

        private Queue<GameObject> poolQueue = new Queue<GameObject>();

        private Transform _parent;

        private bool _parentIsNull = true;

        private GameObject _prefab;

        #endregion

        #region Constructors

        public PoolObjects(GameObject poolObject, int count)
        {
            _prefab = poolObject;

            poolQueue = new Queue<GameObject>(count);

            for (int i = 0; i < count; i++)
            {
                var temp = Object.Instantiate(poolObject);
                temp.SetActive(false);
                poolQueue.Enqueue(temp);
            }
        }

        public PoolObjects(GameObject poolObject, Transform parent, int count)
        {
            _prefab = poolObject;

            poolQueue = new Queue<GameObject>(count);

            if (parent != null)
            {
                _parent = parent;
                _parentIsNull = false;
            }

            GameObject temp;

            for (int i = 0; i < count; i++)
            {
                if (_parentIsNull)
                {
                    temp = Object.Instantiate(poolObject);
                }
                else
                {
                    temp = Object.Instantiate(poolObject, parent);
                }

                temp.SetActive(false);
                poolQueue.Enqueue(temp);
            }
        }

        #endregion

        #region PublicMethods

        /// <summary>
        /// Получить объект из пула
        /// </summary>
        /// <param name="activateOnReturn"></param>
        /// <returns></returns>
        public GameObject GetObject(bool activateOnReturn = false)
        {
            var obj = poolQueue.Count > 0 ? poolQueue.Dequeue() : Object.Instantiate(_prefab);

            if (activateOnReturn)
            {
                obj.SetActive(true);
            }

            return obj;
        }

        /// <summary>
        /// Вернуть объект в пул
        /// </summary>
        /// <param name="gameObject"></param>
        public void ReturnToPool(GameObject gameObject)
        {
            if (!_parentIsNull)
            {
                gameObject.transform.SetParent(_parent);
                gameObject.transform.SetAsLastSibling();
            }

            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }

            poolQueue.Enqueue(gameObject);
        }

        /// <summary>
        /// Получить размер пула
        /// </summary>
        /// <returns></returns>
        public int GetPoolSize()
        {
            return poolQueue.Count;
        }

        /// <summary>
        /// Очистить пул
        /// </summary>
        public void ClearPool()
        {
            while (poolQueue.Count != 0)
            {
                var obj = GetObject();
                Object.DestroyImmediate(obj);
            }

            poolQueue.Clear();
        }

        #endregion
    }
}
