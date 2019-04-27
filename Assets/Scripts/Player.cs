using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    Rigidbody2D rigidbody;

    public float moveSpeedMultiplier = 10.0f;
    public float shootKnockback = 1.0f;
    [Tooltip("max speed in relation to time since movement start")]
    public AnimationCurve maxSpeedCurve;
    public AnimationCurve timeSubtractAngleTurn;
    [Tooltip("gets capped at the length of the maxSpeed curve")]
    public float timeSinceMoveStart = 0.0f;

    public Weapon weapon;

    public MoveWithRotation bulletTemplate;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    Vector2 mousePosition
    {
        get => Camera.main.ViewportToWorldPoint(Input.mousePosition / new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight));
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 axisDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Vector2 mouseDirection = (mousePosition - transform.position.xy()).normalized;
        Vector2 moveDirection = axisDirection * moveSpeedMultiplier;

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

        if (Input.GetButtonDown("Shoot"))
        {
            weapon.Shoot(mouseDirection);
        }
        if (Input.GetButtonDown("Reload"))
        {
            weapon.Reload();
        }

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
