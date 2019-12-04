using System.Collections.Generic;
using UnityEngine;

public class JsonGenerator : MonoBehaviour
{
    [SerializeField]
    private string _directoryPath;

    [SerializeField]
    private string _fileName = "Players.json";

    [Space(10)]
    [SerializeField]
    private List<string> _firstName;

    [SerializeField]
    private List<string> _lastName;

    [SerializeField]
    [Range(0f, 50000f)]
    private int _playersAmount = 0;

    [SerializeField]
    [Range(0f, 50000f)]
    private int _maxScoreNumber = 0;

    private string _filePath;

    private int _id = 0;

    private const int MIN = 0;

    private const int MAX = 50000;

    
    private void Init()
    {
        if (_directoryPath == null)
        {
            _directoryPath = Application.dataPath;
        }

        if (_fileName == null)
        {
            _fileName = "Default";
        }

        _filePath = System.IO.Path.Combine(_directoryPath, _fileName);
    }

    public void GenerateJson()
    {
        Init();

        if (_filePath == null) return;
        if (_firstName == null || _firstName.Count <= 0) return;
        if (_lastName == null || _lastName.Count <= 0) return;

        int index;
        string firstName;
        string lastName;
        int score;

        List<Player> players = new List<Player>(_playersAmount);

        for (_id = 0; _id < _playersAmount; _id++)
        {
            index = Random.Range(MIN, _firstName.Count - 1);
            firstName = _firstName[index];

            index = Random.Range(MIN, _lastName.Count - 1);
            lastName = _lastName[index];

            score = Random.Range(MIN, _maxScoreNumber);

            var newPlayer = new Player
            {
                ID = _id,
                FirstName = firstName,
                LastName = lastName,
                Score = score
            };

            players.Add(newPlayer);
        }

        var data = new JsonDataFromLocalDisc<Player>();

        data.Save(players.ToArray(), _filePath);
    }
}
