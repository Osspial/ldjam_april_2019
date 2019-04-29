using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePlayer : MonoBehaviour
{
    public Vector2Event trackPos;
    public float radius = 5.0f;
    public float circleSpeed = 30.0f;
    public float angle = 0.0f;
    public LayerMask floorLayers;

    private Player player;

    void OnEnable()
    {
        player = FindObjectOfType<Player>();
        angle = Vector2.Angle(player.transform.position - transform.position, Vector2.right);
    }

    private Vector2 playerPosition;
    void Update()
    {
        angle += circleSpeed * Time.deltaTime;
        if (player != null)
        {
            playerPosition = player.transform.position;
        }
        var pos = transform.position;
        var activeRadius = radius;

        var overlaps = new List<Collider2D>();
        var contactFilter = new ContactFilter2D();
        contactFilter.layerMask = floorLayers;
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = true;

        var limit = 100;
        do
        {
            overlaps.Clear();
            pos = playerPosition + (Quaternion.Euler(0, 0, angle) * Vector2.right).xy() * activeRadius;
            activeRadius *= .95f;
            limit -= 1;
        } while (Physics2D.OverlapPoint(pos, contactFilter, overlaps) == 0 && limit != 0);

        trackPos.Invoke(pos);
    }
}
