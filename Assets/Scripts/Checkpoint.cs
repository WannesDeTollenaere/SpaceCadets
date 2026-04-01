using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private int playersEntered = 0;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);

        if (other.CompareTag("Player"))
            playersEntered++;

        if (playersEntered >= 2)
            PlayerManager.Instance.UpdateCheckpoint(transform.position);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playersEntered--;

        if(playersEntered <= 0) playersEntered = 0;
    }
}