using UnityEngine;

public class StaticSpikesTrigger : MonoBehaviour
{
    private GameController _gameController;

    void Start()
    {
        _gameController = GameObject.Find("GameManager").GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.name == "Player")
        {
            _gameController.GameOver();   
        }
    }
}