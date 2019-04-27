using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    public int clipSize = 6;
    public bool autoFire = false;
    public float fireSpeed = 0;
    public float bulletSpeed = 30;
    public float knockback = 4;
    public float reloadTime = 0.4f;
    public int reloadCost = 1;
}
