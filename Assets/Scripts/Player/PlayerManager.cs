using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 100.0f;
    [SerializeField]
    private float _timeBuffer = 0.3f;
    [SerializeField]
    private float _duration = 2.0f;

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

    public enum PiggyBackState
    {
        Detached,
        Attaching,
        Attached
    }
    private PiggyBackState _piggyBackState = PiggyBackState.Detached;

    public UnityEvent OnPlayersAttached;
    public UnityEvent OnPlayersDetached;

    private void Start()
    {
        _bigPlayer.OnPlayerPressedPiggyBack.AddListener(AttachPlayers);
        _smallPlayer.OnPlayerPressedPiggyBack.AddListener(AttachPlayers);
    }

    private void OnDestroy()
    {
        _bigPlayer.OnPlayerPressedPiggyBack.RemoveListener(AttachPlayers);
        _smallPlayer.OnPlayerPressedPiggyBack.RemoveListener(AttachPlayers);
    }

    private void Update()
    {
        if (_piggyBackState == PiggyBackState.Attached)
        {
            _smallPlayer.transform.SetPositionAndRotation(_bigPlayer.AttachTransform.position, _bigPlayer.AttachTransform.rotation);
        }
    }

    private void AttachPlayers()
    {
        switch (_piggyBackState)
        {
            case PiggyBackState.Detached:
                StartCoroutine(TimerRoutine());
                break;
            case PiggyBackState.Attaching:
                break;
            case PiggyBackState.Attached:
                DetachPlayers();
                break;
        }
    }

    IEnumerator TimerRoutine()
    {
        float timer = 0f;

        while (timer < _timeBuffer)
        {
            bool allPlayersPressedPiggyBack = true;
            if (!_bigPlayer.PressedPiggyBack || !_smallPlayer.PressedPiggyBack)
            {
                allPlayersPressedPiggyBack = false;
            }

            if (allPlayersPressedPiggyBack)
            {
                StartPiggyBack();

                _piggyBackState = PiggyBackState.Attaching;
                _bigPlayer.PressedPiggyBack = false;
                _smallPlayer.PressedPiggyBack = false;

                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        _bigPlayer.PressedPiggyBack = false;
        _smallPlayer.PressedPiggyBack = false;
    }

    private void StartPiggyBack()
    {
        Vector3 moveDirection = _bigPlayer.transform.position - _smallPlayer.transform.position;
        moveDirection.y = 0.0f;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        _bigPlayer.transform.rotation = Quaternion.Slerp(_bigPlayer.transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        _smallPlayer.transform.rotation = Quaternion.Slerp(_smallPlayer.transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);

        StartCoroutine(PiggyBackRoutine());
    }

    IEnumerator PiggyBackRoutine()
    {
        float elapsedTime = 0.0f;
        Vector3 startPosition = _smallPlayer.transform.position;

        while (elapsedTime < _duration)
        {
            float t = elapsedTime / _duration;
            
            _smallPlayer.transform.position = Vector3.Lerp(startPosition, _bigPlayer.AttachTransform.position, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        };

        _smallPlayer.transform.position = _bigPlayer.AttachTransform.position;

        _smallPlayer.transform.SetParent(_bigPlayer.transform);
        var _smallPlayerRB = _smallPlayer.GetComponent<Rigidbody>();
        _smallPlayerRB.useGravity = false;

        _piggyBackState = PiggyBackState.Attached;
        OnPlayersAttached?.Invoke();
    }

    private void DetachPlayers()
    {
        Debug.Log("Throw small guy");

        _smallPlayer.transform.SetParent(null);

        var _smallPlayerRB = _smallPlayer.GetComponent<Rigidbody>();
        _smallPlayerRB.useGravity = true;

        _piggyBackState = PiggyBackState.Detached;
        OnPlayersDetached?.Invoke();
    }
}

