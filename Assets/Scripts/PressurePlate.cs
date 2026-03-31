using SpaceCadets.Audio;
using UnityEngine;
using UnityEngine.Events; 

public class PressurePlate : MonoBehaviour
{

    [SerializeField] 
    private string _triggerTag = "Player";
    [SerializeField] private MultiLayerAudioEnvironment m_EnvMLA;
    private AudioSource m_audioSource;

    public UnityEvent OnPressed;
    public UnityEvent OnReleased;

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

            }
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
            }
        }
    }
}