using UnityEngine;

public class InputController : MonoBehaviour
{
    public Vector2 Smooth { get; private set; }

    //[SerializeField] private Vector2 _smooth = new Vector2();

    void Start()
    {
        Smooth = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        //Smooth = _smooth;
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Smooth += touch.deltaPosition.normalized * 3f;
            }
        }
    }
}
