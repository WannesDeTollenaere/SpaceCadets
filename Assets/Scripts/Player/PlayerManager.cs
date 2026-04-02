using SpaceCadets.Audio;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField] private int _totalLives = 3;
    [SerializeField] private string _gameOverSceneName = "GameOver";
    private Vector3 _currentCheckpoint;

    [SerializeField] private float _rotationSpeed = 100.0f;
    [SerializeField] private float _timeBuffer = 0.3f;
    [SerializeField] private float _attachDuration = 2.0f;
    [SerializeField] private float _distanceThreshold = 1.0f;
    [SerializeField] private PiggyBackInputHint _piggyBackInput;
    [SerializeField] private float _rangeIndicatorHeight = 1.0f;
    [SerializeField] private GameObject _respawnVFX;
    [SerializeField] private float _respawnTime = 2.0f;

    private bool _isShuttingDown = false;

    public UnityEvent<int> OnHealthChanged;

    public int TotalLives => _totalLives;

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
    private bool _isRespawning = false;
    public bool IsRespawning
    {
        get { return _isRespawning; }
    }

    public UnityEvent OnPlayersAttached;
    public UnityEvent OnPlayersDetached;
    public UnityEvent OnPlayersRespawned;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _bigPlayer.OnPlayerPressedPiggyBack.AddListener(AttachPlayers);
        _smallPlayer.OnPlayerPressedPiggyBack.AddListener(AttachPlayers);


        if (_bigPlayer != null)
        {
            _currentCheckpoint = _bigPlayer.transform.position;
        }
    }

    private void OnDestroy()
    {
        if (_bigPlayer != null) _bigPlayer.OnPlayerPressedPiggyBack.RemoveListener(AttachPlayers);
        if (_smallPlayer != null) _smallPlayer.OnPlayerPressedPiggyBack.RemoveListener(AttachPlayers);
    }

    private void Update()
    {
        if (_piggyBackState == PiggyBackState.Detached)
        {

        }


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

        if (_piggyBackInput)
        {
            if (_arePlayersInRange && _piggyBackState == PiggyBackState.Detached)
            {
                var playersPositionCenter = (_bigPlayer.transform.position + _smallPlayer.transform.position) / 2.0f;
                _piggyBackInput.SetPosition(new Vector3(
                    playersPositionCenter.x,
                    playersPositionCenter.y + _rangeIndicatorHeight,
                    playersPositionCenter.z
                    )
                );

                _piggyBackInput.Show();
            }
            else
            {
                _piggyBackInput.Hide();
            }
        }
    }


    public void UpdateCheckpoint(Vector3 newCheckpointPosition)
    {
        _currentCheckpoint = newCheckpointPosition;
    }

    public void PlayerDied()
    {
        if (_isShuttingDown) return;
        _totalLives--;

        _isRespawning = true;

        OnHealthChanged?.Invoke(_totalLives);

        StartCoroutine(RestartRoutine());
    }

    IEnumerator RestartRoutine()
    {
        yield return new WaitForSeconds(_respawnTime);

        if (_totalLives <= 0)
        {
            SceneManager.LoadScene(_gameOverSceneName);
        }
        else
        {
            RespawnPlayers();
            OnPlayersRespawned?.Invoke();
        }

        _isRespawning = false;
    }

    private void RespawnPlayers()
    {
        ForceDetachForRespawn();
        AudioEvents.PlayerRespawn();
        var rbBig = _bigPlayer.GetComponent<Rigidbody>();
        var rbSmall = _smallPlayer.GetComponent<Rigidbody>();

        Vector3 bigRespawnPos = _currentCheckpoint + new Vector3(-0.2f, 0, 0);
        Vector3 smallRespawnPos = _currentCheckpoint + new Vector3(0.2f, 0, 0);

        _bigPlayer.transform.position = bigRespawnPos;
        rbBig.position = bigRespawnPos;

        _smallPlayer.transform.position = smallRespawnPos;
        rbSmall.position = smallRespawnPos;


        if (_respawnVFX)
        {
            Instantiate(_respawnVFX, _bigPlayer.transform);
            Instantiate(_respawnVFX, _smallPlayer.transform);
            
        }

        ResetRigidbody(rbBig);
        ResetRigidbody(rbSmall);
    }

    private void ForceDetachForRespawn()
    {
        if (_piggyBackState != PiggyBackState.Detached)
        {

            _smallPlayer.gameObject.GetComponentInChildren<Scanner>().Deactivate();
            _smallPlayer.transform.SetParent(null);

            var _smallPlayerRB = _smallPlayer.GetComponent<Rigidbody>();
            _smallPlayerRB.useGravity = true;

            _isAttachCooldownActive = true;
            _piggyBackState = PiggyBackState.Detached;
            OnPlayersDetached?.Invoke();

            var rbSmall = _smallPlayer.GetComponentsInChildren<Collider>();
            foreach (var rb in rbSmall)
            {
                rb.enabled = true;
            }


            StartCoroutine(AttachCooldownRoutine());
        }
    }

    private void ResetRigidbody(Rigidbody rb)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void SetNewPlayers(PiggyBack newBig, PiggyBack newSmall)
    {
        if (_bigPlayer != null) _bigPlayer.OnPlayerPressedPiggyBack.RemoveListener(AttachPlayers);
        if (_smallPlayer != null) _smallPlayer.OnPlayerPressedPiggyBack.RemoveListener(AttachPlayers);

        _bigPlayer = newBig;
        _smallPlayer = newSmall;

        _bigPlayer.OnPlayerPressedPiggyBack.AddListener(AttachPlayers);
        _smallPlayer.OnPlayerPressedPiggyBack.AddListener(AttachPlayers);

        _piggyBackState = PiggyBackState.Detached;
        _isAttachCooldownActive = false;
    }


    private void AttachPlayers()
    {
        switch (_piggyBackState)
        {
            case PiggyBackState.Detached:
                if (_arePlayersInRange)
                {
                    if (_bigPlayer.PressedPiggyBack)
                    {
                        StartPiggyBack();
                        _bigPlayer.PressedPiggyBack = false;
                        _smallPlayer.PressedPiggyBack = false;
                    }
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

        var rbSmall = _smallPlayer.GetComponentsInChildren<Collider>();
        foreach (var rb in rbSmall)
        {
            rb.enabled = false;
        }
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

        _smallPlayer.transform.position = _bigPlayer.AttachTransform.position;
        _smallPlayer.transform.SetParent(_bigPlayer.transform);

        var _smallPlayerRB = _smallPlayer.GetComponent<Rigidbody>();
        _smallPlayerRB.useGravity = false;

        _piggyBackState = PiggyBackState.Attached;

        _smallPlayer.gameObject.GetComponentInChildren<Scanner>().Activate();

        OnPlayersAttached?.Invoke();
    }
    private void OnApplicationQuit()
    {
        _isShuttingDown = true;
    }

    private void OnDisable()
    {
        _isShuttingDown = true;
    }
    private void DetachPlayers()
    {


        _smallPlayer.gameObject.GetComponentInChildren<Scanner>().Deactivate();
        _smallPlayer.transform.SetParent(null);

        var _smallPlayerRB = _smallPlayer.GetComponent<Rigidbody>();
        _smallPlayerRB.useGravity = true;

        _smallPlayer.Launch();

        _isAttachCooldownActive = true;
        _piggyBackState = PiggyBackState.Detached;
        OnPlayersDetached?.Invoke();

        var rbSmall = _smallPlayer.GetComponentsInChildren<Collider>();
        foreach (var rb in rbSmall)
        {
            rb.enabled = true;
        }


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