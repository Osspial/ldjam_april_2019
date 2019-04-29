using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    public Rigidbody2D holder;

    public MoveWithRotation bulletTemplate;
    public Vector2 direction;
    public int activeWeaponIndex;
    public bool pulled = false;
    public WeaponArrayEntry[] weapons;

    [Serializable]
    public class WeaponArrayEntry
    {
        public WeaponData data;
        public IntChange bulletsInClip;
        public BoolChange unlocked = new BoolChange(false);
        public IntEvent onAmmoChange;
        public UnityEvent onUnlock;
    }

    private AudioSource source;

    private float lastShootTime = -1000;

    public WeaponArrayEntry activeWeapon
    {
        get => weapons[activeWeaponIndex];
    }

    public void PullTrigger()
    {
        if (activeWeapon.bulletsInClip.num != 0 && Time.time - lastShootTime > activeWeapon.data.fireSpeed)
        {
            activeWeapon.bulletsInClip.num -= 1;
            lastShootTime = Time.time;
            for (int i = 0; i < activeWeapon.data.bulletsPerShot; i++)
            {
                var angle = Mathf.Lerp(-activeWeapon.data.bulletSpread, activeWeapon.data.bulletSpread, ((float)i / activeWeapon.data.bulletsPerShot));
                MoveWithRotation bullet = Instantiate(bulletTemplate);
                bullet.speed = activeWeapon.data.bulletSpeed;
                bullet.direction = Quaternion.Euler(0, 0, angle) * direction;
                bullet.transform.position = transform.position;
                bullet.GetComponent<TriggerBullet>().damage = activeWeapon.data.damage;
            }
            if (activeWeapon.data.zeroBaseSpeed)
            {
                holder.velocity = Vector2.zero;
            }
            holder.velocity -= direction * activeWeapon.data.knockback;
            pulled = true;
            if (activeWeapon.data.sound.Length > 0)
            {
                source.PlayOneShot(activeWeapon.data.sound[Random.Range(0, activeWeapon.data.sound.Length - 1)]);
            }
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
            if (activeWeapon.data.reload)
            {
                source.PlayOneShot(activeWeapon.data.reload);
            }
            yield return new WaitForSeconds(activeWeapon.data.reloadTime);
            activeWeapon.bulletsInClip.num = activeWeapon.data.clipSize;
            reloading = false;
        }

        if (reloading == false && activeWeapon.bulletsInClip.num < activeWeapon.data.clipSize)
        {
            reloading = true;
            StartCoroutine(ReloadCoroutine());
            return activeWeapon.data.reloadCost;
        }
        else
        {
            return 0;
        }
    }

    public WeaponData SwitchWeapon(int delta)
    {
        do
        {
            activeWeaponIndex += delta;
            activeWeaponIndex %= weapons.Length;
            if (activeWeaponIndex < 0)
            {
                activeWeaponIndex = weapons.Length - 1;
            }
        } while (!activeWeapon.unlocked.b);
        return activeWeapon.data;
    }

    void Start()
    {
        foreach (var weapon in weapons)
        {
            weapon.bulletsInClip.num = weapon.data.clipSize;
        }
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (activeWeapon.data.autoFire && pulled)
        {
            PullTrigger();
        }

        foreach (var weapon in weapons)
        {
            if (weapon.bulletsInClip.changed)
            {
                weapon.onAmmoChange.Invoke(weapon.bulletsInClip.num);
            }
            weapon.bulletsInClip.Reset();
            if (weapon.unlocked.changed && weapon.unlocked.b)
            {
                weapon.onUnlock.Invoke();
            }
            weapon.unlocked.Reset();
        }
    }
}
