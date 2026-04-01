using System;
using UnityEngine;
using SpaceCadets.Audio;
using System.Collections;

public class BombLogic : MonoBehaviour, ICell
{
    [SerializeField] private MultiLayerAudioEnvironment m_EnvMLA;
    private AudioSource m_audioSource;
    public ICell.State CellState { get; set; } = ICell.State.Hidden;


    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CellState != ICell.State.Triggered)
        {
            Activate();

            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Die();
            }
        }
    }

    public void Reveal()
    {
        if (CellState == ICell.State.Triggered) return;


    }

    public void Activate()
    {
        AudioEvents.BombExlpode();
       
        // Add 0.5s delay before visual shows up here
        CellState = ICell.State.Triggered;


    }

    
}