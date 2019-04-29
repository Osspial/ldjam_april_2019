using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health
    {
        get => _health.num;
        set => _health.num = value;
    }
    [SerializeField]
    private IntChange _health = new IntChange(4);
    public IntEvent onHealthChange;

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    void LateUpdate()
    {
        if (_health.changed)
        {
            onHealthChange.Invoke(health);
        }
        _health.Reset();

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
