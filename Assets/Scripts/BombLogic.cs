using UnityEngine;

public class BombLogic : MonoBehaviour, ICell
{
    public ICell.State CellState { get; set; } = ICell.State.Hidden;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CellState != ICell.State.Triggered)
        {
            Activate();

            // kill player
            Destroy(other.gameObject);
        }
    }

    public void Reveal()
    {
        if (CellState == ICell.State.Triggered) return;


    }

    public void Activate()
    {
        CellState = ICell.State.Triggered;
    }
}