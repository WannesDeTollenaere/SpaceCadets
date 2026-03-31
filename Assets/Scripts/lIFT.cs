using System.Runtime.CompilerServices;
using UnityEngine;

public class Lift : MonoBehaviour, ICell
{
    public bool IsActivated { get; private set; }

    [SerializeField]
    private Animator _ElevatorAnim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Activate();
        }

    }

    public void Activate()
    {
        _ElevatorAnim.SetBool("IsElevated", true);
    }
}