using MatchThree.Interfaces;

namespace MatchThree.Objects
{
    public class Emitter<T> : IEmitter<T> where T : class
    {
        private IDatabase<T> _data;

        public Emitter(IDatabase<T> data)
        {
            _data = data;
        }

        public bool Generate(out T item)
        {
            var index = UnityEngine.Random.Range(1, _data.Size);

            return _data.GetByIndex(index, out item);
        }
    }
}
