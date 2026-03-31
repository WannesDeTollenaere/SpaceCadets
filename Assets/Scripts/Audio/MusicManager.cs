using SpaceCadets.Audio;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource m_drumSource;
    [SerializeField] AudioSource m_percussionMainSource;
    [SerializeField] AudioSource m_percussionTomsSource;
    [SerializeField] AudioSource m_percussionClapsSource;
    [SerializeField] AudioSource m_bassSource;
    [SerializeField] AudioSource m_padSource;
    [SerializeField] AudioSource m_melodySource;
    [SerializeField] MultiLayerAudioMusic m_musicMLA;

    private void Awake()
    {
        m_percussionTomsSource.volume = 0;
        m_percussionClapsSource.volume = 0;
        m_melodySource.volume = 0;
    }

    
}
