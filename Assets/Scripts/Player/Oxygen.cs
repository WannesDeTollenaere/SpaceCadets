using System.Collections.Generic;
using UnityEngine;

public class Oxygen : MonoBehaviour
{
    [SerializeField] private float _drainRate = 0.3f;
    [SerializeField] private PlayerJoin _playerJoin;
    [SerializeField] private Material _pulsingMaterial;

    private PlayerManager _playermanager;
    private float _currentMeter = 1f;
    private float _maxMeter = 1f;

    public float CurrentOxygen => _currentMeter;
    public float MaxOxygen => _maxMeter;

    [SerializeField] private Renderer _renderer;

    // Cache original materials to restore them accurately
    private Material[] _originalMaterials;
    private bool _isPulsing = false;

    void Start()
    {
        _playermanager = GetComponent<PlayerManager>();

        if (_playerJoin?.SmallPlayer != null)
        {
            _renderer = _playerJoin.SmallPlayer.GetComponentInChildren<Renderer>();
            if (_renderer != null)
            {
                // Store the original material array
                _originalMaterials = _renderer.sharedMaterials;
            }
        }

        if (_renderer == null) Debug.LogError("FAILED TO FIND SMALL PLAYER RENDERER");
    }

    void Update()
    {
        if (PlayerManager.Instance.IsRespawning) return;

        if (!_playermanager.ArePlayersInRange())
        {
            _currentMeter = Mathf.Max(0f, _currentMeter - _drainRate * Time.deltaTime);

            if (!_isPulsing)
            {
                ApplyPulsingEffect(true);
            }

            if (_currentMeter <= 0)
            {
                HandleDeath();
            }
        }
        else
        {
            _currentMeter = _maxMeter;

            if (_isPulsing)
            {
                ApplyPulsingEffect(false);
            }
        }
    }

    private void ApplyPulsingEffect(bool shouldPulse)
    {
        if (_renderer == null || _pulsingMaterial == null) return;

        _isPulsing = shouldPulse;

        if (shouldPulse)
        {
            // Create an array where EVERY slot is the pulsing material
            // This ensures the whole body pulses regardless of sub-meshes
            Material[] pulsingArray = new Material[_originalMaterials.Length];
            for (int i = 0; i < pulsingArray.Length; i++)
            {
                pulsingArray[i] = _pulsingMaterial;
            }
            _renderer.materials = pulsingArray;
        }
        else
        {
            // Restore the original multi-material setup
            _renderer.materials = _originalMaterials;
        }
    }

    private void HandleDeath()
    {
        if (_playerJoin.SmallPlayer)
        {
            PlayerHealth health = _playerJoin.SmallPlayer.GetComponent<PlayerHealth>();
            health?.Die();
        }
        _currentMeter = _maxMeter;
    }
}