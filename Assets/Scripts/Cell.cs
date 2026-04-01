using SpaceCadets.Audio;
using UnityEngine;
using UnityEngine.Events;

public class Cell : MonoBehaviour, ICell
{
    public ICell.State CellState { get; set;} = ICell.State.Hidden;

    [SerializeField]
    private GameObject _revealedPrefab;

    [SerializeField]
    private GameObject _scannedVisual;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CellState != ICell.State.Triggered)
        {
            Activate();
        }
    }
    public bool Reveal()
    {
        if (CellState == ICell.State.Triggered || CellState == ICell.State.Revealed)
            return false;

        CellState = ICell.State.Revealed;

        if (_scannedVisual != null)
        {
            _scannedVisual.SetActive(true);

            return true;
        }
        return false;
    }

    public void Hide()
    {
        if (CellState == ICell.State.Triggered || CellState == ICell.State.Hidden)
            return;

        CellState = ICell.State.Hidden;

        if (_scannedVisual != null)
        {
            _scannedVisual.SetActive(false);
        }
    }

    public void Activate()
    {
        CellState = ICell.State.Triggered;
        if (_revealedPrefab != null)      
            Instantiate(_revealedPrefab, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    public bool IsBomb()
    {
        return _revealedPrefab != null;
    }
}