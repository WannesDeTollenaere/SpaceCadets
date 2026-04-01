using UnityEngine;

public class Oxygen : MonoBehaviour
{
    [SerializeField] private float _drainRate = 0.3f;

    private PlayerManager _playermanager;
    private PlayerJoin _playerJoin;

    private float _currentMeter = 1f;
    private float _maxMeter = 1f;

    public float CurrentOxygen => _currentMeter;
    public float MaxOxygen => _maxMeter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playermanager = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playermanager.ArePlayersInRange())
        {
            _currentMeter = Mathf.Max(0f, _currentMeter - _drainRate * Time.fixedDeltaTime);
        }
        else
        {
            _currentMeter = _maxMeter;
        }

        if (_currentMeter <= 0)
        {
            Destroy(_playerJoin.SmallPlayer);
        }
    }
}
