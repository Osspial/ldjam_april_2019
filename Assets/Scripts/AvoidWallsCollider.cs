using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class AvoidWallsCollider : MonoBehaviour
{
    HashSet<Collider2D> closeWalls = new HashSet<Collider2D>();

    new CircleCollider2D collider;

    void Start()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    public void AdjustVelocityAwayFromWalls(ref Vector2 velocity, Vector2 targetDirection, float strength)
    {
        var avoidDirection = Vector2.zero;
        foreach (var wall in closeWalls)
        {
            var closestPoint = wall.ClosestPoint(transform.position);
            var wallDirection = closestPoint - transform.position.xy();
            avoidDirection += wallDirection.normalized * (collider.radius - wallDirection.magnitude);
        }

        velocity -= avoidDirection * strength * (1.0f - (Vector2.Angle(avoidDirection, targetDirection) / 180f));
    }

    void OnTriggerEnter2D(Collider2D wall)
    {
        closeWalls.Add(wall);
    }

    void OnTriggerExit2D(Collider2D wall)
    {
        closeWalls.Remove(wall);
    }
}
