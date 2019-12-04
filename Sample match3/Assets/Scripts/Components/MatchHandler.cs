using System.Collections.Generic;
using System.Linq;
using MatchThree.Interfaces;
using MatchThree.Objects;
using MatchThree.Controllers;

namespace MatchThree.Components
{

    public class MatchHandler : BaseComponent, IMatchHandler
    {
        #region Controllers

        private IDrawController _drawer;

        #endregion

        #region StandartMethodEvents

        private void Start()
        {
            if(!ServiceContainer.GetService(out _drawer))
            {
                throw new System.NullReferenceException("[MatchHandler] Draw controller not found");
            }
        }

        #endregion

        #region PublicMethods
        
        /// <summary>
        /// Поиск и удаление всех матчей на поле
        /// </summary>
        /// <param name="source"> Игровое поле на котором проводится поиск матчей </param>
        /// <returns></returns>
        public bool ClearAllMatches(IField<ItemDescription> source)
        {
            if (source == null) return false;

            var maxLength = source.Rows > source.Columns ? source.Rows : source.Columns;

            for (int i = 0; i < maxLength; i++)
            {
                if (i < source.Rows)
                {
                    var line = source.Get(cells => cells.RowPosition == i);
                    MarkLine(line);
                }

                if (i < source.Columns)
                {
                    var line = source.Get(cells => cells.ColumnPosition == i);
                    MarkLine(line);
                }
            }

            var isCleared = false;

            foreach (var cell in source.GetAll())
            {
                if (cell.Item.MatchID > 0)
                {
                     _drawer.ClearCell(cell, true);
                    cell.Item.Clear();
                    isCleared = true;
                }
            }

            return isCleared;
        }

        /// <summary>
        /// Поиск матчей начиная из конкретной ячейки по горизонтали и по вертикали.
        /// Возвращает true если был найден хотябы один матч.
        /// </summary>
        /// <param name="rowPosition"> Позиция ячейки в строке </param>
        /// <param name="columnPosition"> Позиция ячейки в столбце </param>
        /// <param name="source"> Игровое поле на котором проводится поиск матчей </param>
        /// <returns></returns>
        public bool CheckMatchWithCell(int rowPosition, int columnPosition, IField<ItemDescription> source)
        {
            var columnCells = source.Get(cells => cells.ColumnPosition == columnPosition).ToList();
            var rowCells = source.Get(cells => cells.RowPosition == rowPosition).ToList();

            int lengthFwd = 0;
            int lengthBwd = 0;

            for (int i = 1; i < 3; i++)
            {
                if (rowPosition - i >= 0 && lengthBwd + 1 >= i)
                {
                    if (columnCells[rowPosition - i].IsActive)
                    {
                        lengthBwd = columnCells[rowPosition].Item.ID == columnCells[rowPosition - i].Item.ID ? lengthBwd + 1 : lengthBwd;
                    }
                }

                if (rowPosition + i < columnCells.Count && lengthFwd + 1 >= i)
                {
                    if (columnCells[rowPosition + i].IsActive)
                    {
                        lengthFwd = columnCells[rowPosition].Item.ID == columnCells[rowPosition + i].Item.ID ? lengthFwd + 1 : lengthFwd;
                    }
                }
                
                if (lengthFwd == 2 || lengthBwd == 2 || (lengthFwd + lengthBwd == 2)) return true;
            }

            lengthFwd = 0;
            lengthBwd = 0;

            for (int i = 1; i < 3; i++)
            {
                if (columnPosition - i >= 0 && lengthBwd + 1 >= i)
                {
                    if (rowCells[columnPosition - i].IsActive)
                    {
                        lengthBwd = rowCells[columnPosition].Item.ID == rowCells[columnPosition - i].Item.ID ? lengthBwd + 1 : lengthBwd;
                    }
                }

                if (columnPosition + i < rowCells.Count && lengthFwd + 1 >= i)
                {
                    if (rowCells[columnPosition + i].IsActive)
                    {
                        lengthFwd = rowCells[columnPosition].Item.ID == rowCells[columnPosition + i].Item.ID ? lengthFwd + 1 : lengthFwd;
                    }
                }
                if (lengthFwd == 2 || lengthBwd == 2 || (lengthFwd + lengthBwd == 2)) return true;
            }

            return false;
        }

        #endregion

        #region PrivateMethods

        private void MarkLine(IEnumerable<ICell<ItemDescription>> cells)
        {
            var line = cells.ToList();

            bool collectMatch = false;

            for (int i = 1; i < line.Count - 1; i++)
            {
                if (collectMatch)
                {
                    if (line[i + 1].IsActive && (line[i + 1].Item.ID == line[i].Item.ID))
                    {
                        line[i + 1].Item.MatchID++;
                    }
                    else
                    {
                        collectMatch = false;
                    }
                }

                if (line[i - 1].IsActive && line[i].IsActive && line[i + 1].IsActive)
                {
                    if ((line[i - 1].Item.ID == line[i].Item.ID) && (line[i + 1].Item.ID == line[i].Item.ID))
                    {
                        collectMatch = true;
                        line[i - 1].Item.MatchID++;
                        line[i].Item.MatchID++;
                        line[i + 1].Item.MatchID++;
                    }
                }
            }
        }

        #endregion
    }
}
