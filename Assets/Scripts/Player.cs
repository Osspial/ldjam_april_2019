using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    Rigidbody2D rigidbody;

    public float moveSpeedMultiplier = 10.0f;
    [Tooltip("max speed in relation to time since movement start")]
    public AnimationCurve maxSpeedCurve;
    public AnimationCurve timeSubtractAngleTurn;
    [Tooltip("gets capped at the length of the maxSpeed curve")]
    public float timeSinceMoveStart = 0.0f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * moveSpeedMultiplier;
        Vector2 mousePos = Camera.main.ViewportToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = (mousePos - transform.position.xy()).normalized;

        if (moveDirection.magnitude > 0)
        {
            timeSinceMoveStart += Time.deltaTime;
            timeSinceMoveStart = Mathf.Min(timeSinceMoveStart, maxSpeedCurve.keys[maxSpeedCurve.keys.Length - 1].time);
        }
        else
        {
            timeSinceMoveStart = 0;
        }
        rigidbody.velocity += moveDirection * Time.deltaTime;

        var maxSpeed = maxSpeedCurve.Evaluate(timeSinceMoveStart);
        if (moveDirection.magnitude > 0 && Vector2.Dot(rigidbody.velocity, moveDirection) > 0)
        {
            if (rigidbody.velocity.magnitude > maxSpeed)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
            }
        }
    }
}
