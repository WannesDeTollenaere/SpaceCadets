using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Scanner : MonoBehaviour
{
    [SerializeField]
    private Transform _satellitePivot;

    [SerializeField]
    private Vector3 _raderScale;

    private Vector2 _lookInput;

    private bool _isActive = false;

    private Collider _scanCollider;
    private Renderer _scanRenderer;


    void Awake()
    {
        _scanCollider = GetComponentInChildren<Collider>();
        _scanCollider.enabled = false;

        _scanRenderer = GetComponentInChildren<Renderer>();
        _scanRenderer.enabled = false;
    }

    private void Update()
    {
        if (_isActive)
        {

            transform.localScale = _raderScale;
            _scanCollider.enabled = true;
            _scanRenderer.enabled = true;
        }
        else
        {
            gameObject.transform.localScale = Vector3.zero;
            _scanRenderer.enabled = false;
        }
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
        _isActive = true;
    }
    public void Deactivate()
    {
        _isActive = false;
    }
}
