using System.Collections.Generic;
using UnityEngine;
using MatchThree.Interfaces;
using MatchThree.Objects;

namespace MatchThree.Components
{
    public class FieldCreator : BaseComponent, IFieldCreator
    {
        #region SerializableFields

        [SerializeField]
        private string _cellPrefabTag = "Cell";

        [SerializeField]
        [Range(0f, 150f)]
        private float _cellWidth = 100;

        [SerializeField]
        [Range(0f, 150f)]
        private float _cellHeight = 100;

        #endregion

        #region PrivateVariables

        private int _rows;

        private int _columns;

        private GameObject _cellPrefab;

        private bool _cellPrefabIsNull = true;

        private List<GameObject> _cashedField = new List<GameObject>();

        #endregion

        #region StandartMethodEvents

        private void Start()
        {
            if (ServiceContainer.GetService<IDatabase<GameObject>>(out var cash))
            {
                _cellPrefabIsNull = !cash.Get(findPrefab => findPrefab.CompareTag(_cellPrefabTag), out _cellPrefab);
            }
        }

        #endregion
        
        #region PublicMethods

        /// <summary>
        /// Создаёт визуальное предстваление поля на основе игрового поля
        /// </summary>
        /// <param name="gameField"> Игровой поле, которое нужно визуализировать </param>
        /// <returns></returns>
        public ICellUI[,] CreateField(IField<ItemDescription> gameField)
        {
            if (_cashedField.Count > 0) Clear();
            
            _rows = gameField.Rows;

            _columns = gameField.Columns;

            var fieldCells = new ICellUI[_rows, _columns];

            var cells = gameField.GetAll();

            foreach (var cell in cells)
            {
                fieldCells[cell.RowPosition, cell.ColumnPosition] = AddCellToField(cell);
            }

            return fieldCells;
        }

        #endregion

        #region PrivateMethods

        private ICellUI AddCellToField(ICell<ItemDescription> cell)
        {
            var row = (_cellHeight / 2f) * (_rows - (2f * cell.RowPosition) - 1f);

            var column = (_cellWidth / 2f) * (((-1f) * _columns) + (2f * cell.ColumnPosition) + 1f);

            var createdCell = Instantiate(_cellPrefab, transform);

            var uiCell = createdCell.GetComponent<ICellUI>();

            uiCell.GetRectTransform.anchoredPosition = new Vector2(column, row);

            uiCell.GetRectTransform.sizeDelta = new Vector2(_cellWidth, _cellHeight);

            uiCell.ConfigCell(cell);

            _cashedField.Add(createdCell);

            return uiCell;
        }

        private void Clear()
        {
            foreach (var cell in _cashedField)
            {
                Destroy(cell);
            }

            _cashedField.Clear();
        }

        #endregion
    }
}
