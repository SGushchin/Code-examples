using UnityEngine;

public class PopupAnimation : MonoBehaviour, IPopupAnimation
{
    private Animator _animator;

    private bool _isNull = true;


    private static readonly int OnShowTrigger = Animator.StringToHash("Show");

    private static readonly int OnHideTrigger = Animator.StringToHash("Hide");

    private static readonly int IsEnableBool = Animator.StringToHash("IsEnable");


    private bool _isEnabled;


    private void Awake()
    {
        _animator = GetComponent<Animator>();

        if (_animator != null) _isNull = false;
    }


    public void Play(bool IsEnabled)
    {
        if (_isNull) return;

        _isEnabled = IsEnabled;

        if (IsEnabled)
        {
            _animator.SetTrigger(OnShowTrigger);
        }
        else
        {
            _animator.SetTrigger(OnHideTrigger);
        }
    }


    private void SwitchStateBool()
    {
        _animator.SetBool(IsEnableBool, _isEnabled);
    }
}