using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{

    [SerializeField] private float _timeRemaining = 120f; 
    [SerializeField] private string _gameOverSceneName = "GameOver"; 
    [SerializeField] private TextMeshProUGUI _timerText; 

    private bool _isTimerRunning = false;

    private void Start()
    {
        _isTimerRunning = true;
    }

    private void Update()
    {
        if (_isTimerRunning)
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(_timeRemaining);
            }
            else
            {
                _timeRemaining = 0;
                _isTimerRunning = false;
                UpdateTimerDisplay(_timeRemaining);
                HandleTimerEnded();
            }
        }
    }

    private void UpdateTimerDisplay(float timeToDisplay)
    {
        if (_timerText != null)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void HandleTimerEnded()
    {
        SceneManager.LoadScene(_gameOverSceneName);
    }
}