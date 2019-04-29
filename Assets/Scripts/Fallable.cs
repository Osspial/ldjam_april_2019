using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Fallable : MonoBehaviour
{
    public bool fall = true;

    [HideInInspector]
    public Damageable damageable;
    public Collider2D collider;

    void Start()
    {
        damageable = GetComponent<Damageable>();
    }

    void Update()
    {
        if (fall)
        {
            var contactFilter = new ContactFilter2D();
            contactFilter.useTriggers = true;
            var colliders = new List<Collider2D>();
            collider.OverlapCollider(contactFilter, colliders);
            foreach (var c in colliders)
            {
                if (c.GetComponent<Floor>())
                {
                    return;
                }
            }
            damageable.takeDamage.Invoke(999999);
        }
    }
}
