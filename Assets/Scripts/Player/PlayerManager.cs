using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 100.0f;

    private PiggyBack _bigPlayer;
    public PiggyBack BigPlayer
    {
        get { return _bigPlayer; }
        set {  _bigPlayer = value; }
    }
    private PiggyBack _smallPlayer;
    public PiggyBack SmallPlayer
    {
        get { return _smallPlayer; }
        set { _smallPlayer = value; }
    }

    private void Start()
    {
        _bigPlayer.OnPlayerPressedPiggyBack.AddListener(StartPiggyBack);
        _smallPlayer.OnPlayerPressedPiggyBack.AddListener(StartPiggyBack);
    }

    private void OnDestroy()
    {
        _bigPlayer.OnPlayerPressedPiggyBack.RemoveListener(StartPiggyBack);
        _smallPlayer.OnPlayerPressedPiggyBack.RemoveListener(StartPiggyBack);
    }

    private void StartPiggyBack()
    {
        Vector3 moveDirection = _bigPlayer.transform.position - _smallPlayer.transform.position;
        moveDirection.y = 0.0f;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        _bigPlayer.transform.rotation = Quaternion.Slerp(_bigPlayer.transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        _smallPlayer.transform.rotation = Quaternion.Slerp(_smallPlayer.transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
    }
}

