using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5.0f;

    private CharacterController _characterController;
    private Vector2 _moveInput;
    private bool _isMoving = false;

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
        if (transform.parent != null)
        {
            if (_isMoving)
            {
                _isMoving = false;

                OnStoppedMoving?.Invoke();
            }

            return;
        }

        MoveCharacter();
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

        Vector3 moveVelocity = _moveInput * _moveSpeed;

        _characterController.Move(moveVelocity * Time.fixedDeltaTime);
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
