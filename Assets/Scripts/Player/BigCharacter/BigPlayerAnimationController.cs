using UnityEngine;

public class BigPlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    private PlayerMovement _playerMovement;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        if (!_playerMovement)
        {
            Debug.Log("No PlayerMovement found. Animations will not work");
        }
        _playerMovement.OnStartedMoving.AddListener(() => _animator.SetBool("IsWalking", true));
        _playerMovement.OnStoppedMoving.AddListener(() => _animator.SetBool("IsWalking", false));
    }
}
