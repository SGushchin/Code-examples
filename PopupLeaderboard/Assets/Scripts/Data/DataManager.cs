using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour, IDataManager<Player>
{
    [SerializeField]
    private string _directoryPath;

    [SerializeField]
    private string _fileName = "Players.json";

    private string _path;

    private IData<Player> _data;


    public void Awake()
    {
        if (_directoryPath == null)
        {
            _directoryPath = Application.dataPath;
        }

        if (_fileName == null)
        {
            _fileName = "Default";
        }

        _path = System.IO.Path.Combine(_directoryPath, _fileName);

        _data = new JsonDataFromLocalDisc<Player>();
    }


    public void SaveDataByID(int id, Player player)
    {
        var data = _data.Load(_path).ToList();

        var findedPlayer = data.Find(findPlayer => findPlayer.ID == id);

        findedPlayer.FirstName = player.FirstName;
        findedPlayer.LastName = player.LastName;
        findedPlayer.Score = player.Score;
        
        _data.Save(data.ToArray(), _path);
    }


    public void SaveAll(List<Player> allPlayers) => _data.Save(allPlayers.ToArray(), _path);


    public Player LoadDataByID(int id)
    {
        var loadedData = _data.Load(_path);

        if (loadedData is null) return null;

        return loadedData.Where(player => player.ID == id).ToList()[0];
    }


    public IEnumerable<Player> LoadAll()
    {
        var loadedData = _data.Load(_path);

        if (loadedData is null) return null;

        return loadedData.ToList();
    }
    
}
