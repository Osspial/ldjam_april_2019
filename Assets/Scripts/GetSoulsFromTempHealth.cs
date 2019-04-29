using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class GetSoulsFromTempHealth : MonoBehaviour
{
    public int souls = 0;
    public IntEvent onSoulsChanged;
    public float rate = 2.0f;

    private Health health;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
    }

    float lastTake = 0;
    // Update is called once per frame
    void Update()
    {
        if (AggroTracker.instance.aggros.Count == 0 && Time.time - lastTake > rate && health.tempHealth.num > 0)
        {
            health.tempHealth.num -= 2;
            if (health.tempHealth.num < 0)
            {
                health.tempHealth.num = 0;
            }
            else
            {
                souls += 1;
            }
            lastTake = Time.time;
        }
        onSoulsChanged.Invoke(souls);
    }
}
