using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Rigidbody2D holder;

    public MoveWithRotation bulletTemplate;
    public int bulletsInClip;
    public WeaponData weaponData;

    private float lastShootTime = -1000;
    public void Shoot(Vector2 direction)
    {
        if (bulletsInClip != 0 && Time.time - lastShootTime > weaponData.fireSpeed)
        {
            bulletsInClip -= 1;
            MoveWithRotation bullet = Instantiate(bulletTemplate);
            bullet.speed = weaponData.bulletSpeed;
            bullet.direction = direction;
            lastShootTime = Time.time;
            holder.velocity -= direction * weaponData.knockback;
            bullet.transform.position = transform.position;
        }
    }

    public void Reload()
    {
        IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(weaponData.reloadTime);
            bulletsInClip = weaponData.clipSize;
        }

        StartCoroutine(ReloadCoroutine());
    }
}
