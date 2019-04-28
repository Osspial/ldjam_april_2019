using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBullet : MonoBehaviour
{
    public int damage = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Damageable>() is Damageable d)
        {
            d.takeDamage.Invoke(damage);
        }
        Destroy(gameObject);
    }
}
