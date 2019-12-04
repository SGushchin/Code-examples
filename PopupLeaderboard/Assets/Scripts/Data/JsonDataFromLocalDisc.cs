using UnityEngine;
using System.IO;
using System.Text;

public class JsonDataFromLocalDisc<T> : IData<T>
{
    public void Save(T[] data, string path = null)
    {
        if (data == null) return;

        var str = new StringBuilder();
        
        foreach (var d in data)
        {
            str.AppendLine(JsonUtility.ToJson(d));
        }
        
        File.WriteAllText(path, str.ToString());
    }

    public T[] Load(string path = null)
    {
        if (!File.Exists(path)) return null;
        
        var str = File.ReadAllLines(path);

        var data = new T[str.Length];

        for (int i = 0; i < str.Length; i++)
        {
            data[i] = JsonUtility.FromJson<T>(str[i]);
        }
        
        return data;
    }
}
