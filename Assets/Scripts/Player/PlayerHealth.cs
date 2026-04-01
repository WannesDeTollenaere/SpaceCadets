using SpaceCadets.Audio;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float _respawnTime = 2.0f;

    private bool _isDead = false;

    public UnityEvent OnPlayerDied;
    public UnityEvent OnPlayerRespawned;

    public void Die()
    {
        if (PlayerManager.Instance != null && !_isDead)
        {
            _isDead = true;

            OnPlayerDied?.Invoke();
            StartCoroutine(RespawnTimerRoutine());
            //PlayerManager.Instance.PlayerDied();
            AudioEvents.PlayerDamage();
        }
    }

    IEnumerator RespawnTimerRoutine()
    {
        float timer = 0.0f;

        while (timer < _respawnTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        _isDead = false;
        PlayerManager.Instance.PlayerDied();

        OnPlayerRespawned?.Invoke();
    }
}