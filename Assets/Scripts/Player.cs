using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    Animator animator;
    Health health;

    public float sprintTime = 10;
    public float sprintUseRate = 1;
    public float sprintRecharge = 0.5f;
    public float sprintUsed = 0;
    public float sprintRegain = 8;
    public bool sprintRecovering = false;
    public IntEvent sprintChanged;
    public PlayerControlData walkControlData;
    public PlayerControlData sprintControlData;
    public PlayerControlData slideData;

    private PlayerControlData activeData;
    [Tooltip("gets capped at the length of the maxSpeed curve")]
    public float timeSinceMoveStart = 0.0f;

    public Weapon weapon;

    public MoveWithRotation bulletTemplate;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    Vector2 mousePosition
    {
        get => Camera.main.ViewportToWorldPoint(Input.mousePosition / new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight));
    }

    // Update is called once per frame
    void Update()
    {
        if (sprintUsed >= sprintTime)
        {
            sprintRecovering = true;
        }
        else if (sprintUsed <= sprintRegain)
        {
            sprintRecovering = false;
        }
        if (Input.GetButton("Sprint") && !sprintRecovering)
        {
            sprintUsed += Time.deltaTime * sprintUseRate;
            activeData = sprintControlData;
        }
        else
        {
            sprintUsed -= sprintRecharge * Time.deltaTime;
            activeData = walkControlData;
        }
        sprintUsed = Mathf.Clamp(sprintUsed, 0.0f, sprintTime);

        sprintChanged.Invoke(Mathf.RoundToInt(sprintTime - sprintUsed));

        Vector2 axisDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Vector2 mouseDirection = (mousePosition - transform.position.xy()).normalized;
        Vector2 moveDirection = axisDirection * activeData.moveSpeedMultiplier;

        animator.SetFloat("WeaponRotate", Vector2.SignedAngle(Vector2.right, mouseDirection) / 360f);

        if (axisDirection.magnitude > 0.1)
        {
            rigidbody.drag = activeData.walkingDrag;
        }
        else
        {
            rigidbody.drag = activeData.neutralDrag;
        }

        if (moveDirection.magnitude > 0 || Vector2.Angle(axisDirection, rigidbody.velocity) >= 90)
        {
            timeSinceMoveStart += Time.deltaTime;
            timeSinceMoveStart = Mathf.Min(timeSinceMoveStart, activeData.maxSpeedCurve.keys.Last().time);
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
            health.health -= weapon.Reload();
        }

        var maxSpeed = activeData.maxSpeedCurve.Evaluate(timeSinceMoveStart);
        if (moveDirection.magnitude > 0 && Vector2.Dot(rigidbody.velocity, moveDirection) > 0)
        {
            // if (rigidbody.velocity.magnitude > maxSpeed)
            // {
            //     rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
            // }
        }

        var lookDirection = mouseDirection;
        animator.SetFloat("velX", Mathf.Round(Mathf.Clamp(lookDirection.x, -1, 1)));
        animator.SetFloat("velY", Mathf.Round(Mathf.Clamp(lookDirection.y, -1, 1)));
        animator.SetFloat("speed", Mathf.Lerp(0.2f, 1.0f, normalizedSpeed));
    }

    private float normalizedSpeed
    {
        get => rigidbody.velocity.magnitude / sprintControlData.maxSpeedCurve.keys.Last().value;
    }
}
