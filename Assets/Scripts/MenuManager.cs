using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; 

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _settingsPanel;


    [SerializeField] private GameObject _mainMenuFirstSelected;
    [SerializeField] private GameObject _settingsFirstSelected;

    [SerializeField] private string _gameSceneName = "GameScene";

    private void Start()
    {
        ShowMainMenu();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(_gameSceneName);
    }

    public void ShowSettings()
    {
        _mainMenuPanel.SetActive(false);
        if (_settingsPanel != null)
        {
            _settingsPanel.SetActive(true);

            SetSelectedUIObject(_settingsFirstSelected);
        }
    }

    public void ShowMainMenu()
    {
        if (_settingsPanel != null)
            _settingsPanel.SetActive(false);

        _mainMenuPanel.SetActive(true);
        SetSelectedUIObject(_mainMenuFirstSelected);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }

    private void SetSelectedUIObject(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(obj);
    }
}