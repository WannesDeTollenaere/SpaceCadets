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
    private float _attachDuration = 2.0f;
    [SerializeField]
    private float _distanceThreshold = 1.0f;
    [SerializeField]
    private GameObject _playersInRangeIndicator;
    [SerializeField]
    private float _rangeIndicatorHeight = 1.0f;

    private PiggyBack _bigPlayer;
    public PiggyBack BigPlayer
    {
        get { return _bigPlayer; }
        set { _bigPlayer = value; }
    }
    private PiggyBack _smallPlayer;
    public PiggyBack SmallPlayer
    {
        get { return _smallPlayer; }
        set { _smallPlayer = value; }
    }

    private Scanner _scanner;

    public enum PiggyBackState
    {
        Detached,
        Attaching,
        Attached
    }

    private PiggyBackState _piggyBackState = PiggyBackState.Detached;

    private bool _arePlayersInRange = false;
    private bool _isAttachCooldownActive = false;

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

        if (ArePlayersInRange() && !_isAttachCooldownActive)
        {
            _arePlayersInRange = true;
        }
        else
        {
            _arePlayersInRange = false;
        }

        if (_playersInRangeIndicator)
        {
            if (_arePlayersInRange && _piggyBackState == PiggyBackState.Detached)
            {
                var playersPositionCenter = (_bigPlayer.transform.position + _smallPlayer.transform.position) / 2.0f;
                _playersInRangeIndicator.transform.position = new Vector3(
                    playersPositionCenter.x,
                    playersPositionCenter.y + _rangeIndicatorHeight,
                    playersPositionCenter.z
                );

                _playersInRangeIndicator.SetActive(true);
            }
            else
            {
                _playersInRangeIndicator.SetActive(false);
            }
        }
    }

    private void AttachPlayers()
    {
        switch (_piggyBackState)
        {
            case PiggyBackState.Detached:
                if (_arePlayersInRange)
                {
                    StartCoroutine(TimerRoutine());
                }
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

        while (elapsedTime < _attachDuration)
        {
            float t = elapsedTime / _attachDuration;

            _smallPlayer.transform.position = Vector3.Lerp(startPosition, _bigPlayer.AttachTransform.position, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ;

        _smallPlayer.transform.position = _bigPlayer.AttachTransform.position;

        _smallPlayer.transform.SetParent(_bigPlayer.transform);
        var _smallPlayerRB = _smallPlayer.GetComponent<Rigidbody>();
        _smallPlayerRB.useGravity = false;

        _piggyBackState = PiggyBackState.Attached;
        
        _smallPlayer.gameObject.GetComponentInChildren<Scanner>().Toggle();

        OnPlayersAttached?.Invoke();
    }

    private void DetachPlayers()
    {
        _smallPlayer.gameObject.GetComponentInChildren<Scanner>().Toggle();

        _smallPlayer.transform.SetParent(null);

        var _smallPlayerRB = _smallPlayer.GetComponent<Rigidbody>();
        _smallPlayerRB.useGravity = true;

        _smallPlayer.Launch();

        _isAttachCooldownActive = true;
        _piggyBackState = PiggyBackState.Detached;
        OnPlayersDetached?.Invoke();


        StartCoroutine(AttachCooldownRoutine());
    }

    public bool ArePlayersInRange()
    {
        float sqrDistance = Vector3.SqrMagnitude(_bigPlayer.transform.position - _smallPlayer.transform.position);

        if (sqrDistance < (_distanceThreshold * _distanceThreshold))
        {
            return true;
        }

        return false;
    }

    IEnumerator AttachCooldownRoutine()
    {
        const float attachCooldown = 1.0f;
        float timer = 0f;

        while (timer < attachCooldown)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        _isAttachCooldownActive = false;
    }
}

