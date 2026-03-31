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
        //Start Volume 
        //m_percussionTomsSource.volume = 0;
       // m_percussionClapsSource.volume = 0;
        m_melodySource.volume = 0;

        //Start Playing
        m_musicMLA.PlayContainerElement(m_percussionMainSource, MusicElements.Percussion, true);
        m_musicMLA.PlayContainerElement(m_melodySource, MusicElements.Melody, true);
    }

    private void OnEnable()
    {
        AudioEvents.OnWallExploded += HandleWallExplosion;
    }

    private void HandleWallExplosion()
    {
        m_musicMLA.FadeInAndPlay(m_melodySource, this, 1, 10.0f);
        Debug.Log("HandleWall Explosion");
    }




}
