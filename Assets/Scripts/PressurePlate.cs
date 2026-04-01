using SpaceCadets.Audio;
using UnityEngine;
using UnityEngine.Events; 

public class PressurePlate : MonoBehaviour
{

    [SerializeField] 
    private string _triggerTag = "Player";
    [SerializeField] private MultiLayerAudioEnvironment m_EnvMLA;
    private AudioSource m_audioSource;
    [SerializeField]
    private float flipDownSpeed;
    [SerializeField]
    private Transform _VisualPosition;

    public UnityEvent OnPressed;
    public UnityEvent OnReleased;

    private bool _IsDown;

    private int _objectsOnPlate = 0;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_triggerTag))
        {
            _objectsOnPlate++;

            if (_objectsOnPlate == 1)
            {
                OnPressed?.Invoke();
                m_EnvMLA.PlayContainerElement(m_audioSource, EnvironmentElements.PressurePlateDown);
                _IsDown = true;
            }
        }
    }

    private void Update()
    {
        if (_IsDown)
        {
            _VisualPosition.localPosition = Vector3.Slerp(_VisualPosition.localPosition, Vector3.down * -0.2f, flipDownSpeed * Time.deltaTime);
        }
        else if (!_IsDown) {
            _VisualPosition.localPosition = Vector3.Slerp(_VisualPosition.localPosition, Vector3.zero, flipDownSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_triggerTag))
        {
            _objectsOnPlate--;

            if (_objectsOnPlate <= 0)
            {
                _objectsOnPlate = 0; 
                OnReleased?.Invoke();
                m_EnvMLA.PlayContainerElement(m_audioSource, EnvironmentElements.PressurePlateUp);
                _IsDown = false;
            }
        }
    }
}