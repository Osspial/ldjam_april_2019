using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    public Rigidbody2D holder;

    public MoveWithRotation bulletTemplate;
    public IntChange bulletsInClip;
    public IntEvent onAmmoChange;
    public WeaponData weaponData;
    public Vector2 direction;

    private AudioSource source;

    private float lastShootTime = -1000;
    public bool pulled = false;
    public void PullTrigger()
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
            bullet.GetComponent<TriggerBullet>().damage = weaponData.damage;
            pulled = true;
            source.PlayOneShot(weaponData.sound[Random.Range(0, weaponData.sound.Length - 1)]);
        }
    }

    public void ReleaseTrigger()
    {
        pulled = false;
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
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (weaponData.autoFire && pulled)
        {
            PullTrigger();
        }

        if (bulletsInClip.changed)
        {
            onAmmoChange.Invoke(bulletsInClip.num);
        }
        bulletsInClip.Reset();
    }
}
