using SpaceCadets.Audio;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PiggyBack : MonoBehaviour
{
    [SerializeField]
    private Transform _attachTransform;
    [SerializeField]
    private float _launchForce = 10.0f;
    [SerializeField]
    private float _launchUpFactor = 0.5f;
    [SerializeField]
    private MultiLayerAudioLilGuy m_lilGuyMLA;
    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public Transform AttachTransform
    {
        get { return _attachTransform; }
    }

    private Rigidbody _rigidBody;

    private bool _pressedPiggyBack = false;
    public bool PressedPiggyBack
    {
        get { return _pressedPiggyBack; }
        set { _pressedPiggyBack = value; }
    }

    public UnityEvent OnPlayerPressedPiggyBack;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void PressPiggyBack()
    {
        _pressedPiggyBack = true;
        OnPlayerPressedPiggyBack?.Invoke();
        //Start Sound Attach
        m_lilGuyMLA.PlayContainerElement(m_audioSource, LilGuyElements.Attach);
    }

    public void Launch()
    {
        Vector3 launchDirection = transform.forward;
        launchDirection.y = _launchUpFactor;
        launchDirection.Normalize();

        _rigidBody.AddForce(_launchForce * launchDirection);
        //Start Sound Detach 
        m_lilGuyMLA.PlayContainerElement(m_audioSource, LilGuyElements.Detach);
    }
}
