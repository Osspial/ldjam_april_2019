using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    public int clipSize = 6;
    public bool autoFire = false;
    public float fireSpeed = 0;
    public float bulletSpeed = 30;
    public float knockback = 4;
    public bool zeroBaseSpeed = false;
    public float liftTime = 0;
    public float liftTimeAdditional = 0;
    public float reloadTime = 0.4f;
    public int reloadCost = 1;
    public int bulletsPerReload = 0;
    public int damage = 1;
    public float bulletSpread = 0;
    public int bulletsPerShot = 1;
    public AudioClip[] sound;
    public AudioClip reload;
    public AudioClip empty;
}
