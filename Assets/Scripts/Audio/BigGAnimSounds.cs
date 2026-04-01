using SpaceCadets.Audio;
using UnityEngine;

public class BigGAnimSounds : MonoBehaviour
{
    [SerializeField] private MultiLayerAudioRobot m_bigGMLA;
    [SerializeField] private AudioSource m_oneshotSource;
    [SerializeField] private AudioSource m_footstepSource;


    private void PlayBigGIdleSound()
    {
        m_bigGMLA.PlayContainerElement(m_oneshotSource, RobotElements.Idling);
       // Debug.Log("Play idle");
    }

    private void PlayBGFootstepSound()
    {
        m_bigGMLA.PlayContainerElement(m_footstepSource, RobotElements.Footstep);
       // Debug.Log("Play footstep");
    }
}
