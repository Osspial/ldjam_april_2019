using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtPlayer : MonoBehaviour
{
    Player player;
    public Weapon weapon;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        float hitTime = (player.transform.position - weapon.shooter.transform.position).magnitude / weapon.activeWeapon.data.bulletSpeed;
        Vector2 target = player.transform.position.xy() + player.rigidbody.velocity * hitTime;
        weapon.shooter.direction = (target - weapon.shooter.transform.position.xy()).normalized;
        animator.SetFloat("WeaponRotate", Vector2.SignedAngle(Vector2.right, weapon.shooter.direction) / 360f);
        weapon.PullTrigger();
    }
}
