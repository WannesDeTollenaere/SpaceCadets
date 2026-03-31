using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Scanner _scannerComp;


    private PlayerInput _playerInput;

    private PlayerMovement _movementComp;
    private PiggyBack _piggyBackComp;

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _piggyBackAction;


    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _movementComp = GetComponent<PlayerMovement>();
        //_scannerComp = GetComponentInChildren<Scanner>();
        _piggyBackComp = GetComponent<PiggyBack>();


        _moveAction = _playerInput.actions["Move"];

        _lookAction = _playerInput.actions["Look"];

        _piggyBackAction = _playerInput.actions["PiggyBack"];
        _piggyBackAction.performed += OnPiggyBack;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var moveDirection = _moveAction.ReadValue<Vector2>();

        _movementComp.Move(moveDirection);

        var lookDirection = _lookAction.ReadValue<Vector2>();

        _scannerComp.Look(lookDirection);
    }
    
    private void OnPiggyBack(InputAction.CallbackContext context)
    {
        //_piggyBackComp.Jump();
    }
}
