using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
	
    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot ()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}