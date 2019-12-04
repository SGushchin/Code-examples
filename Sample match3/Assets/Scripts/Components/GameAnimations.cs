using UnityEngine;
using System.Collections;
using MatchThree.Interfaces;
using MatchThree.Objects;

namespace MatchThree.Components
{
    public class GameAnimations : BaseComponent, IGameAnimations
    {
        public event System.Action OnCompleteAllAnimations;

        #region SerializeFields

        [SerializeField]
        private string _prefabName = "Item";

        [SerializeField]
        private GameObject _prefab;

        #endregion

        #region Constants
        private readonly Vector2 _anchor = new Vector2(0.5f, 0.5f);
        private readonly Vector3 _clearMatchScale = new Vector3(0f, 0f, 0f);
        private const float _clearMatchTime = 0.25f;
        private const float _moveItemTime = 0.2f;
        private const float _shuffleTime = 0.5f;
        #endregion

        #region PrivateVariables
        
        private PoolObjects _pool;

        private Canvas _canvas;

        private int _startedAnims = 0;

        #endregion

        #region StandartMethodsEvents
        private void Start()
        {
            if (ServiceContainer.GetService<IDatabase<GameObject>>(out var cashedPrefabs))
            {
                if (!cashedPrefabs.Get(prefab => prefab.name == _prefabName, out _prefab))
                    throw new System.ArgumentNullException("[GameAnimations] Prefab not found");
            }
        }

        private void OnDestroy()
        {
            _pool?.ClearPool();
        }
        #endregion

        #region PublicMethods
        /// <summary>
        /// Инициализация класса анимации действи над предметами
        /// </summary>
        /// <param name="fieldSize">Размер поля для создания пула под количество ячеек</param>
        public void Initialization(int fieldSize)
        {
            if (_pool == null)
            {
                var folder = new GameObject("[PoolFolder]").transform;
                _pool = new PoolObjects(_prefab, folder, fieldSize);
            }

            _canvas = GetComponentInParent<Canvas>();
            if (_canvas == null) throw new System.NullReferenceException("[GameAnimations] Canvas not found");
        }

        /// <summary>
        /// Перемещение представления предмета в заданные координаты
        /// </summary>
        /// <param name="destinationCell">Ячейка с данными о предмете</param>
        /// <param name="presentFrom">Визуальная ячейка предмета на поле, откуда перемещается предмет</param>
        /// <param name="presentTo">Визуальная ячейка предмета на поле, кула перемещается предмет</param>
        public void MoveItem(ICell<ItemDescription> destinationCell, ICellUI presentFrom, ICellUI presentTo)
        {
            var obj = CreateAnimObject(presentFrom);

            var item = obj.GetComponent<IItemUI>();

            item.SetItem(destinationCell.Item.Description);
            item.EnableStatus = true;

            obj.SetActive(true);
            
            presentFrom.ClearCell();
            presentTo.SetItemToCell(destinationCell.Item);

            presentFrom.SetItemVisibleStatus(false);
            presentTo.SetItemVisibleStatus(false);

            var positionTo = GetPosition(presentTo);

            var result = new Hashtable(2)
            {
                { "GameObject", obj },
                { "IItemUI", item },
                { "ICellUITo", presentTo }
            };

            _startedAnims++;
            
            iTween.MoveTo(obj, iTween.Hash("position", transform.InverseTransformPoint(positionTo),
                                           "time", _moveItemTime,
                                           "islocal", true,
                                           "easetype", iTween.EaseType.easeInSine,
                                           "looptype", iTween.LoopType.none,
                                           "oncomplete", nameof(CompleteMoveItem),
                                           "oncompletetarget", gameObject,
                                           "oncompleteparams", result));
        }

        /// <summary>
        /// Обмен позициями фишек
        /// </summary>
        /// <param name="first">Предмет, помещенный в парвую ячейку</param>
        /// <param name="second">Предмет, помещенный во вторую</param>
        /// <param name="presentFirst">Представление предмета, которое нужно переместить из первой во вторую ячейкеу</param>
        /// <param name="presentSecond">Представление предмета, которое нужно переместить из второй в первую ячейкеу</param>
        public void Shuffle(ICell<ItemDescription> first, ICell<ItemDescription> second, ICellUI presentFirst, ICellUI presentSecond)
        {
            var objFirst = CreateAnimObject(presentFirst);
            var objSecond = CreateAnimObject(presentSecond);

            var itemFirst = objFirst.GetComponent<IItemUI>();
            var itemSecond = objSecond.GetComponent<IItemUI>();

            itemFirst.SetItem(second.Item.Description);
            itemSecond.SetItem(first.Item.Description);
            itemFirst.EnableStatus = true;
            itemSecond.EnableStatus = true;

            presentFirst.SetItemToCell(first.Item);
            presentSecond.SetItemToCell(second.Item);

            presentFirst.SetItemVisibleStatus(false);
            presentSecond.SetItemVisibleStatus(false);

            objFirst.SetActive(true);
            objSecond.SetActive(true);

            var positionToFirst = GetPosition(presentSecond);
            var positionToSecond = GetPosition(presentFirst);

            var result = new Hashtable(2)
            {
                { "GameObjectFirst", objFirst },
                { "GameObjectSecond", objSecond },
                { "IItemUIFirst", itemFirst },
                { "IItemUISecond", itemSecond },
                { "ICellUIToFirst", presentFirst },
                { "ICellUIToSecond", presentSecond }
            };

            iTween.MoveTo(objFirst, iTween.Hash("position", transform.InverseTransformPoint(positionToFirst),
                                                "time", _shuffleTime,
                                                "islocal", true,
                                                "easetype", iTween.EaseType.easeInOutCubic,
                                                "looptype", iTween.LoopType.none));

            iTween.MoveTo(objSecond, iTween.Hash("position", transform.InverseTransformPoint(positionToSecond),
                                                 "time", _shuffleTime,
                                                 "islocal", true,
                                                 "easetype", iTween.EaseType.easeInOutCubic,
                                                 "looptype", iTween.LoopType.none,
                                                 "oncomplete", nameof(CompleteShuffle),
                                                 "oncompletetarget", gameObject,
                                                 "oncompleteparams", result));
        }

        /// <summary>
        /// Удаление матчей
        /// </summary>
        /// <param name="cell">Описание предмета в ячейке</param>
        /// <param name="present">Представление предмета на поле</param>
        public void ClearMatch(ICell<ItemDescription> cell, ICellUI present)
        {
            var obj = CreateAnimObject(present);

            var item = obj.GetComponent<IItemUI>();

            item.SetItem(cell.Item.Description);
            item.EnableStatus = true;

            obj.SetActive(true);

            present.SetItemVisibleStatus(false);
            present.ClearCell();

            var result = new Hashtable(2)
            {
                { "GameObject", obj },
                { "IItemUI", item },
                { "ICellUI", present }
            };

            _startedAnims++;

            iTween.ScaleTo(obj, iTween.Hash("scale", _clearMatchScale, 
                                            "time", _clearMatchTime,
                                            "easetype", iTween.EaseType.easeInExpo,
                                            "looptype", iTween.LoopType.none, 
                                            "oncomplete", nameof(CompleteClearMatch),
                                            "oncompletetarget", gameObject,
                                            "oncompleteparams", result));
        }
        #endregion

        #region PrivateMethods
        private void CompleteMoveItem(object result)
        {
            var data = (Hashtable)result;

            var returnedGameObject = (GameObject)data["GameObject"];
            returnedGameObject.transform.localScale = Vector3.one;
            ((IItemUI)data["IItemUI"]).Clear();
            ((ICellUI)data["ICellUITo"]).SetItemVisibleStatus(true);

            _pool.ReturnToPool(returnedGameObject);

            _startedAnims--;

            if (_startedAnims == 0)
            {
                if (OnCompleteAllAnimations != null)
                {
                    OnCompleteAllAnimations.Invoke();
                }
            }
        }

        private void CompleteShuffle(object result)
        {
            var data = (Hashtable)result;

            var returnedGameObjectFirst = (GameObject)data["GameObjectFirst"];
            var returnedGameObjectSecond = (GameObject)data["GameObjectSecond"];

            ((IItemUI)data["IItemUIFirst"]).Clear();
            ((IItemUI)data["IItemUISecond"]).Clear();

            ((ICellUI)data["ICellUIToFirst"]).SetItemVisibleStatus(true);
            ((ICellUI)data["ICellUIToSecond"]).SetItemVisibleStatus(true);

            _pool.ReturnToPool(returnedGameObjectFirst);
            _pool.ReturnToPool(returnedGameObjectSecond);

            if (OnCompleteAllAnimations != null) OnCompleteAllAnimations.Invoke();
        }

        private void CompleteClearMatch(object result)
        {
            var data = (Hashtable)result;

            var returnedGameObject = (GameObject)data["GameObject"];
            returnedGameObject.transform.localScale = Vector3.one;
            ((IItemUI)data["IItemUI"]).Clear();

            _pool.ReturnToPool(returnedGameObject);

            _startedAnims--;

            if (_startedAnims == 0)
            {
                if (OnCompleteAllAnimations != null)
                {
                    OnCompleteAllAnimations.Invoke();
                }
            }
        }

        private GameObject CreateAnimObject(ICellUI present)
        {
            var obj = _pool.GetObject();

            obj.transform.SetParent(transform, false);
            obj.transform.SetAsLastSibling();

            var objRectTransform = (obj.transform as RectTransform);
            objRectTransform.anchorMin = _anchor;
            objRectTransform.anchorMax = _anchor;
            objRectTransform.sizeDelta = present.GetRectTransform.sizeDelta;

            objRectTransform.position = GetPosition(present);

            return obj;
        }

        private Vector3 GetPosition(ICellUI present)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(present.GetRectTransform,
                                                                         RectTransformUtility.WorldToScreenPoint(Camera.main, present.GetRectTransform.position),
                                                                         Camera.main,
                                                                         out var pos))
            {
                return pos;
            }

            return Vector3.zero;
        }
        #endregion
    }
}
