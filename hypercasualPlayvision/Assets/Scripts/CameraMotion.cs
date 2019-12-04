using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothMinAngleX = -70f;
    [SerializeField] private float _smoothMaxAngleX = 70f;
    [SerializeField] private float _smoothMinAngleY = -10f;
    [SerializeField] private float _smoothMaxAngleY = 70f;

    private bool _targetIsNull = true;
    private InputController _inputController;

    private void Awake()
    {
        _inputController = GameObject.FindObjectOfType<InputController>() ?? throw new System.NullReferenceException("[CameraMotiom] Input not found");
    }

    private void Start()
    {
        if (!(_target is null)) _targetIsNull = false;
    }
    
    private void LateUpdate()
    {
        if (!_targetIsNull)
        {
            var smooth = _inputController.Smooth / 3f;

            var x = Mathf.Clamp(smooth.x, _smoothMinAngleX, _smoothMaxAngleX);
            var y = Mathf.Clamp(smooth.y, _smoothMinAngleY, _smoothMaxAngleY);

            var yRot = Quaternion.AngleAxis(x, Vector3.right);
            var zRot = Quaternion.AngleAxis(y, Vector3.forward);

            transform.rotation = yRot * zRot;

            transform.position = _target.position;
        }
    }
}
