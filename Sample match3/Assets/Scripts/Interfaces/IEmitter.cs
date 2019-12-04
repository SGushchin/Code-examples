namespace MatchThree.Interfaces
{
    public interface IEmitter<T> where T : class
    {
        bool Generate(out T item);
    }
}