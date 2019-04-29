using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public MoveWithRotation bulletTemplate;
    public Vector2 direction;
    public float lastShootTime;
    public Rigidbody2D holder;
    public float liftTime = 0;
    public AudioSource source;
    public void Shoot(Weapon.WeaponArrayEntry activeWeapon)
    {
        if (Time.time - lastShootTime > activeWeapon.data.fireSpeed)
        {
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
                if (holder)
                {
                    if (activeWeapon.data.zeroBaseSpeed)
                    {
                        holder.velocity = Vector2.zero;
                    }
                    holder.velocity -= direction * activeWeapon.data.knockback;
                }
                liftTime += activeWeapon.data.liftTime;
                if (liftTime > 0)
                {
                    liftTime += activeWeapon.data.liftTimeAdditional;
                }
                if (activeWeapon.data.sound.Length > 0 && source != null)
                {
                    source.PlayOneShot(activeWeapon.data.sound[Random.Range(0, activeWeapon.data.sound.Length - 1)]);
                }
            }
            else if (source != null)
            {
                source.PlayOneShot(activeWeapon.data.empty);
            }
        }
    }

    void Update()
    {
        liftTime -= Time.deltaTime;
        if (liftTime < 0)
        {
            liftTime = 0;
        }
    }
}
