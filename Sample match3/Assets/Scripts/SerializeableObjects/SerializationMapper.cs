using System.Collections.Generic;
using MatchThree.Interfaces;

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MatchThree.Objects
{
    public static class SerializationMapper
    {
        public static SerializableItem ToSerializableItem<T>(this IItem<T> item)
        {
            return new SerializableItem() { ID = item.ID, IsEmpty = item.IsEmpty };
        }

        public static SerializableCell ToSerializableCell<T>(this ICell<T> cell)
        {
            return new SerializableCell()
            {
                IsActive = cell.IsActive,
                IsEmitter = cell.IsEmitter,
                Row = cell.RowPosition,
                Column = cell.ColumnPosition,
                Item = cell.Item.ToSerializableItem()
            };
        }

        public static SerializableField ToSerializableField<T>(this IField<T> field)
        {
            var serializableCells = new List<SerializableCell>(field.Columns * field.Rows);

            foreach (var cell in field.GetAll())
            {
                serializableCells.Add(cell.ToSerializableCell());
            }

            return new SerializableField()
            {
                Columns = field.Columns,
                Rows = field.Rows,
                Cells = serializableCells
            };
        }

        public static Item<ItemDescription> ToItem(this SerializableItem item, IDatabase<ItemDescription> data)
        {
            var newItem = new Item<ItemDescription>();

            if(!item.IsEmpty)
            {
                if (data.Get(findItem => findItem.ID == item.ID, out var thisItem))
                {
                    newItem.Set(item.ID, thisItem);
                }
            }

            return newItem;
        }

        public static Cell<ItemDescription> ToCell(this SerializableCell cell, IDatabase<ItemDescription> data)
        {
            return new Cell<ItemDescription>(cell.Row, cell.Column, cell.Item.ToItem(data), cell.IsActive, cell.IsEmitter);
        }

        public static Field<ItemDescription> ToField(this SerializableField field, IDatabase<ItemDescription> data)
        {
            var cells = new List<ICell<ItemDescription>>(field.Rows * field.Columns);

            foreach (var cell in field.Cells)
            {
                cells.Add(cell.ToCell(data));
            }

            return new Field<ItemDescription>(field.Rows, field.Columns, cells);
        }
    }

}
