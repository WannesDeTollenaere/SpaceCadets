using UnityEngine;

public class SmallPlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    private PlayerMovement _playerMovement;
    private PlayerHealth _playerHealth;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        if (!_playerMovement)
        {
            Debug.Log("No PlayerMovement found");
        }
        _playerMovement.OnStartedMoving.AddListener(() => _animator.SetBool("IsWalking", true));
        _playerMovement.OnStoppedMoving.AddListener(() => _animator.SetBool("IsWalking", false));

        _playerHealth = GetComponent<PlayerHealth>();
        if (!_playerMovement)
        {
            Debug.Log("No PlayerHealth found");
        }
        _playerHealth.OnPlayerDied.AddListener(() => _animator.SetBool("IsDead", true));
        //_playerHealth.OnPlayerRespawned.AddListener(() => _animator.SetBool("IsDead", false));
        PlayerManager.Instance.OnPlayersRespawned.AddListener(() => _animator.SetBool("IsDead", false));
    }
}
