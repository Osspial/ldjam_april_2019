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
    public float liftTime = 0;
    public WeaponArrayEntry[] weapons;
    public Fallable fallable;

    [Serializable]
    public class WeaponArrayEntry
    {
        public WeaponData data;
        public IntChange bulletsInClip;
        public BoolChange unlocked = new BoolChange(false);
        public IntEvent onAmmoChange;
        public UnityEvent onUnlock;
        public BoolEvent onPrimary;
        public BoolEvent onSecondary;
    }

    private AudioSource source;

    private float lastShootTime = -1000;

    public WeaponArrayEntry primaryWeapon
    {
        get => weapons[activeWeaponIndex];
    }
    public WeaponArrayEntry secondaryWeapon
    {
        get
        {
            var w = weapons[(activeWeaponIndex + 1) % weapons.Length];
            if (w.unlocked.b)
            {
                return w;
            }
            else
            {
                return null;
            }
        }
    }
    public WeaponArrayEntry activeWeapon
    {
        get
        {
            switch (activeSlot)
            {
                case Slot.Primary:
                    return primaryWeapon;
                case Slot.Secondary:
                    return secondaryWeapon;
            }
            throw new InvalidOperationException("Unreachable");
        }
    }
    public Slot activeSlot = Slot.Primary;

    public enum Slot
    {
        Primary,
        Secondary,
    }
    public void PullTrigger()
    {
        if (!pulled)
        {
            Shoot();
        }
    }
    public void Shoot()
    {
        if (Time.time - lastShootTime > activeWeapon.data.fireSpeed)
        {
            pulled = true;
            lastShootTime = Time.time;
            if (activeWeapon.bulletsInClip.num != 0)
            {
                activeWeapon.bulletsInClip.num -= 1;
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
                liftTime += activeWeapon.data.liftTime;
                if (liftTime > 0)
                {
                    liftTime += activeWeapon.data.liftTimeAdditional;
                }
                if (activeWeapon.data.sound.Length > 0)
                {
                    source.PlayOneShot(activeWeapon.data.sound[Random.Range(0, activeWeapon.data.sound.Length - 1)]);
                }
            }
            else
            {
                source.PlayOneShot(activeWeapon.data.empty);
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
        if (!pulled)
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
        }
        return 0;
    }

    public void SwitchWeapon(int delta)
    {
        primaryWeapon.onPrimary.Invoke(false);
        secondaryWeapon.onSecondary.Invoke(false);
        activeSlot = Slot.Primary;
        if (!pulled)
        {
            do
            {
                activeWeaponIndex += delta;
                activeWeaponIndex %= weapons.Length;
                if (activeWeaponIndex < 0)
                {
                    activeWeaponIndex = weapons.Length - 1;
                }
            } while (!primaryWeapon.unlocked.b);
        }
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
            Shoot();
        }

        fallable.fall = liftTime == 0;
        liftTime -= Time.deltaTime;
        if (liftTime < 0)
        {
            liftTime = 0;
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

    void LateUpdate()
    {
        primaryWeapon.onPrimary.Invoke(true);
        secondaryWeapon?.onSecondary.Invoke(true);
    }
}
