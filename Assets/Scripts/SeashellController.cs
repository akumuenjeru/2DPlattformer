using System.Collections;
using UnityEngine;

public class SeashellController : MonoBehaviour
{
    public BulletController bulletController;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            bulletController.damage = 100;
            Destroy(gameObject);
            
            RunDelayed(3f,() => bulletController.damage = 50);
        }

    
    }


    private IEnumerator DelayedCoroutine(float delay, System.Action a)
    {
        yield return new WaitForSeconds(delay);
        a();
    }

    private Coroutine RunDelayed(float delay, System.Action a)
    {
        return StartCoroutine(DelayedCoroutine(delay, a));
    }
}