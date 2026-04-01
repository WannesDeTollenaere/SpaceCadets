using SpaceCadets.Audio;
using System;
using System.Runtime.CompilerServices;
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

    [SerializeField] private MultiLayerAudioLilGuy m_lilGuyMLA;
    [SerializeField] private AudioSource m_oneShotSource;
    [SerializeField] private AudioSource m_loopSource;



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
        

        Cell otherCell = other.GetComponent<Cell>();

        if (otherCell)
        {
            //Show Bomb SFX

            if (otherCell.Reveal())
                m_lilGuyMLA.PlayContainerElement(m_oneShotSource, LilGuyElements.BombShown);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Cell>())
        {
            other.GetComponent<Cell>().Hide();
        }
    }

    public void Activate()
    {
        _isActive = true;

        //startsound
        m_lilGuyMLA.PlayContainerElement(m_oneShotSource, LilGuyElements.ScanStart);
        m_lilGuyMLA.PlayContainerElement(m_loopSource, LilGuyElements.ScanLoop, true, this);
    }

    public void Deactivate()
    {
        _isActive = false;

        // End Sound
        m_lilGuyMLA.PlayContainerElement(m_oneShotSource, LilGuyElements.ScanEnd);
        m_lilGuyMLA.FadeOutAndStop(m_loopSource, this);
    }
}
