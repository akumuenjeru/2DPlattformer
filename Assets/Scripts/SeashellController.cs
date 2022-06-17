using System.Collections;
using UnityEngine;

public class SeashellController : MonoBehaviour
{
    public BulletController bulletController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            StartCoroutine(PowerUp());
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    IEnumerator PowerUp()
    {
        bulletController.damage = 100;
        Debug.Log("5 seconds start now - bullet damage: " + bulletController.damage);
        yield return new WaitForSeconds(5f);
        bulletController.damage = 50;
        Debug.Log("5 seconds over - bullet damage: " + bulletController.damage);
        Destroy(gameObject);
    }
}