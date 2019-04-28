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
        animator.SetFloat(keyVelocityX, rigidbody.velocity.x);
        animator.SetFloat(keyVelocityY, rigidbody.velocity.y);
    }
}
