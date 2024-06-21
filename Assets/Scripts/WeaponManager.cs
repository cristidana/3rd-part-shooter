using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [System.Serializable]
    public class Weapon
    {
        public GunData gunData;
        public Transform muzzlePosition;
        public AudioSource audioSource;
        public GameObject weaponObject;
        public int currentAmmo;
    }

    [SerializeField] private List<Weapon> weapons;
    private int activeWeaponIndex = 0;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InitializeWeapons();
        ActivateWeapon(activeWeaponIndex);
    }

    private void Update()
    {
        HandleWeaponSwitch();
    }

    private void InitializeWeapons()
    {
        foreach (var weapon in weapons)
        {
            weapon.currentAmmo = weapon.gunData.ammo;
            weapon.weaponObject.SetActive(false);
        }
    }

    private void ActivateWeapon(int index)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].weaponObject.SetActive(i == index);
        }
    }

    private void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeWeaponIndex = 0;
            ActivateWeapon(activeWeaponIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Count > 1)
        {
            activeWeaponIndex = 1;
            ActivateWeapon(activeWeaponIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && weapons.Count > 2)
        {
            activeWeaponIndex = 2;
            ActivateWeapon(activeWeaponIndex);
        }
    }

    public void Shoot(RaycastHit rayHit)
    {
        Weapon activeWeapon = weapons[activeWeaponIndex];

        if (activeWeapon.currentAmmo <= 0)
        {
            return;
        }

        activeWeapon.currentAmmo--;

        if (activeWeapon.gunData.muzzlePrefab != null)
        {
            Instantiate(activeWeapon.gunData.muzzlePrefab, activeWeapon.muzzlePosition.position, activeWeapon.muzzlePosition.rotation);
        }

        if (activeWeapon.gunData.shotSound != null && activeWeapon.audioSource != null)
        {
            activeWeapon.audioSource.PlayOneShot(activeWeapon.gunData.shotSound);
        }

        if (rayHit.transform != null && rayHit.transform.CompareTag("Enemy"))
        {
            rayHit.transform.gameObject.GetComponent<EnemyController>().TakeDamage(activeWeapon.gunData.damage);
        }
    }

    public int GetCurrentAmmo()
    {
        return weapons[activeWeaponIndex].currentAmmo;
    }

    public int GetMaxAmmo()
    {
        return weapons[activeWeaponIndex].gunData.ammo;
    }

    public string GetGunName()
    {
        return weapons[activeWeaponIndex].gunData.gunName;
    }
}
