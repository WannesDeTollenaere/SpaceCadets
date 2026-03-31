using UnityEngine;

public class Lift : MonoBehaviour
{

    [SerializeField]
    private Transform _ElevatorAnim;
    [SerializeField]
    private float _ExtendSpeed;

    private bool _IsExtended;

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

    }

    public void Deactivate()
    {
        _State = ExtendState.MovingDown;
        //naarbenedensound
        Debug.Log("ELEVATOR GOING DOWN");

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
                //FINISH GOING UP SOUND
                Debug.Log("ELEVATOR REACHED DOWN");
            }
        }
    }
}