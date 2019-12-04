using System.Linq;
using MatchThree.Interfaces;
using MatchThree.Objects;
using MatchThree.Components;

namespace MatchThree.Controllers
{
    public class MotionHandler : BaseComponent, IMotionHandler
    {
        #region Controllers

        private IDrawController _drawer;

        #endregion

        #region PrivateVariables

        private IEmitter<ItemDescription> _emitter;

        #endregion

        #region StandartMethodEvents

        private void Start()
        {
            if (ServiceContainer.GetService<IDatabase<ItemDescription>>(out var data))
            {
                _emitter = new Emitter<ItemDescription>(data);
            }
            else
            {
                throw new System.Exception("[MotionHandler] Emitter was not created");
            }

            if (!ServiceContainer.GetService(out _drawer))
            {
                throw new System.NullReferenceException("[MotionHandler] Draw controller not found");
            }
        }

        #endregion

        #region PublicMethods

        /// <summary>
        /// Обмен предметов местами
        /// </summary>
        /// <param name="field"> Игровое поле </param>
        /// <param name="firstRowPos"> Позиция строки первого предмета </param>
        /// <param name="firstColumnPos"> Позиция колонки первого предмета </param>
        /// <param name="secondRowPos"> Позиция строки второго предмета </param>
        /// <param name="secondColumnPos"> Позиция колонки второго предмета </param>
        /// <returns></returns>
        public bool Shuffle(IField<ItemDescription> field, int firstRowPos, int firstColumnPos, int secondRowPos, int secondColumnPos)
        {
            if (firstRowPos != secondRowPos && firstColumnPos != secondColumnPos) return false;

            if (UnityEngine.Mathf.Abs(firstRowPos - secondRowPos) > 1f || UnityEngine.Mathf.Abs(firstColumnPos - secondColumnPos) > 1f) return false;

            var firstCell = field.Get(findCell => findCell.RowPosition == firstRowPos && findCell.ColumnPosition == firstColumnPos).ToArray()[0];
            var secondCell = field.Get(findCell => findCell.RowPosition == secondRowPos && findCell.ColumnPosition == secondColumnPos).ToArray()[0];

            if (!firstCell.IsActive && firstCell.Item.IsEmpty && !firstCell.IsActive && firstCell.Item.IsEmpty) return false;
            
            var temp = secondCell.Item.Description;
            
            secondCell.Item.Set(firstCell.Item.ID, firstCell.Item.Description);
            firstCell.Item.Set(temp.ID, temp);
            
            _drawer.Shuffle(firstCell, secondCell, true);
            
            return true;
        }

        /// <summary>
        /// Метод вертикального перемещения предметов
        /// </summary>
        /// <param name="field"> Игровое поле в котором перемещаются фишки </param>
        /// <returns></returns>
        public bool VerticalMovement(IField<ItemDescription> field)
        {
            var rows = field.Rows;

            var columns = field.Columns;

            var emptyAmount = 0;

            var status = false;
            
            for (int i = 0; i < columns; i++)
            {
                emptyAmount = 0;

                var column = field.Get(findColumn => findColumn.ColumnPosition == i).ToList();

                for (int j = column.Count() - 1; j >= 0; j--)
                {
                    if (column[j].IsActive)
                    {
                        if (column[j].Item.IsEmpty)
                        {
                            if (column[j].IsEmitter)
                            {
                                do
                                {
                                    if (_emitter.Generate(out var item))
                                    {
                                        column[j + emptyAmount].Item.Set(item.ID, item);
                                        _drawer.MoveItemFromTo(column[j], column[j + emptyAmount], true);
                                        emptyAmount--;
                                    }
                                } while (emptyAmount >= 0);

                                emptyAmount = 0;
                                status = true;
                            }
                            else 
                            {
                                emptyAmount++;
                            }
                        }
                        else if (emptyAmount > 0)
                        {
                            column[j + emptyAmount].Item.Set(column[j].Item.Description.ID, column[j].Item.Description);
                            _drawer.MoveItemFromTo(column[j], column[j + emptyAmount], true);
                            column[j].Item.Clear();
                            _drawer.ClearCell(column[j], false);
                            
                            if (column[j].IsEmitter)
                            {
                                emptyAmount--;
                                do
                                {
                                    if (_emitter.Generate(out var item))
                                    {
                                        column[j + emptyAmount].Item.Set(item.ID, item);
                                        _drawer.MoveItemFromTo(column[j], column[j + emptyAmount], true);
                                        emptyAmount--;
                                    }
                                } while (emptyAmount >= 0);

                                emptyAmount = 0;
                                status = true;
                            }
                        }
                    }
                    else
                    {
                        emptyAmount = 0;
                    }
                }
            }

            return status;
        }

        /// <summary>
        /// Метод диагонального перемещения предметов
        /// </summary>
        /// <param name="field"> Игровое поле в котором перемещаются фишки </param>
        /// <returns></returns>
        public bool DiagonalMovement(IField<ItemDescription> field)
        {
            var rows = field.Rows;

            var columns = field.Columns;

            var isMoved = false;

            for (int i = rows - 2; i >= 0; i--)
            {
                if (isMoved) return isMoved;
                
                var rowMain = field.Get(findRow => findRow.RowPosition == i).ToArray();
                var rowUnder = field.Get(findRow => findRow.RowPosition == i + 1).ToArray();

                for (int j = 0; j < rowMain.Count(); j++)
                {
                    if (rowMain[j].IsActive && !rowMain[j].Item.IsEmpty)
                    {
                        if (CheckCellForActiveAndEmpty(rowUnder, j + 1))
                        {
                            SetItemToCellOnDiagonal(rowMain[j], rowUnder[j + 1]);

                            isMoved = true;
                        }
                        else if (CheckCellForActiveAndEmpty(rowUnder, j - 1))
                        {
                            SetItemToCellOnDiagonal(rowMain[j], rowUnder[j - 1]);

                            isMoved = true;
                        }
                    }
                }
            }

            return isMoved;
        }

        #endregion

        #region PrivateMethods

        private bool CheckCellForActiveAndEmpty(ICell<ItemDescription>[] row, int pos)
        {
            if (pos < row.Count() && pos >= 0)
            {
                if (row[pos].IsActive && row[pos].Item.IsEmpty)
                {
                    return true;
                }
            }

            return false;
        }

        private void SetItemToCellOnDiagonal(ICell<ItemDescription> source, ICell<ItemDescription> destination)
        {
            destination.Item.Set(source.Item.ID, source.Item.Description);
            _drawer.MoveItemFromTo(source, destination, true);
            source.Item.Clear();
            _drawer.ClearCell(source, false);
        }

        #endregion
    }
}