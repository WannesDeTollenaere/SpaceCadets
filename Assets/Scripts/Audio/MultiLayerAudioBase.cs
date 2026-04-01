using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SpaceCadets.Audio
{
    public class MultiLayerAudioBase <T> : ScriptableObject where T : System.Enum
    {

        [SerializeField] private Vector2 m_pitchMinMax = Vector2.one;
        [SerializeField] private AudioLayer<T>[] m_layers;
        private Dictionary<AudioSource, Coroutine> m_activeCoroutines = new Dictionary<AudioSource, Coroutine>();

        public void PlayMultiLayerOnSource(AudioSource source)
        {
            source.pitch = Random.Range(m_pitchMinMax.x, m_pitchMinMax.y);
            foreach (AudioLayer<T> layer in m_layers)
            {
                layer.PlayOnSource(source);
            }
        }

        public void PlayContainerElement(AudioSource source, T element, bool shouldLoop = false, MonoBehaviour caller = null, float fadeInDuration = 0f, float targetVolume = 1f)
        {
            foreach (AudioLayer<T> layer in m_layers)
            {
                if (layer.ElementName.Equals(element))
                {
                 
                    if (!source.isPlaying)
                    {
                        source.pitch = Random.Range(m_pitchMinMax.x, m_pitchMinMax.y);
                    }
                    source.loop = shouldLoop;

                    if (caller != null && fadeInDuration > 0f)
                    {
                        layer.PlayOnSource(source, shouldLoop);
                        FadeInAndPlay(source, caller, targetVolume, fadeInDuration);
                    }
                    else
                    {
                        layer.PlayOnSource(source, shouldLoop);
                    }

                    return;
                }
            }
        }
        public void FadeOutAndStop(AudioSource source, MonoBehaviour caller, float duration = 0.05f)
        {
            if (m_activeCoroutines.TryGetValue(source, out Coroutine existing))
                caller.StopCoroutine(existing);
            m_activeCoroutines[source] = caller.StartCoroutine(FadeCoroutine(source, duration));
        }

        private IEnumerator FadeCoroutine(AudioSource source, float duration)
        {
            float startVolume = source.volume;
            //Debug.Log($"Fade started, source.volume is: {source.volume}");
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                source.volume = Mathf.Lerp(startVolume, 0f, t / duration);
                yield return null;
            }
            //Debug.Log($"Fade finished, restoring to: {startVolume}");
            source.Stop();
            source.volume = startVolume;
        }

        public void FadeInAndPlay(AudioSource source, MonoBehaviour caller, float targetVolume = 1f, float duration = 0.05f)
        {
            if (m_activeCoroutines.TryGetValue(source, out Coroutine existing))
                caller.StopCoroutine(existing);
            source.volume = 0f;
           // source.Play();
            m_activeCoroutines[source] = caller.StartCoroutine(FadeInCoroutine(source, targetVolume, duration));
        }

        private IEnumerator FadeInCoroutine(AudioSource source, float targetVolume, float duration)
        {
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                source.volume = Mathf.Lerp(0f, targetVolume, t / duration);
                yield return null;
            }

            source.volume = targetVolume; 
        }
    }
}