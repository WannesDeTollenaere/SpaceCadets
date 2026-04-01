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
    [SerializeField] AudioSource m_envSource;
    [SerializeField] AudioSource m_damageSource;
    [SerializeField] MultiLayerAudioMusic m_musicMLA;
    [SerializeField] MultiLayerAudioEnvironment m_envMLA;
    [SerializeField] AudioMixer m_mixer;
    private bool m_addedClapsStem = false;
    private float m_lastDamagePlayTime = -3f;

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

        StartCoroutine(FadeMixerVolume("MusicVolume", -40f, -12.0f, 4f));
    }

    private void OnEnable()
    {
        AudioEvents.OnWallExploded += HandleWallExplosion;
        AudioEvents.OnElevatorUp += HandleElevatorUp;
        AudioEvents.OnAttach += HandleAttach;
        AudioEvents.OnDetach += HandleDetach;
        AudioEvents.OnBombExplode += HandleBombExplode;
        AudioEvents.OnPlayerDamaged += HandlePlayerDamaged;

    }

    private void OnDisable()
    {
        AudioEvents.OnWallExploded -= HandleWallExplosion;
        AudioEvents.OnElevatorUp -= HandleElevatorUp;
        AudioEvents.OnAttach -= HandleAttach;
        AudioEvents.OnDetach -= HandleDetach;
        AudioEvents.OnBombExplode -= HandleBombExplode;
        AudioEvents.OnPlayerDamaged -= HandlePlayerDamaged;

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
        m_envMLA.PlayContainerElement(m_envSource, EnvironmentElements.WallExplode);
        Debug.Log("Fade in melody");
    }
    private void HandleElevatorUp()
    {
        if (!m_addedClapsStem)
        {
            m_musicMLA.FadeInAndPlay(m_percussionClapsSource, this, 1, 10.0f);
            Debug.Log("Fade in clappas");
            m_addedClapsStem = true;
        }
       
    }

    private void HandleAttach()
    {
        m_musicMLA.FadeOutAndStop(m_percussionTomsSource, this, 2.0f);
            Debug.Log("Fade out Toms");

    }

    private void HandleDetach()
    {
        m_musicMLA.FadeInAndPlay(m_percussionTomsSource, this, 1, 5.0f);
        Debug.Log("Fade In Toms");

    }

    private void HandleBombExplode()
    {
        m_envMLA.PlayContainerElement(m_envSource, EnvironmentElements.BombExplode);

    }
    private void HandlePlayerDamaged()
    {
        if (Time.time - m_lastDamagePlayTime >= 3f)
        {
            m_lastDamagePlayTime = Time.time;
            m_envMLA.PlayContainerElement(m_damageSource, EnvironmentElements.TakeDamage);
            Debug.Log("Play damaged sound");
        }


    }






}
