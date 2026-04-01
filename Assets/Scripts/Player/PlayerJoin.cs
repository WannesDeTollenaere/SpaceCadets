using Unity.Cinemachine;
using UnityEngine;

public class PlayerJoin : MonoBehaviour
{
    [SerializeField] private Transform _bigPlayerSpawn;
    [SerializeField] private Transform _smallPlayerSpawn;
    [SerializeField] private GameObject _bigPlayerPrefab;
    [SerializeField] private GameObject _smallPlayerPrefab;

    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private CameraFollow _camera;

    private GameObject _bigPlayer;
    private GameObject _smallPlayer;

    public GameObject BigPlayer => _bigPlayer;
    public GameObject SmallPlayer => _smallPlayer;

    private void Awake()
    {
        _bigPlayer = Instantiate(_bigPlayerPrefab, _bigPlayerSpawn.position, _bigPlayerSpawn.rotation);
        _smallPlayer = Instantiate(_smallPlayerPrefab, _smallPlayerSpawn.position, _smallPlayerSpawn.rotation);

        _camera.Player1 = _bigPlayer;
        _camera.Player2 = _smallPlayer;

        PiggyBack bigPiggy = _bigPlayer.GetComponent<PiggyBack>();
        PiggyBack smallPiggy = _smallPlayer.GetComponent<PiggyBack>();

        _playerManager.BigPlayer = bigPiggy;
        _playerManager.SmallPlayer = smallPiggy;
    }
}