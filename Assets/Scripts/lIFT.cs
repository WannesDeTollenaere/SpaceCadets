using UnityEngine;

public class Lift : MonoBehaviour, ICell
{
    public bool IsActivated { get; private set; }

    [SerializeField]
    private GameObject _ElevatorObject;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Activate();
        }
    }

    public void Activate()
    {
        IsActivated = true;
    }
}