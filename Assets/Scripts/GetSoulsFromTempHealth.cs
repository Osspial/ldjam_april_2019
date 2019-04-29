using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class GetSoulsFromTempHealth : MonoBehaviour
{
    public int souls = 0;
    public IntEvent onSoulsChanged;

    private Health health;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AggroTracker.instance.aggros.Count == 0)
        {
            souls += health.tempHealth.num / 2;
            health.tempHealth.num = 0;
        }
        onSoulsChanged.Invoke(souls);
    }
}
