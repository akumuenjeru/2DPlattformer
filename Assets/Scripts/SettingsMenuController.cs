using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    private Slider _musicSlider;
    private Slider _effectsSlider;
    private AudioSource _buttonSound;
    private MusicController _musicController;
    public AudioMixer mainMixer;

    void Start()
    {
        _buttonSound = GetComponent<AudioSource>();

        _musicController = GameObject.Find("MusicManager").GetComponent<MusicController>();
        _musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        _effectsSlider = GameObject.Find("EffectsSlider").GetComponent<Slider>();

        // ReSharper disable once PossibleInvalidOperationException
        _musicSlider.value = (float) _musicController.GetMusicVolume();
        // ReSharper disable once PossibleInvalidOperationException
        _effectsSlider.value = (float) _musicController.GetEffectsVolume();
    }

    public void BackToMenu()
    {
        _buttonSound.Play();
        SceneManager.LoadScene("MainMenu");
    }
    
    //used by Sliders as OnChange Event
    public void SetMixerMusicVolume()
    {
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(_musicSlider.value) * 20);
        _musicController.SetMusicVolume(_musicSlider.value);
    }

    public void SetMixerEffectsVolume()
    {
        mainMixer.SetFloat("EffectsVolume", Mathf.Log10(_effectsSlider.value) * 20);
        _musicController.SetEffectsVolume(_effectsSlider.value);
    }
}
