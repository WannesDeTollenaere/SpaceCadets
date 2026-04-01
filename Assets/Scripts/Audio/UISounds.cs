using SpaceCadets.Audio;
using System.Collections;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    [SerializeField] private MultiLayerAudioUI m_uiMLA;
    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void PlayHoverSound()
    {
        m_uiMLA.PlayContainerElement(m_audioSource, UIElements.Hover);
    }

    public void PlayClickSound()
    {
        m_uiMLA.PlayContainerElement(m_audioSource, UIElements.Click);
        m_audioSource.transform.SetParent(null);
        DontDestroyOnLoad(m_audioSource.gameObject);
        StartCoroutine(DestroyAfterSound());
    }

    private IEnumerator DestroyAfterSound()
    {
        yield return new WaitWhile(() => m_audioSource.isPlaying);
        Destroy(m_audioSource.gameObject);
    }
}
