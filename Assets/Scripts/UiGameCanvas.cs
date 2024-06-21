using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiGameCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text gunName;
    [SerializeField] private TMP_Text currentHealth;

    [SerializeField] private WeaponManager gun;
    [SerializeField] private ShooterController playerHealth;


    private void Update()
    {
        int currentAmmo = gun.GetCurrentAmmo();
        int maxAmmo = gun.GetMaxAmmo();
        string name = gun.GetGunName();
        float health = playerHealth.GetCurrentHealth();

        ammoText.text = $"{currentAmmo}/{maxAmmo}";
        gunName.text = name;
        currentHealth.text = $"{(int)health}/100";
    }

}
