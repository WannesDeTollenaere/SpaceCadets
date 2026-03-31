using UnityEngine;

public class BombLogic : MonoBehaviour, ICell
{
    public bool IsActivated { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !IsActivated)
        {
            Activate();

            Destroy(other.gameObject);
        }
    }

    public void Activate()
    {
        IsActivated = true;
    }
}