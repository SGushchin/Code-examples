using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private Vector3 _forceVector;
    private Rigidbody _rb;
    private InputController _inputController;
    [SerializeField] private float _reduceForce = 3f;

    private bool _onSurface = false;

    private void Awake()
    {
        _inputController = GameObject.FindObjectOfType<InputController>() ?? throw new System.NullReferenceException("[BallBehaviour] Input not found");
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (Time.frameCount % 4 == 0)
        {
            if (_onSurface) _forceVector = _inputController.Smooth.ToXZ();
            else _forceVector = Vector3.zero;

            _rb.AddForce(_forceVector / _reduceForce, ForceMode.Force);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _onSurface = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _onSurface = true;
    }
}

public static class Ext
{
    public static Vector3 ToXZ(this Vector2 slice)
    {
        return new Vector3(slice.y, 0f, (-1f) * slice.x);
    }
}
