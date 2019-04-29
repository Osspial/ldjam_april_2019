using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTempHealth : MonoBehaviour
{
    public int addHealth = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() is Player p)
        {
            p.GetComponent<Health>().tempHealth.num += addHealth;
            Destroy(gameObject);
        }
    }
}
