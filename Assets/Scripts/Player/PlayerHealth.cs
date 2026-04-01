using SpaceCadets.Audio;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public UnityEvent OnPlayerDied;

    public void Die()
    {
        if (PlayerManager.Instance != null)
        {
            Debug.Log("Kill Player");

            OnPlayerDied?.Invoke();
            AudioEvents.PlayerDamage();
            PlayerManager.Instance.PlayerDied();
        }
    }
}