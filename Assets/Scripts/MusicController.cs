using UnityEngine;

public class MusicController : MonoBehaviour
{
    //makes sure music continue to play in submenus
    private AudioSource _audioSource;
    private float? _musicVolume;
    private float? _effectsVolume;
    private GameObject[] _other;
    private bool _notFirst;
    private void Awake()
    {
        _other = GameObject.FindGameObjectsWithTag("Music");
 
        foreach (GameObject oneOther in _other)
        {
            if (oneOther.scene.buildIndex == -1)
            {
                _notFirst = true;
            }
        }
 
        if (_notFirst)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();
        
        //initialise volumes in case user never uses Settings menu
        if (_musicVolume == null)
        {
            SetMusicVolume(1);
        }

        if (_effectsVolume == null)
        {
            SetEffectsVolume(1);
        }
    }
 
    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }
 
    public void StopMusic()
    {
        _audioSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = volume;
    }

    public float? GetMusicVolume()
    {
        return _musicVolume;
    }
    
    public void SetEffectsVolume(float volume)
    {
        _effectsVolume = volume;
    }

    public float? GetEffectsVolume()
    {
        return _effectsVolume;
    }
}
