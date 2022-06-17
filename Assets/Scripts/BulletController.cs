using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 20f;
    public int damage = 50;
    public GameObject impactEffect;
    private GameObject _impactEffectClone;
    
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        SimpleEnemyController enemy = hitInfo.GetComponent<SimpleEnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        _impactEffectClone = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
       
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.name == "Plattforms")
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Destroy(_impactEffectClone,1f);
    }
}