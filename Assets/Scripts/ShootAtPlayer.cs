using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtPlayer : MonoBehaviour
{
    Player player;
    public Weapon.WeaponArrayEntry weapon;
    public Shooter shooter;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        float hitTime = (player.transform.position - shooter.transform.position).magnitude / weapon.data.bulletSpeed;
        Vector2 target = player.transform.position.xy() + player.rigidbody.velocity * hitTime;
        shooter.direction = (target - shooter.transform.position.xy()).normalized;
        shooter.Shoot(weapon);
    }
}
