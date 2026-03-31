using System.Runtime.CompilerServices;
using UnityEngine;

public class Lift : MonoBehaviour
{

    [SerializeField]
    private Transform _ElevatorAnim;
    [SerializeField]
    private float _ExtendSpeed;

    private bool _IsExtended;

    public void Activate()
    {
        _IsExtended = true;
    }

    public void Deactivate()
    {
        _IsExtended = false;
    }

    private void Update()
    {
        if (_ElevatorAnim == null) return;

        if (_IsExtended)
        {
            Debug.Log("ae;ioja;difoj");
            _ElevatorAnim.localPosition = Vector3.Slerp(_ElevatorAnim.localPosition, Vector3.up, _ExtendSpeed);
        } else
        {
            _ElevatorAnim.localPosition = Vector3.Slerp(_ElevatorAnim.localPosition, Vector3.zero, _ExtendSpeed);
        }
    }
}