using SpaceCadets.Audio;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

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
    
    private List<Collider> _collidersOnPlate = new List<Collider>();

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_triggerTag))
        {
            if (!_collidersOnPlate.Contains(other))
            {
                _collidersOnPlate.Add(other);

                if (_collidersOnPlate.Count == 1 && !_IsDown)
                {
                    OnPressed?.Invoke();
                    m_EnvMLA.PlayContainerElement(m_audioSource, EnvironmentElements.PressurePlateDown);
                    _IsDown = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_triggerTag))
        {
            if (_collidersOnPlate.Contains(other))
            {
                _collidersOnPlate.Remove(other);
                CheckIfPlateShouldRelease();
            }
        }
    }

    private void Update()
    {
        bool collidersRemoved = false;

        for (int i = _collidersOnPlate.Count - 1; i >= 0; i--)
        {
            Collider col = _collidersOnPlate[i];
            if (col == null || !col.enabled || !col.gameObject.activeInHierarchy)
            {
                _collidersOnPlate.RemoveAt(i);
                collidersRemoved = true;
            }
        }

        if (collidersRemoved)
        {
            CheckIfPlateShouldRelease();
        }

        if (_IsDown)
        {
            _VisualPosition.localPosition = Vector3.Slerp(_VisualPosition.localPosition, Vector3.down * 0.1f, flipDownSpeed * Time.deltaTime);
        }
        else
        {
            _VisualPosition.localPosition = Vector3.Slerp(_VisualPosition.localPosition, Vector3.zero, flipDownSpeed * Time.deltaTime);
        }
    }

    private void CheckIfPlateShouldRelease()
    {
        if (_collidersOnPlate.Count == 0 && _IsDown)
        {
            OnReleased?.Invoke();
            m_EnvMLA.PlayContainerElement(m_audioSource, EnvironmentElements.PressurePlateUp);
            _IsDown = false;
        }
    }
}