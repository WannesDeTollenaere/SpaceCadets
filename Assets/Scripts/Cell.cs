using UnityEngine;

public class Cell : MonoBehaviour, ICell
{
    public ICell.State CellState { get; set;} = ICell.State.Hidden;

    [SerializeField]
    private GameObject _revealedPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CellState != ICell.State.Triggered)
        {
            Activate();
        }
    }

    public void Activate()
    {
        CellState = ICell.State.Triggered;
        if (_revealedPrefab != null)      
            Instantiate(_revealedPrefab, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}