using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PiggyBack : MonoBehaviour
{
    public UnityEvent OnPlayerPressedPiggyBack;
    
    public void OnJump(InputValue button)
    {
        OnPlayerPressedPiggyBack?.Invoke();
    }
}
