using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData gunData; 
    [SerializeField] private Transform muzzlePosition;
    [SerializeField] private AudioSource audioSource;

    private Transform player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    public void EnemyShoot()
    {

        if (gunData.muzzlePrefab != null)
        {
            Instantiate(gunData.muzzlePrefab, muzzlePosition.position, muzzlePosition.rotation);
        }


        if (gunData.shotSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(gunData.shotSound);
        }

        player.transform.gameObject.GetComponent<ShooterController>().TakeDamage(10.0f);


    }

}
