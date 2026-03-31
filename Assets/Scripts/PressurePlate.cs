using UnityEngine;
using UnityEngine.Events; 

public class PressurePlate : MonoBehaviour
{

    [SerializeField] 
    private string _triggerTag = "Player";

    public UnityEvent OnPressed;
    public UnityEvent OnReleased;

    private int _objectsOnPlate = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_triggerTag))
        {
            _objectsOnPlate++;

            if (_objectsOnPlate == 1)
            {
                OnPressed?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_triggerTag))
        {
            _objectsOnPlate--;

            if (_objectsOnPlate <= 0)
            {
                _objectsOnPlate = 0; 
                OnReleased?.Invoke();
            }
        }
    }
}