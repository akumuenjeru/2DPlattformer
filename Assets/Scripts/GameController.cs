using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private GameObject _player;
    private GameObject[] _perlTriggers;
    
    private TextMeshProUGUI _timer;
    private TextMeshProUGUI _gameOver;
    private Canvas _settingsCanvas;
    private AudioSource _mainAudio;
    private AudioSource _buttonClickAudio;
    
    private CollectingPerls _collectingPerls;

    public float timeLeft = 30.00f;
    private Slider _slider;
    private Slider _effectsSlider;
    public AudioMixer mainMixer;

    private void Start()
    {
        _mainAudio = GetComponent<AudioSource>();
        _player = GameObject.Find("Player");
        _perlTriggers = GameObject.FindGameObjectsWithTag("Perl");
        //handles Perl Count
        _collectingPerls = gameObject.GetComponent<CollectingPerls>();

        _settingsCanvas = GameObject.Find("SettingsCanvas").GetComponent<Canvas>();
        _settingsCanvas.enabled = false;
        
        _buttonClickAudio = _settingsCanvas.GetComponent<AudioSource>();
        _buttonClickAudio.Stop();

        _timer = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        _gameOver = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        _gameOver.enabled = false;
        
        _timer.text = "Time Left: " + timeLeft;
        
        _slider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        _effectsSlider = GameObject.Find("EffectsSlider").GetComponent<Slider>();

        //assigns each perl their script and sets up the collection counting system
        foreach (var perl in _perlTriggers)
        {
            if (perl == null) continue;
            PerlTriggerController perlScript = perl.GetComponent<PerlTriggerController>();
            if (perlScript == null) continue;
            perlScript.Setup(_collectingPerls);
        }
        _collectingPerls.UpdateText();
        InvokeRepeating(nameof(Timer), 1.0f, 1.0f);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Time.timeScale = 0;
            _settingsCanvas.enabled = true;
        }
    }

    void Timer()
    {
        timeLeft--;
        _timer.text = "Time Left: " + timeLeft;
        
        if (timeLeft == 0)
        {
            GameOver();
        }
        
        //restarts scenes after 3 seconds
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (timeLeft == -3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void GameOver()
    {
        _timer.enabled = false;
        timeLeft = 0;
        _player.SetActive(false);
        _mainAudio.Stop();
        _gameOver.enabled = true;
        Debug.Log("Game Over!");
    }

    public void Resume()
    {
        _buttonClickAudio.Play();
        Time.timeScale = 1;
        _settingsCanvas.enabled = false;
    }

    public void SetMusicVolume()
    {
        //_mainAudio.volume = _slider.value;
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(_slider.value) * 20);
    }

    public void SetEffectsVolume()
    {
        mainMixer.SetFloat("EffectsVolume", Mathf.Log10(_effectsSlider.value) * 20);
    }
}