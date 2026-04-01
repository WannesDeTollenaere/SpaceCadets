using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class PlayerMovement : MonoBehaviour
{
    private float _moveSpeed = 20.0f;
    [SerializeField] private float _walkSpeed = 20.0f;
    [SerializeField] private float _airSpeed = 5.0f;

    [SerializeField]
    private float _rotationSpeed = 5.0f;

    private PlayerHealth _health;
    private CharacterController _characterController;
    private Vector2 _moveInput;
    private bool _isMoving = false;
    //private float _verticalSpeed = 0.0f;
    private bool _canMove = true;

    public UnityEvent OnStartedMoving;
    public UnityEvent OnStoppedMoving;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _health = GetComponent<PlayerHealth>();
        if (!_health)
        {
            Debug.LogError("No PlayerHealth component found.");
        }
        _health.OnPlayerDied.AddListener(StopMovement);
        _health.OnPlayerRespawned.AddListener(() => _canMove = true);
    }

    private void FixedUpdate()
    {
        MoveCharacter();
        //ApplyGravity();
    }

    private void MoveCharacter()
    {
        if (_moveInput.sqrMagnitude < float.Epsilon)
        {
            if (_isMoving)
            {
                _isMoving = false;

                OnStoppedMoving?.Invoke();
            }
            return;
        }

        Vector3 moveDirection = new Vector3(_moveInput.x, 0.0f, _moveInput.y);
        Vector3 moveVelocity = moveDirection * _moveSpeed;

        //_characterController.Move(moveVelocity * Time.fixedDeltaTime);
        _rb.AddForce(moveVelocity);
        

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        _rb.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
    }

    //private void ApplyGravity()
    //{
    //    const float GRAVITY = -18.0f;

    //    _verticalSpeed += GRAVITY * Time.fixedDeltaTime;
    //    _characterController.Move(_verticalSpeed * Vector3.up * Time.fixedDeltaTime);

    //    if (_characterController.isGrounded)
    //    {
    //        _verticalSpeed = 0.0f;
    //    }
    //}

    public void Move(Vector2 input)
    {
        if (!_canMove) return;

        _moveInput = input;

        if (_moveInput.sqrMagnitude > float.Epsilon && !_isMoving)
        {
            _isMoving = true;
            OnStartedMoving?.Invoke();
        }


        if (Physics.SphereCast(_rb.position - 2f * Vector3.down, 5.0f, Vector3.down, out var hitInfo))
        {
            _moveSpeed = _walkSpeed;
        }
        else
        {
            _moveSpeed = _airSpeed;
        }
    }

    private void StopMovement()
    {
        _moveInput = Vector2.zero;

        _canMove = false;
    }
}
