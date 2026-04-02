using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 20.0f;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private float _rotationSpeed = 10.0f;
    [SerializeField] private Animator _animator;

    private PlayerHealth _health;
    private Vector2 _moveInput;
    private bool _isMoving = false;
    private bool _canMove = true;

    private bool _isGrounded = true;

    public UnityEvent OnStartedMoving;
    public UnityEvent OnStoppedMoving;

    private Rigidbody _rb;

    [SerializeField] private float _castRadius = 0.5f;
    [SerializeField] private float _castDistance = 0.6f;
    [SerializeField] private Vector3 _castOffset = new Vector3(0, 0.5f, 0);

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _health = GetComponent<PlayerHealth>();
        if (!_health)
        {
            Debug.LogError("No PlayerHealth component found.");
        }
        else
        {
            _health.OnPlayerDied.AddListener(StopMovement);
        }

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnPlayersRespawned.AddListener(() => _canMove = true);
        }
    }

    private void FixedUpdate()
    {
        if (!_canMove) return;

        CheckGrounded();
        MoveCharacter();
    }

    private void CheckGrounded()
    {
        Vector3 startPos = transform.position + _castOffset;
        _isGrounded = Physics.SphereCast(startPos, _castRadius, Vector3.down, out var hitInfo, _castDistance, _groundLayer);
    }

    private void MoveCharacter()
    {
        Vector3 moveDirection = new Vector3(_moveInput.x, 0.0f, _moveInput.y);

        if (_isGrounded)
        {
            Vector3 targetVelocity = moveDirection * _walkSpeed;
            targetVelocity.y = _rb.linearVelocity.y;
            _rb.linearVelocity = targetVelocity;
        }
        else
        {
            // trust, Aprilvis in je kont
            int i = 67;
        }

        if (moveDirection.sqrMagnitude > float.Epsilon)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);

            if (_animator)
            {
                float smoothSpeed = Mathf.Lerp(0.3f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, moveDirection.magnitude));
                _animator.SetFloat("WalkSpeed", smoothSpeed);
            }
        }
    }

    public void Move(Vector2 input)
    {
        if (!_canMove) return;

        _moveInput = input;

        if (_moveInput.sqrMagnitude > float.Epsilon && !_isMoving)
        {
            _isMoving = true;
            OnStartedMoving?.Invoke();
        }
        else if (_moveInput.sqrMagnitude <= float.Epsilon && _isMoving)
        {
            _isMoving = false;
            OnStoppedMoving?.Invoke();
        }
    }

    private void StopMovement()
    {
        _moveInput = Vector2.zero;
        _rb.linearVelocity = Vector3.zero;
        _canMove = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 startPos = transform.position + _castOffset;

        Gizmos.DrawWireSphere(startPos, _castRadius);

        Vector3 lineStart = startPos + (Vector3.down * _castRadius);
        Vector3 lineEnd = startPos + (Vector3.down * (_castRadius + _castDistance));
        Gizmos.DrawLine(lineStart, lineEnd);

        Vector3 endPosition = startPos + (Vector3.down * _castDistance);
        Gizmos.DrawWireSphere(endPosition, _castRadius);
    }
}