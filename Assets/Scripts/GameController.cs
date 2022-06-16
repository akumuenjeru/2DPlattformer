using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private CollectingPerls _collectingPerls;
    private GameObject[] _perlTriggers;
    public float timeLeft = 30.00f;
    private float _timePassed;
    private TextMeshProUGUI _timer;
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.Find("Player");
        _perlTriggers = GameObject.FindGameObjectsWithTag("Perl");
        //handles Perl Count
        _collectingPerls = gameObject.GetComponent<CollectingPerls>();

        _timer = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        _timer.text = "Time Left: " + timeLeft;

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
        Debug.Log("Game Over!");
    }
}