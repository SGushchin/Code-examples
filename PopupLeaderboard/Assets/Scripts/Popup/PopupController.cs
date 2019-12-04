using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private int _numOfDisplayedPlayers = 5;

    [SerializeField] private int _curentPlayerID = 0;


    private IDataManager<Player> _dataManager;

    private IPopupAnimation _animation;

    private IDrawer<Player> [] _drawers;


    private bool _dataManagerIsNull = true;

    private bool _animationIsNull = true;

    private bool _isEnable = false;

    private List<Player> _allPlayers;


    public int numberDisplayedPlayers { get => _numOfDisplayedPlayers; }

    public int curentPlayerID { get => _curentPlayerID; }


    private void Awake()
    {
        _dataManager = FindObjectOfType<DataManager>();

        _animation = GetComponentInChildren<IPopupAnimation>();

        if (!(_dataManager is null)) _dataManagerIsNull = false;

        if (!(_animation is null)) _animationIsNull = false;

        _allPlayers = new List<Player>(_numOfDisplayedPlayers);

        _drawers = GetComponentsInChildren<IDrawer<Player>>();
    }


    public void SwithState()
    {
        _isEnable = !_isEnable;

        if (_dataManagerIsNull) return;

        if (_isEnable)
        {
            var data = _dataManager.LoadAll();

            if (data == null) 
            {
                _isEnable = false;
                return;
            }
        
            _allPlayers = data.AsParallel().OrderByDescending(player => player.Score).ToList(); 
        }

        if (_animationIsNull) return;

        _animation.Play(_isEnable);

        if (_isEnable)
        {
            for (int i = 0; i < _drawers.Length; i++)
            {
                _drawers[i]?.Draw(_allPlayers);
            }
        }
        else
        {
            for (int i = 0; i < _drawers.Length; i++)
            {
                _drawers[i]?.Clear();
            }
        }
    }
}


