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
    [SerializeField]private AudioSource m_loopAudioSource;
    [SerializeField] private AudioSource m_oneShotAudioSource;
    [SerializeField] private AudioSource m_footstepAudioSource;
    [SerializeField] private bool m_isBigG;




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
        if (!m_isBigG)
        {
            m_lilGuyMLA.PlayContainerElement(m_oneShotAudioSource, LilGuyElements.Attach);
        }

    }

    public void Launch()
    {
        Vector3 launchDirection = transform.forward;
        launchDirection.y = _launchUpFactor;
        launchDirection.Normalize();

        _rigidBody.AddForce(_launchForce * launchDirection);
        //Start Sound Detach 
        if (!m_isBigG)
        {
            m_lilGuyMLA.PlayContainerElement(m_oneShotAudioSource, LilGuyElements.Detach);
        }
    }
}
