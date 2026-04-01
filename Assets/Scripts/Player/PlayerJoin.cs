using Unity.Cinemachine;
using UnityEngine;

public class PlayerJoin : MonoBehaviour
{
    [SerializeField]
    private Transform _bigPlayerSpawn;
    [SerializeField]
    private Transform _smallPlayerSpawn;
    [SerializeField]
    private GameObject _bigPlayerPrefab;
    [SerializeField]
    private GameObject _smallPlayerPrefab;

    [SerializeField]
    private PlayerManager _playerManager;

    [SerializeField]
    private CameraFollow _camera;

    private GameObject _bigPlayer;
    private GameObject _smallPlayer;

    private PiggyBack _bigPlayerPiggyBack;
    private PiggyBack _smallPlayerPiggyBack;

    private void Awake()
    {
        _bigPlayer = Instantiate(_bigPlayerPrefab, _bigPlayerSpawn.position, _bigPlayerSpawn.rotation);
        _smallPlayer = Instantiate(_smallPlayerPrefab, _smallPlayerSpawn.position, _smallPlayerSpawn.rotation);

        _camera.Player1 = _bigPlayer;
        _camera.Player2 = _smallPlayer;

        _bigPlayerPiggyBack = _bigPlayer.GetComponent<PiggyBack>();
        _smallPlayerPiggyBack = _smallPlayer.GetComponent<PiggyBack>();

        _playerManager.BigPlayer = _bigPlayerPiggyBack;
        _playerManager.SmallPlayer = _smallPlayerPiggyBack;
    }
}
