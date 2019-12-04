using System.Collections.Generic;

public interface IDrawer<T>
{
    void Draw(List<T> data);

    void Clear();
}