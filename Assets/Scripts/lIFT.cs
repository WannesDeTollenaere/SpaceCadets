using SpaceCadets.Audio;
using UnityEngine;

public class Lift : MonoBehaviour
{

    [SerializeField]
    private Transform _ElevatorAnim;
    [SerializeField]
    private float _ExtendSpeed;

    [SerializeField] MultiLayerAudioEnvironment m_envMLA;
    private AudioSource m_audioSource;

    private bool _IsExtended;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }
    private enum ExtendState
    {
        Down,
        MovingUp,
        Up,
        MovingDown
    }

    private ExtendState _State;

    public void Activate()
    {
        _State = ExtendState.MovingUp;

        //beginsound
        Debug.Log("ELEVATOR GOING UP");
        m_envMLA.PlayContainerElement(m_audioSource, EnvironmentElements.ElevatorUp);

    }

    public void Deactivate()
    {
        _State = ExtendState.MovingDown;
        //naarbenedensound
        Debug.Log("ELEVATOR GOING DOWN");
        m_envMLA.PlayContainerElement(m_audioSource, EnvironmentElements.ElevatorDown);

    }

    private void Update()
    {
        if (_ElevatorAnim == null) return;

        if (_State == ExtendState.MovingUp)
        {
            _ElevatorAnim.localPosition = Vector3.Slerp(_ElevatorAnim.localPosition, Vector3.up, _ExtendSpeed * Time.deltaTime);
            if (Vector3.Distance(_ElevatorAnim.localPosition, Vector3.up) < 0.1)
            {
                _State = ExtendState.Up;
                //FINISH GOING UP SOUND
                Debug.Log("ELEVATOR REACHED UP");
            }

        } else if ( _State == ExtendState.MovingDown)
        {
            _ElevatorAnim.localPosition = Vector3.Slerp(_ElevatorAnim.localPosition, Vector3.zero, _ExtendSpeed * Time.deltaTime);
            if (Vector3.Distance(_ElevatorAnim.localPosition, Vector3.zero) < 0.1)
            {
                _State = ExtendState.Down;
                //FINISH GOING D SOUND
                Debug.Log("ELEVATOR REACHED DOWN");
            }
        }
    }
}