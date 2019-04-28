using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class AnimationVelocity : MonoBehaviour
{
    Animator animator;
    new Rigidbody2D rigidbody;
    public string keyVelocityX = "VelocityX";
    public string keyVelocityY = "VelocityY";
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(keyVelocityX, Mathf.Round(Mathf.Clamp(rigidbody.velocity.x, -1, 1)));
        animator.SetFloat(keyVelocityY, Mathf.Round(Mathf.Clamp(rigidbody.velocity.y, -1, 1)));
    }
}
