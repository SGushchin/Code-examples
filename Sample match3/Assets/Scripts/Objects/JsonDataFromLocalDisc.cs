using UnityEngine;
using System.IO;
using MatchThree.Interfaces;

namespace MatchThree.Objects
{
    public class JsonDataFromLocalDisc<T> : IData<T> where T : class
    {
        public void Save(T data, string path = null)
        {
            if (data == null) return;

            var str = JsonUtility.ToJson(data);

            File.WriteAllText(path, str);
        }

        public T Load(string path = null)
        {
            if (!File.Exists(path)) return null;

            var str = File.ReadAllText(path);
            
            return JsonUtility.FromJson<T>(str);
        }
    }
}