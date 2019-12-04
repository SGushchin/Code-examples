public interface IData<T>
{
    T[] Load(string path = null);
    void Save(T[] data, string path = null);
}