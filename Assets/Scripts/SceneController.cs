using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private string _sceneName = "Level1";
    private AudioSource _buttonSound;
    private MusicController _musicController;

    private void Start()
    {
        _buttonSound = GetComponent<AudioSource>();
        _musicController = GameObject.Find("MusicManager").GetComponent<MusicController>();
        if (transform.name == "GameOverCanvas")
        {
            _musicController.PlayMusic();
        }
    }

    public void StartGame()
    {
        _buttonSound.Play();
        SceneManager.LoadScene(_sceneName);
    }

    public void OpenSettings()
    {
        _buttonSound.Play();
        SceneManager.LoadScene("Settings");
    }

    public void QuitGame()
    {
        _buttonSound.Play();
        Application.Quit();
        Debug.Log("Application terminated.");
    }

    public void GoBackToStart()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
