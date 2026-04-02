using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionBoxSceneTransitioner : MonoBehaviour
{
    [SerializeField] private string _gameSceneName = "EndScene";
    [SerializeField] private int _amountOfPlayersThatHaveToEnterToEndTheGameAndGoToTheEndSceneAndMaybeReplayTheGameOrQuitIDK67 = 2;

    private int playersEntered = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playersEntered++;

        if(playersEntered >= _amountOfPlayersThatHaveToEnterToEndTheGameAndGoToTheEndSceneAndMaybeReplayTheGameOrQuitIDK67)
            SceneManager.LoadScene(_gameSceneName);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playersEntered--;
    }
}
