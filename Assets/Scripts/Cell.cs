using UnityEngine;

public class Cell : MonoBehaviour, ICell
{
    public bool IsActivated { get; private set; }

    [SerializeField]
    private GameObject _revealedPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !IsActivated)
        {
            Activate();
        }
    }

    public void Activate()
    {
        IsActivated = true;
        if (_revealedPrefab != null)      
            Instantiate(_revealedPrefab, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}