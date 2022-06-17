using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishController : MonoBehaviour
{
    private GameController _gameController;
    void Start()
    {
        _gameController = GameObject.Find("GameManager").GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        string objectName = gameObject.name;
        if (col.gameObject.name == "Player")
        {
            if(_gameController.WinCondition(objectName)) HandleEndReached();
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void HandleEndReached()
    {
        if (gameObject.name == "EndLevel1")
        {
            SceneManager.LoadScene("Level2");
        }

        if (gameObject.name == "EndLevel2")
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}
