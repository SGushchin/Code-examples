using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingBar : MonoBehaviour, ILoadingBar
{
    [SerializeField] private float _timeout = 1f;

    private Image _circle;


    private void Awake()
    {
        _circle = GetComponent<Image>();
    }

    public void Play()
    {
        StartCoroutine(nameof(Run));
    }

    public void Stop()
    {
        StopCoroutine(nameof(Run));
        _circle.fillAmount = 0f;
    }


    private IEnumerator Run()
    {
        yield return new WaitForSecondsRealtime(_timeout);

        var dir = 1f;

        while (true)
        {

            if (_circle.fillAmount > 0.99f)
            {
                _circle.fillClockwise = false;

                dir = -1f;
            }
            else if (_circle.fillAmount < 0.01f)
            {
                _circle.fillClockwise = true;

                dir = 1f;
            }

            var result =  _circle.fillAmount + 0.03f * dir;

            _circle.fillAmount = Mathf.Clamp01(result);

            yield return new WaitForEndOfFrame();
        }
    }

}
