using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChasm : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<Fallable>() is Fallable f)
        {
            if (f.fall)
            {
                f.damageable.takeDamage.Invoke(99999999);
            }
        }
    }
}
