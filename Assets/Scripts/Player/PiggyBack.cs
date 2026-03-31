using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PiggyBack : MonoBehaviour
{
    private bool _pressedPiggyBack = false;
    public bool PressedPiggyBack
    {
        get { return _pressedPiggyBack; }
        set { _pressedPiggyBack = value; }
    }

    public UnityEvent OnPlayerPressedPiggyBack;
    
    public void OnJump(InputValue button)
    {
        _pressedPiggyBack = true;
        OnPlayerPressedPiggyBack?.Invoke();
    }
}
