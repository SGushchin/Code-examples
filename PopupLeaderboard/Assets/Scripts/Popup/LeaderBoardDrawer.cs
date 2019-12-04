using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardDrawer : MonoBehaviour, IDrawer<Player>
{
    private GameObject _slotPrefab;

    private PopupController _controller;

    private bool _slotIsNull = true;

    private bool _controllerIsNull = true;

    private List<PlayerField> _allCreatedFields = new List<PlayerField>();


    private void Awake()
    {
        _slotPrefab = Resources.Load<GameObject>("PlayerField");

        if (!(_slotPrefab is null)) _slotIsNull = false;

        _controller = GetComponentInParent<PopupController>();

        if (!(_controller is null)) _controllerIsNull = false;
    }
    

    public void Draw(List<Player> data)
    {
        if (data == null) return;

        if (_slotIsNull) return;

        if (_controllerIsNull) return;

        var max = _controller.numberDisplayedPlayers < data.Count ? _controller.numberDisplayedPlayers : data.Count;

        for (int i = 0; i < max; i++)
        {
            var playerField = Instantiate(_slotPrefab, transform).GetComponent<PlayerField>();

            playerField.position = i + 1;

            playerField.firstName = data[i].FirstName;

            playerField.lastName = data[i].LastName;

            playerField.score = data[i].Score;

            _allCreatedFields.Add(playerField);
        }
        
    }


    public void Clear()
    {
        foreach(var field in _allCreatedFields)
        {
            DestroyImmediate(field.gameObject);
        }

        _allCreatedFields.Clear();
    }
}
