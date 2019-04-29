using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int totalHealth
    {
        get => health.num + tempHealth.num;
    }
    public IntChange health = new IntChange(4);
    public int maxHealth = 99999;
    public IntChange tempHealth = new IntChange(0);
    public IntEvent onHealthChange;
    public IntEvent onTempHealthChange;
    public UnityEvent onDie;
    public GameObject create;
    public bool reloadOnKill;

    public void TakeDamage(int damage)
    {
        if (tempHealth.num > 0)
        {
            tempHealth.num -= damage;
            if (tempHealth.num < 0)
            {
                health.num += tempHealth.num;
            }
        }
        else
        {
            health.num -= damage;
        }
    }

    void LateUpdate()
    {
        if (health.changed)
        {
            if (health.num > maxHealth)
            {
                health.num = maxHealth;
            }
            onHealthChange.Invoke(health.num);
        }
        health.Reset();
        if (tempHealth.changed)
        {
            onTempHealthChange.Invoke(tempHealth.num);
        }
        tempHealth.Reset();

        if (totalHealth <= 0)
        {
            if (create)
            {
                var c = Instantiate(create);
                c.transform.parent = transform.parent;
                c.transform.position = transform.position;
            }
            onDie.Invoke();
            Destroy(gameObject);
            if (reloadOnKill)
            {
                FindObjectOfType<CheckpointController>().Load();
            }
        }
    }
}
