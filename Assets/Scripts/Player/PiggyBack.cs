using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PiggyBack : MonoBehaviour
{
    [SerializeField]
    private Transform _attachTransform;
    public Transform AttachTransform
    {
        get { return _attachTransform; }
    }

    private bool _pressedPiggyBack = false;
    public bool PressedPiggyBack
    {
        get { return _pressedPiggyBack; }
        set { _pressedPiggyBack = value; }
    }

    public UnityEvent OnPlayerPressedPiggyBack;
    
    public void PressPiggyBack()
    {
        _pressedPiggyBack = true;
        OnPlayerPressedPiggyBack?.Invoke();
    }
}
