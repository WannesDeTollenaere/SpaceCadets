using System.Runtime.CompilerServices;
using UnityEngine;

public class Lift : MonoBehaviour
{

    [SerializeField]
    private Animator _ElevatorAnim;

    public void Activate()
    {
        _ElevatorAnim.SetBool("IsElevated", true);
    }

    public void Deactivate()
    {
        _ElevatorAnim.SetBool("IsElevated", false);
    }
}