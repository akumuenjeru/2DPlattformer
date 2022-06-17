using System;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleEnemyController : MonoBehaviour
{
    public int health = 100;
    public GameObject deathEffect;
    private GameObject _deathEffectClone;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _deathEffectClone = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        
    }

    private void OnDestroy()
    {

           Destroy(_deathEffectClone, 0.5f);
    }
}