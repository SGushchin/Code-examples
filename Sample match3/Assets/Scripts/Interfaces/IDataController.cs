namespace MatchThree.Interfaces
{
    public interface IDataController<T> where T : class
    {
        bool Load(out T data);
        bool Save(T data);
    }
}