using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float _zOffset = 10.0f;
    [SerializeField]
    private float _smoothDamp = 1.0f;
    [SerializeField]
    private float _movementThreshold = 1.0f;

    private GameObject _player1;
    public GameObject Player1
    {
        get { return _player1; }
        set { _player1 = value; }
    }
    private GameObject _player2;
    public GameObject Player2
    {
        get { return _player2; }
        set { _player2 = value; }
    }

    private float _velocity = 0.0f;

    private void LateUpdate()
    {
        if (!_player1 || !_player2) return;

        float targetZ = (_player1.transform.position.z + _player2.transform.position.z) / 2.0f - _zOffset;

        float deltaZ = targetZ - transform.position.z;

        float finalTargetZ = transform.position.z;

        if (deltaZ > _movementThreshold)
        {
            finalTargetZ = targetZ - _movementThreshold;
        }
        else if (deltaZ < -_movementThreshold)
        {
            finalTargetZ = targetZ + _movementThreshold;
        }

        //if (Mathf.Abs(transform.position.z - targetZ) < _movementThreshold) return;

        float smoothZ = Mathf.SmoothDamp(transform.position.z, finalTargetZ, ref _velocity, _smoothDamp);

        transform.position = new Vector3(transform.position.x, transform.position.y, smoothZ);
    }
}
