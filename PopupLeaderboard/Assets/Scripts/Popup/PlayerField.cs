using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerField : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _position;

    [SerializeField]
    private Image _icon;

    [SerializeField]
    private TextMeshProUGUI _firstName;

    [SerializeField]
    private TextMeshProUGUI _lastName;

    [SerializeField]
    private TextMeshProUGUI _score;

    
    public int position { set => _position.text = value.ToString(); }

    public Sprite icon { set => _icon.sprite = value; }

    public string firstName { set => _firstName.text = value; }

    public string lastName { set => _lastName.text = value; }

    public int score { set => _score.text = value.ToString(); }
}
