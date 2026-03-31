using System.Runtime.CompilerServices;
using UnityEngine;

public class Lift : MonoBehaviour, ICell
{
    public bool IsActivated { get; private set; }

    [SerializeField]
    private Animator _ElevatorAnim;


    public void Activate()
    {
        Debug.Log("Kaas");

        _ElevatorAnim.SetBool("IsElevated", true);
    }

    public void Deactivate()
    {
        _ElevatorAnim.SetBool("IsElevated", false);
    }
}