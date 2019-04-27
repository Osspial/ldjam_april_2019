using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    Animator animator;

    public PlayerControlData controlData;
    public PlayerControlData slideData;

    private PlayerControlData activeData;
    [Tooltip("max speed in relation to time since movement start")]
    public AnimationCurve maxSpeedCurve;
    public AnimationCurve timeSubtractAngleTurn;
    [Tooltip("gets capped at the length of the maxSpeed curve")]
    public float timeSinceMoveStart = 0.0f;

    public IntChange health = new IntChange(4);
    public IntEvent onHealthChange;

    public Weapon weapon;

    public MoveWithRotation bulletTemplate;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    Vector2 mousePosition
    {
        get => Camera.main.ViewportToWorldPoint(Input.mousePosition / new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight));
    }

    // Update is called once per frame
    void Update()
    {
        if (health.num == 0)
        {
            Destroy(gameObject);
        }

        if (Input.GetButton("Slide"))
        {
            activeData = slideData;
        }
        else
        {
            activeData = controlData;
        }

        Vector2 axisDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Vector2 mouseDirection = (mousePosition - transform.position.xy()).normalized;
        Vector2 moveDirection = axisDirection * activeData.moveSpeedMultiplier;
        rigidbody.drag = activeData.linearDrag;

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

        weapon.direction = mouseDirection;
        if (Input.GetButtonDown("Shoot"))
        {
            weapon.PullTrigger();
        }
        if (Input.GetButtonUp("Shoot"))
        {
            weapon.ReleaseTrigger();
        }
        if (Input.GetButtonDown("Reload"))
        {
            health.num -= weapon.Reload();
        }

        var maxSpeed = maxSpeedCurve.Evaluate(timeSinceMoveStart);
        if (moveDirection.magnitude > 0 && Vector2.Dot(rigidbody.velocity, moveDirection) > 0)
        {
            // if (rigidbody.velocity.magnitude > maxSpeed)
            // {
            //     rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
            // }
        }

        if (health.changed)
        {
            onHealthChange.Invoke(health.num);
        }

        health.Reset();
        var lookDirection = rigidbody.velocity;
        if (weapon.pulled)
        {
            lookDirection = mouseDirection;
        }
        animator.SetFloat("velX", Mathf.Round(Mathf.Clamp(lookDirection.x, -1, 1)));
        animator.SetFloat("velY", Mathf.Round(Mathf.Clamp(lookDirection.y, -1, 1)));
    }
}
