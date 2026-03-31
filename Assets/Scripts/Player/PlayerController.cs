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
    private InputAction _scanAction;
    private InputAction _piggyBackAction;


    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _movementComp = GetComponent<PlayerMovement>();
        _piggyBackComp = GetComponent<PiggyBack>();


        _moveAction = _playerInput.actions["Move"];

        _lookAction = _playerInput.actions["Look"];

        _scanAction = _playerInput.actions["Scan"];
        _scanAction.performed += OnScan;

        _piggyBackAction = _playerInput.actions["PiggyBack"];
        _piggyBackAction.performed += OnPiggyBack;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_movementComp != null)
        {
            var moveDirection = _moveAction.ReadValue<Vector2>();

            _movementComp.Move(moveDirection);
        }

        
        if (_scannerComp != null)
        {
            var lookDirection = _lookAction.ReadValue<Vector2>();

            _scannerComp.Look(lookDirection);
        }
    }
    
    private void OnPiggyBack(InputAction.CallbackContext context)
    {
        _piggyBackComp.PressPiggyBack();
    }
    private void OnScan(InputAction.CallbackContext context)
    {
        if (_scannerComp == null) return;

        if (context.ReadValueAsButton())
        {
            _scannerComp.Activate();
        }
        else
        {
            _scannerComp.Deactivate();
        }
    }
}
