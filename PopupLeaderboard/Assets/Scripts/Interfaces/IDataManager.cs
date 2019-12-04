using System.Collections.Generic;

public interface IDataManager<T>
{
    void SaveDataByID(int id, T player);
    void SaveAll(List<T> allPlayers);
    T LoadDataByID(int id);
    IEnumerable<T> LoadAll();
}