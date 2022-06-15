using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Vector3 _initialPosition;
    private Vector3 _positionA;
    private Vector3 _positionB;
    
    private bool _arrivedAtA;
    private bool _arrivedAtB;
    
    public float speed = 2f;
    public float radius = 3f;
    
    public GameObject playerObject;
    
    void Start()
    {
        playerObject = GameObject.Find("Player");
        _initialPosition = gameObject.transform.position;
        _positionA = new Vector3(_initialPosition.x - radius, _initialPosition.y, _initialPosition.z);
        _positionB = new Vector3(_initialPosition.x + radius, _initialPosition.y, _initialPosition.z);
    }
    
    void Update()
    {
        MoveEnemy();
    }

    //moves Enemy from one side to the other
    void MoveEnemy()
    {
        float step = speed * Time.deltaTime;
        //initial movement from initial position
        if (!_arrivedAtA && !_arrivedAtB)
        {
            transform.position = Vector3.MoveTowards(transform.position, _positionB, step);
        }

        if (transform.position == _positionA)
        {
            _arrivedAtB = false;
            _arrivedAtA = true;
            //flips enemy to look into correct direction
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        if (transform.position == _positionB)
        {
            _arrivedAtA = false;
            _arrivedAtB = true;
            //flips enemy to look into correct direction
            transform.Rotate(0.0f, 180f, 0.0f);
        }

        //moves enemy from left to right
        if (_arrivedAtA)
        {
            transform.position = Vector3.MoveTowards(transform.position, _positionB, step);
        }

        //moves enemy from right to left
        if (_arrivedAtB)
        {
            transform.position = Vector3.MoveTowards(transform.position, _positionA, step);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Player")
        {
            playerObject.SetActive(false);
            Debug.Log("GameOver");
        }
    }
}