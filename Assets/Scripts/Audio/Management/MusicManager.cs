using SpaceCadets.Audio;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

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
    [SerializeField] AudioMixer m_mixer;


    private void Awake()
    {
        m_mixer.SetFloat("MusicVolume", -80f);
        //Start Volume 
        m_percussionTomsSource.volume = 0;
        m_percussionClapsSource.volume = 0;
        m_melodySource.volume = 0;

        //Start Playing
        m_musicMLA.PlayContainerElement(m_drumSource, MusicElements.Drums, true);
        m_musicMLA.PlayContainerElement(m_percussionMainSource, MusicElements.PercussionMain, true);
        m_musicMLA.PlayContainerElement(m_percussionTomsSource, MusicElements.PercussionToms, true);
        m_musicMLA.PlayContainerElement(m_percussionClapsSource, MusicElements.PercussionClaps, true);
        m_musicMLA.PlayContainerElement(m_bassSource, MusicElements.Bass, true);
        m_musicMLA.PlayContainerElement(m_padSource, MusicElements.Pad, true);
        m_musicMLA.PlayContainerElement(m_melodySource, MusicElements.Melody, true);

        StartCoroutine(FadeMixerVolume("MusicVolume", -40f, -8.0f, 4f));
    }

    private void OnEnable()
    {
        AudioEvents.OnWallExploded += HandleWallExplosion;
    }

    private IEnumerator FadeMixerVolume(string parameter, float fromDb, float toDb, float duration)
    {
        float fromLinear = Mathf.Pow(10f, fromDb / 20f);
        float toLinear = Mathf.Pow(10f, toDb / 20f);

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float linear = Mathf.Lerp(fromLinear, toLinear, t / duration);
            float db = Mathf.Log10(Mathf.Max(linear, 0.0001f)) * 20f;
            m_mixer.SetFloat(parameter, db);
            yield return null;
        }
        m_mixer.SetFloat(parameter, toDb);
    }

    private void HandleWallExplosion()
    {
        m_musicMLA.FadeInAndPlay(m_melodySource, this, 1, 10.0f);
        Debug.Log("HandleWall Explosion");
    }




}
