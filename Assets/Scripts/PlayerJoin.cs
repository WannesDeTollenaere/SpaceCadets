using UnityEngine;

public class PlayerJoin : MonoBehaviour
{
    [SerializeField]
    private Transform _player1Spawn;
    [SerializeField]
    private Transform _player2Spawn;
    [SerializeField]
    private GameObject _playerPrefab1;
    [SerializeField]
    private GameObject _playerPrefab2;

    private GameObject _player1;
    private GameObject _player2;

    private void Start()
    {
        _player1 = Instantiate(_playerPrefab1, _player1Spawn.position, _player1Spawn.rotation);
        _player2 = Instantiate(_playerPrefab2, _player2Spawn.position, _player2Spawn.rotation);
    }
}
