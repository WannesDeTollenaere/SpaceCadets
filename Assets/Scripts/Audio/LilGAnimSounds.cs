using SpaceCadets.Audio;
using UnityEngine;

public class LilGAnimSounds : MonoBehaviour
{
    [SerializeField] private MultiLayerAudioLilGuy m_lilGMLA;
    [SerializeField] private AudioSource m_oneshotSource;
    [SerializeField] private AudioSource m_footstepSource;


    public void PlayLilGDeadSound()
    {
        m_lilGMLA.PlayContainerElement(m_oneshotSource, LilGuyElements.Dead);
        Debug.Log("Play lil G Ded");
    }

    public void PlayLGFootstepSound()
    {
        if (!m_footstepSource.isPlaying)
        {
            m_lilGMLA.PlayContainerElement(m_footstepSource, LilGuyElements.Footstep);
           // Debug.Log("Play lil footstep");
        }
    }

    
}
