using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLag : MonoBehaviour
{
    public Transform target;
    public Vector2 realPosition;
    public float maxDistance = 5.0f;
    public float snapToMaxDistance = 0.1f;
    public float dampTime = 0.1f;

    public Vector2 velocity = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        var damped = Vector2.SmoothDamp(realPosition, target.position, ref velocity, dampTime);
        if ((damped - target.position.xy()).magnitude > maxDistance)
        {
            var newDamped = target.position.xy() + (damped - target.position.xy()).normalized * maxDistance;
            damped = newDamped;
        }
        realPosition = new Vector3(damped.x, damped.y, transform.position.z);

        var setPosition = realPosition;
        if ((maxDistance - (realPosition - target.position.xy()).magnitude) < snapToMaxDistance)
        {
            setPosition = target.position.xy() + (damped - target.position.xy()).normalized * maxDistance;
        }
        transform.position = new Vector3(setPosition.x, setPosition.y, transform.position.z);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(realPosition, 0.1f);
    }
}
