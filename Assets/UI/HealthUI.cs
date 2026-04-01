using UnityEngine;
using TMPro; 

public class HealthUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI livesText;

    private void Start()
    {
        UpdateLivesText(PlayerManager.Instance.TotalLives);

        PlayerManager.Instance.OnHealthChanged.AddListener(UpdateLivesText);
    }

    private void OnDestroy()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnHealthChanged.RemoveListener(UpdateLivesText);
        }
    }

    private void UpdateLivesText(int currentLives)
    {
        livesText.text = "Lives: " + currentLives;
    }
}