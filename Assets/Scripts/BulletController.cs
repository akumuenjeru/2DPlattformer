using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 20f;
    public int damage = 50;
    public GameObject impactEffect;
    private GameObject impactEffectClone;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        SimpleEnemyController enemy = hitInfo.GetComponent<SimpleEnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        impactEffectClone = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
       
    }

    private void OnDestroy()
    {
        Destroy(impactEffectClone,1f);
    }
}