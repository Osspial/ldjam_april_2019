using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Rigidbody2D holder;

    public MoveWithRotation bulletTemplate;
    public IntChange bulletsInClip;
    public IntEvent onAmmoChange;
    public WeaponData weaponData;

    private float lastShootTime = -1000;
    public void Shoot(Vector2 direction)
    {
        if (bulletsInClip.num != 0 && Time.time - lastShootTime > weaponData.fireSpeed)
        {
            bulletsInClip.num -= 1;
            MoveWithRotation bullet = Instantiate(bulletTemplate);
            bullet.speed = weaponData.bulletSpeed;
            bullet.direction = direction;
            lastShootTime = Time.time;
            holder.velocity -= direction * weaponData.knockback;
            bullet.transform.position = transform.position;
        }
    }

    private bool reloading = false;
    public int Reload()
    {
        IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(weaponData.reloadTime);
            bulletsInClip.num = weaponData.clipSize;
            reloading = false;
        }

        if (reloading == false)
        {
            reloading = true;
            StartCoroutine(ReloadCoroutine());
            return weaponData.reloadCost;
        }
        else
        {
            return 0;
        }
    }

    void Start()
    {
        bulletsInClip.num = weaponData.clipSize;
    }

    void Update()
    {
        if (bulletsInClip.changed)
        {
            onAmmoChange.Invoke(bulletsInClip.num);
        }

        bulletsInClip.Reset();
    }
}
