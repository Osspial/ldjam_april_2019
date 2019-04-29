using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class HealIfNoAggro : MonoBehaviour
{
    public float healthPer = 0.5f;
    public float lastAdd = -1000;

    private Health health;

    void Start()
    {
        health = GetComponent<Health>();
    }

    void Update()
    {
        if (AggroTracker.instance.aggros.Count == 0 && Time.time - lastAdd >= healthPer)
        {
            health.health.num += 1;
            lastAdd = Time.time;
        }
    }
}
