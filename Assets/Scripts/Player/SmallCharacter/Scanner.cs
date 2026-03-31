using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Scanner : MonoBehaviour
{
    [SerializeField]
    private Transform _satellitePivot;
    //[SerializeField]
    //private float _rotationSpeed = 5f;

    //private CharacterController _characterController;
    private Vector2 _lookInput;


    void Awake()
    {
    }

    void FixedUpdate()
    {
        if (_lookInput.x * _lookInput.x < float.Epsilon) return;

        _satellitePivot.rotation = Quaternion.identity;

        Vector3 lookDirection = new Vector3(_lookInput.x, 0.0f, _lookInput.y);

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        _satellitePivot.rotation = targetRotation;
    }

    public void Look(Vector2 input)
    {
        _lookInput = input;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Cell>()?.Reveal();
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<Cell>()?.Hide();
    }

    public void Activate()
    {

    }
}
