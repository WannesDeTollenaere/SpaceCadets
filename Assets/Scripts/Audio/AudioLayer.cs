
using UnityEngine;


namespace SpaceCadets.Audio
{
    [System.Serializable]
    public class AudioLayer<T> where T :System.Enum
    {
        [SerializeField] private AudioClip[] m_clips;
        [SerializeField] private Vector2 m_minMaxVolume = Vector2.one;
        [SerializeField] private T m_elementName;

        public T ElementName => m_elementName;

        private AudioClip[] m_recentlyPlayed;
        private int m_recentCount = 0;
        public float GetRandomVolume()
        {
            return UnityEngine.Random.Range(m_minMaxVolume.x, m_minMaxVolume.y);
        }

        public AudioClip GetRandom()
        {
            if (m_clips == null || m_clips.Length == 0)
            {
                Debug.LogWarning("AudioLayer has no clips assigned!");
                return null;
            }

            if (m_clips.Length == 1)
            {
                return m_clips[0];
            }

            int noRepeatCount = m_clips.Length / 2;

            if (m_recentlyPlayed == null || m_recentlyPlayed.Length != noRepeatCount)
            {
                m_recentlyPlayed = new AudioClip[noRepeatCount];
                m_recentCount = 0;
            }


            AudioClip selectedClip = null;
            int attempts = 0;

            while (attempts < 50)
            {
                selectedClip = m_clips[UnityEngine.Random.Range(0, m_clips.Length)];

                bool isRecent = false;
                for (int i = 0; i < m_recentCount; i++)
                {
                    if (m_recentlyPlayed[i] == selectedClip)
                    {
                        isRecent = true;
                        break;
                    }
                }

                if (!isRecent)
                {
                    break;
                }

                attempts++;
            }

            if (m_recentCount < m_recentlyPlayed.Length)
            {
                m_recentlyPlayed[m_recentCount] = selectedClip;
                m_recentCount++;
            }
            else
            {

                for (int i = 0; i < m_recentlyPlayed.Length - 1; i++)
                {
                    m_recentlyPlayed[i] = m_recentlyPlayed[i + 1];
                }
                m_recentlyPlayed[m_recentlyPlayed.Length - 1] = selectedClip;
            }

            return selectedClip;
        }
        public void PlayOnSource(AudioSource source,bool loop = false)
        {
            AudioClip clip = GetRandom();
            float volume = GetRandomVolume();
            if (loop)
            {
                source.clip = clip;
                source.volume = volume;
                //Debug.Log($"clip: {source.clip.name}, volume: {source.volume}, loop: {source.loop}, pitch: {source.pitch}, spatialBlend: {source.spatialBlend}");
                if (!source.isPlaying)
                {
                    source.Play();
                }

            }
            else
            {
                source.PlayOneShot(clip, volume);
            }
        }

        
    }
}
