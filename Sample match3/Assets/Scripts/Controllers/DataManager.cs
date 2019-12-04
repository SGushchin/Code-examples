using UnityEngine;
using MatchThree.Interfaces;
using MatchThree.Database;
using MatchThree.Objects;
using MatchThree.Components;
using MatchThree.Controllers;

namespace MatchThree
{
    public class DataManager : BaseComponent
    {
        #region SerializeFields

        [SerializeField] private string _itemDescriptionPath = "Items";

        [SerializeField] private string _prefabsPath = "Prefabs";

        #endregion

        private void Awake()
        {
            var descriptions = Resources.LoadAll<ItemDescription>(_itemDescriptionPath);
            var prefabs = Resources.LoadAll<GameObject>(_prefabsPath);
            
            var _cashedItemsDescription = new DataContainer<ItemDescription>(descriptions);
            var _cashedPrefabs = new DataContainer<GameObject>(prefabs);


            ServiceContainer.RegisterService<IDatabase<ItemDescription>>(_cashedItemsDescription);
            ServiceContainer.RegisterService<IDatabase<GameObject>>(_cashedPrefabs);

            
            var dataController = new DataController<SerializableField>(new JsonDataFromLocalDisc<SerializableField>());
            ServiceContainer.RegisterService<IDataController<SerializableField>>(dataController);

            Destroy(this);
        }
    }
}
