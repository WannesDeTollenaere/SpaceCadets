using System;
using UnityEngine;
using SpaceCadets.Audio;

public class BombLogic : MonoBehaviour, ICell
{
    [SerializeField] private MultiLayerAudioEnvironment m_EnvMLA;
    private AudioSource m_audioSource;
    public ICell.State CellState { get; set; } = ICell.State.Hidden;
    
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CellState != ICell.State.Triggered)
        {
            Activate();

            // kill player
            Destroy(other.gameObject);
        }
    }

    public void Reveal()
    {
        if (CellState == ICell.State.Triggered) return;


    }

    public void Activate()
    {
        m_EnvMLA.PlayContainerElement(m_audioSource, EnvironmentElements.BombExplode);
        CellState = ICell.State.Triggered;
    }
}