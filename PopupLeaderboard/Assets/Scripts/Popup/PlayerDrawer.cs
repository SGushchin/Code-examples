using System.Collections.Generic;
using UnityEngine;

public class PlayerDrawer : MonoBehaviour, IDrawer<Player>
{
    private GameObject _slotPrefab;

    private PopupController _controller;

    private bool _slotIsNull = true;

    private bool _controllerIsNull = true;

    private bool _fieldIsNull = true;

    private PlayerField _playerField;

    private void Awake()
    {
        _slotPrefab = Resources.Load<GameObject>("PlayerField");

        if (!(_slotPrefab is null)) _slotIsNull = false;

        _controller = GetComponentInParent<PopupController>();

        if (!(_controller is null)) _controllerIsNull = false;

        _playerField = GetComponent<PlayerField>();

        if (!(_playerField is null)) _fieldIsNull = false;
    }


    public void Draw(List<Player> data)
    {
        if (data == null) return;

        if (_slotIsNull) return;

        if (_controllerIsNull) return;

        var index = data.FindIndex(player => player.ID == _controller.curentPlayerID);

        if (_fieldIsNull) return;

        _playerField.position = index + 1;

        _playerField.firstName = data[index].FirstName;

        _playerField.lastName = data[index].LastName;

        _playerField.score = data[index].Score;
    }

    public void Clear()
    {
        if (_fieldIsNull) return;

        _playerField.position = 0;

        _playerField.firstName = string.Empty;

        _playerField.lastName = string.Empty;

        _playerField.score = 0;
    }
}
