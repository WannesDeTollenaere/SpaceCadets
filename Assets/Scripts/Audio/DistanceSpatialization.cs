using System.Collections;
using UnityEngine;

public class DistanceSpatialization : MonoBehaviour
{
    [SerializeField] private float _maxDistance = 20f;
    [SerializeField] private float _maxVolume = 1f;
    [SerializeField] private GameObject m_lilGuy;
    [SerializeField] private GameObject m_robot;

    private AudioSource _audioSource;
    

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.spatialBlend = 0f; 
        
    }

    private void Update()
    {
        if (m_lilGuy == null && m_robot == null) return;

        float averageDistance = GetAverageDistance();
        _audioSource.volume = CalculateVolume(averageDistance);
       // Debug.Log($"Avg distance: {averageDistance}, Volume: {_audioSource.volume}");
    }

    private float GetAverageDistance()
    {
        // If only one player is present, use that player's distance
        if (m_lilGuy == null)
            return Vector3.Distance(transform.position, m_robot.transform.position);

        if (m_robot == null)
            return Vector3.Distance(transform.position, m_lilGuy.transform.position);

        float distanceToDark = Vector3.Distance(transform.position, m_lilGuy.transform.position);
        float distanceToLight = Vector3.Distance(transform.position, m_robot.transform.position);

        return (distanceToDark + distanceToLight) / 2f;
    }

    private float CalculateVolume(float averageDistance)
    {
        // Inverse linear falloff from maxVolume at distance 0 to 0 at maxDistance
        float normalizedDistance = Mathf.Clamp01(averageDistance / _maxDistance);
        return Mathf.Lerp(_maxVolume, 0f, normalizedDistance);
    }
    
}