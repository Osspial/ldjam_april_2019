using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMelee : MonoBehaviour
{
    public int damage = 1;
    public Animator animator;
    public string animationTrigger;
    public float cooldown = 1;
    private float lastMeleeTime = -1000;
    public float knockback = 1.0f;

    private HashSet<Damageable> inTrigger = new HashSet<Damageable>();
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.GetComponent<Damageable>() is Damageable d)
        {
            inTrigger.Add(d);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Damageable>() is Damageable d)
        {
            inTrigger.Remove(d);
        }
    }

    void Update()
    {
        if (inTrigger.Count > 0 && (Time.time - lastMeleeTime) >= cooldown)
        {
            animator.SetTrigger(animationTrigger);
            foreach (var d in inTrigger)
            {
                d.takeDamage.Invoke(damage);
                if (d.GetComponent<Rigidbody2D>() is Rigidbody2D r)
                {
                    r.velocity += (r.transform.position - transform.position).xy().normalized * knockback;
                }
            }
            lastMeleeTime = Time.time;
        }
    }
}
