using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5.0f;
    [SerializeField]
    private float _rotationSpeed = 5.0f;

    private CharacterController _characterController;
    private Vector2 _moveInput;
    private bool _isMoving = false;
    private float _verticalSpeed = 0.0f;

    public UnityEvent OnStartedMoving;
    public UnityEvent OnStoppedMoving;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        if (!_characterController)
        {
            Debug.LogError("No CharacterController found on GameObject");
        }
    }

    private void FixedUpdate()
    {
        MoveCharacter();
        ApplyGravity();
    }

    private void MoveCharacter()
    {
        if (_moveInput.x * _moveInput.x < float.Epsilon)
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

        _characterController.Move(moveVelocity * Time.fixedDeltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
    }

    private void ApplyGravity()
    {
        const float GRAVITY = -18.0f;

        _verticalSpeed += GRAVITY * Time.fixedDeltaTime;
        _characterController.Move(_verticalSpeed * Vector3.up * Time.fixedDeltaTime);

        if (_characterController.isGrounded)
        {
            _verticalSpeed = 0.0f;
        }
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();

        if (!_isMoving)
        {
            _isMoving = true;
            OnStartedMoving?.Invoke();
        }
    }
}
