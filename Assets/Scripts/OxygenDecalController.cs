using UnityEngine;
using UnityEngine.UI;

public class OxygenDecalController : MonoBehaviour
{
    [SerializeField]
    private Image _imageToChange;

    [SerializeField]
    private float _minDistance = 3f;

    [SerializeField]
    private float _maxDistance = 4f;

    [SerializeField]
    private float _maxAlpha = 0.5f;

    [SerializeField]
    private float _dangerDistance = 4.2f;

    [SerializeField]
    private Color _dangerColor = Color.red;

    [Header("Pulsate Settings")]
    [SerializeField]
    private float _pulseSpeed = 5f;

    private Color _baseColor;

    void Start()
    {
        if (_imageToChange != null)
        {
            _baseColor = _imageToChange.color;
        }
    }

    void Update()
    {
        if (_imageToChange == null || PlayerManager.Instance == null) return;

        float currentSqrDistance = PlayerManager.Instance.SqrDistanceBetweenPlayers();

        float sqrMinDistance = _minDistance * _minDistance;
        float sqrMaxDistance = _maxDistance * _maxDistance;
        float sqrDangerDistance = _dangerDistance * _dangerDistance;

        float targetAlpha = Mathf.InverseLerp(sqrMinDistance, sqrMaxDistance, currentSqrDistance);
        float colorShiftPercentage = Mathf.InverseLerp(sqrMaxDistance, sqrDangerDistance, currentSqrDistance);


        float sineWave = (Mathf.Sin(Time.time * _pulseSpeed) + 1f) * 0.5f;


        float throbIntensity = Mathf.Lerp(0.5f, 1f, sineWave);

        float pulsatingShift = colorShiftPercentage * throbIntensity;

        Color finalColor = Color.Lerp(_baseColor, _dangerColor, pulsatingShift);

        finalColor.a = Mathf.Min(targetAlpha, _maxAlpha);

        _imageToChange.color = finalColor;
    }
}