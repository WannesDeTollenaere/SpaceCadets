using SpaceCadets.Audio;
using UnityEngine;
using UnityEngine.Events;

public class ExplodableWall : MonoBehaviour, ICell
{
    public ICell.State CellState { get; set;} = ICell.State.Hidden;

    [SerializeField]
    private GameObject _scannedVisual;
    [SerializeField]
    private GameObject _revealedPrefab;

    [SerializeField]private MultiLayerAudioEnvironment m_envMLA;
    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
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

        AudioEvents.WallExploded();

        if (_revealedPrefab != null)
            Instantiate(_revealedPrefab, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}