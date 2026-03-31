using System.Collections;
using UnityEngine;

namespace SpaceCadets.Audio
{
    public class MultiLayerAudioBase <T> : ScriptableObject where T : System.Enum
    {

        [SerializeField] private Vector2 m_pitchMinMax = Vector2.one;
        [SerializeField] private AudioLayer<T>[] m_layers;

        public void PlayMultiLayerOnSource(AudioSource source)
        {
            source.pitch = Random.Range(m_pitchMinMax.x, m_pitchMinMax.y);
            foreach (AudioLayer<T> layer in m_layers)
            {
                layer.PlayOnSource(source);
            }
        }

        public void PlayContainerElement(AudioSource source, T element, bool shouldLoop = false)
        {
            foreach (AudioLayer<T> layer in m_layers)
            {
                if (layer.ElementName.Equals(element))
                {
                    if(!source.isPlaying)
                    {
                        source.pitch = Random.Range(m_pitchMinMax.x, m_pitchMinMax.y);

                    }
                    source.loop = shouldLoop;
                    layer.PlayOnSource(source,shouldLoop);
                    return;
                }
            }

            //Debug.LogWarning($"AudioLayer with element {element} not found!");
        }
        public void FadeOutAndStop(AudioSource source, MonoBehaviour caller, float duration = 0.05f)
        {
            caller.StartCoroutine(FadeCoroutine(source, duration));
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
    }
}