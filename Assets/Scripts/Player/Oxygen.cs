using UnityEngine;

public class Oxygen : MonoBehaviour
{
    [SerializeField] private float _drainRate = 0.3f;
    [SerializeField] private PlayerJoin _playerJoin;
    [SerializeField] private Material _normalMaterial;
    [SerializeField] private Material _pulsingMaterial;

    private PlayerManager _playermanager;

    private float _currentMeter = 1f;
    private float _maxMeter = 1f;

    public float CurrentOxygen => _currentMeter;
    public float MaxOxygen => _maxMeter;

    private Renderer _renderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playermanager = GetComponent<PlayerManager>();
        _renderer = _playerJoin.SmallPlayer.GetComponentInChildren<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!_playermanager.ArePlayersInRange())
        {
            _currentMeter = Mathf.Max(0f, _currentMeter - _drainRate * Time.fixedDeltaTime);

            if (_renderer.material != _pulsingMaterial)
            {
                _renderer.material = _pulsingMaterial;
            }
        }
        else
        {
            _currentMeter = _maxMeter;

            if (_renderer.material != _normalMaterial)
            {
                _renderer.material = _normalMaterial;
            }

            if (_currentMeter <= 0)
            {
                if (_playerJoin.SmallPlayer)
                {
                    Destroy(_playerJoin.SmallPlayer);
                }
            }
        }
    }
}
