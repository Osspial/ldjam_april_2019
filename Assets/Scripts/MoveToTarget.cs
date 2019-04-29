using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveToTarget : MonoBehaviour
{
    [SerializeField]
    private Vector2 Target;
    public Vector2 target
    {
        get => Target;
        set => Target = value;
    }
    public float acceleration = 1;
    public float deceleration = 2;
    public float maxSpeed = 2;
    public AvoidWallsCollider avoidWallsCollider;
    public float avoidStrength = 1;
    public Vector3Event facingDirection;
    public LayerMask wallLayerMask;
    public LayerMask floorLayerMask;
    public LayerMask viewLayerMask;

    new Rigidbody2D rigidbody;
    List<Vector2> path = null;

    private Vector2? activeTarget;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position.xy(), target) >= 0.1)
        {
            path = PathfindController.instance.Search(transform.position.xy(), target, wallLayerMask, floorLayerMask);
        }

        if (path != null)
        {
            if (path.Count > 1)
            {
                if (PathfindController.instance.Passable(transform.position, path[Math.Max(path.Count - 2, 0)], viewLayerMask, new LayerMask()))
                {
                    path.RemoveAt(path.Count - 1);
                }
                else if (path.Count > 0)
                {
                    activeTarget = path.Last() - transform.position.xy();
                }
            }
            else if (path.Count == 1 && Vector2.Distance(transform.position, path.Last()) >= 0.1)
            {
                activeTarget = path.Last() - transform.position.xy();
            }
            else
            {
                activeTarget = null;
                path = null;
            }
        }

        var idealVelocity = (activeTarget?.normalized ?? Vector2.zero) * maxSpeed;
        var delta = (idealVelocity - rigidbody.velocity).normalized * acceleration * Time.deltaTime;
        rigidbody.velocity += delta;
        rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, maxSpeed);
        facingDirection.Invoke(new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, rigidbody.velocity.normalized)));

        var velocity = rigidbody.velocity;
        avoidWallsCollider?.AdjustVelocityAwayFromWalls(ref velocity, (activeTarget - transform.position) ?? Vector2.zero, avoidStrength);
        rigidbody.velocity = velocity;
    }

    void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = 0; i + 1 < path.Count; i++)
            {
                Gizmos.DrawLine(path[i], path[i + 1]);
            }
            if (path.Count == 1)
            {
                Gizmos.DrawLine(transform.position.xy(), path[0]);
            }
        }
    }
}
