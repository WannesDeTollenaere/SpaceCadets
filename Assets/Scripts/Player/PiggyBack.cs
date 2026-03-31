using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PiggyBack : MonoBehaviour
{
    [SerializeField]
    private Transform _attachTransform;
    [SerializeField]
    private float _launchForce = 10.0f;
    [SerializeField]
    private float _launchUpFactor = 0.5f;

    public Transform AttachTransform
    {
        get { return _attachTransform; }
    }

    private Rigidbody _rigidBody;

    private bool _pressedPiggyBack = false;
    public bool PressedPiggyBack
    {
        get { return _pressedPiggyBack; }
        set { _pressedPiggyBack = value; }
    }

    public UnityEvent OnPlayerPressedPiggyBack;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void PressPiggyBack()
    {
        _pressedPiggyBack = true;
        OnPlayerPressedPiggyBack?.Invoke();
    }

    public void Launch()
    {
        Vector3 launchDirection = transform.forward;
        launchDirection.y = _launchUpFactor;
        launchDirection.Normalize();

        _rigidBody.AddForce(_launchForce * launchDirection);
    }
}
