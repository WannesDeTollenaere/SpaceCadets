using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public void Die()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.PlayerDied();
        }
    }
}