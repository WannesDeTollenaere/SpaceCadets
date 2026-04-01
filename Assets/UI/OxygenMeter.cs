using UnityEngine;
using UnityEngine.UI;

public class OxygenMeter : MonoBehaviour
{
    [Header("Oxygen")]
    [SerializeField] private Image _boostBarFill;
    [SerializeField] private Oxygen _playerOxygen;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Update()
    {
        Sync();
    }

    private void Sync()
    {
        float current = _playerOxygen ? _playerOxygen.CurrentOxygen : 0f;
        float max = _playerOxygen ? _playerOxygen.MaxOxygen : 100f;
        SetOxygen(current, max);
    }

    private void SetOxygen(float current, float max)
    {
        if (!_boostBarFill) return;

        float percent = (max > 0f) ? (current / max) : 0f;

        _boostBarFill.transform.localScale = new Vector3(1f, percent, 1f);
    }
}
