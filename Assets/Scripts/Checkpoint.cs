using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private int playersEntered = 0;

    [SerializeField] private int amountOfPlayersThatHaveToEnter = 1; 
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);

        if (other.CompareTag("Player"))
            playersEntered++;

        if (playersEntered >= amountOfPlayersThatHaveToEnter)
            PlayerManager.Instance.UpdateCheckpoint(transform.position);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playersEntered--;

        if(playersEntered <= 0) playersEntered = 0;
    }
}