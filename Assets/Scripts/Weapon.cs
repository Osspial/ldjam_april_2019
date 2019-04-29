using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Shooter))]
public class Weapon : MonoBehaviour
{
    public Shooter shooter;

    public int activeWeaponIndex;
    public bool pulled = false;
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

    public WeaponArrayEntry primaryWeapon
    {
        get => weapons[activeWeaponIndex];
    }
    public WeaponArrayEntry secondaryWeapon
    {
        get
        {
            var secondaryIndex = activeWeaponIndex;
            do
            {
                secondaryIndex += 1;
                secondaryIndex %= weapons.Length;
            } while (!weapons[secondaryIndex].unlocked.b);

            return weapons[secondaryIndex];
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
            pulled = true;
            shooter.Shoot(activeWeapon);
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
                    shooter.source.PlayOneShot(activeWeapon.data.reload);
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
        secondaryWeapon?.onSecondary.Invoke(false);
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

    public void UnlockWeapon(int weapon)
    {
        weapons[weapon].unlocked.b = true;
    }

    void Start()
    {
        foreach (var weapon in weapons)
        {
            weapon.bulletsInClip.num = weapon.data.clipSize;
        }
    }

    void Update()
    {
        if (activeWeapon.data.autoFire && pulled)
        {
            shooter.Shoot(activeWeapon);
        }

        fallable.fall = shooter.liftTime == 0;

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

    void OnDestroy()
    {
        foreach (var weapon in weapons)
        {
            weapon.onPrimary.Invoke(false);
            weapon.onSecondary.Invoke(false);
        }
    }

    void LateUpdate()
    {
        primaryWeapon.onPrimary.Invoke(true);
        secondaryWeapon?.onSecondary.Invoke(true);
    }
}
