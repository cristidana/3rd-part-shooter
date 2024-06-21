using UnityEngine;

[CreateAssetMenu(fileName = "NewGunData", menuName = "ScriptableObjects/GunData", order = 2)]
public class GunData : ScriptableObject
{
    public string gunName;
    public GameObject muzzlePrefab;
    public AudioClip shotSound;
    public float damage;
    public int ammo;
}
