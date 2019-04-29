using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagnet : MonoBehaviour
{
    public Rigidbody2D move;
    public float magnetSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnTriggerStay2D(Collider2D p)
    {
        if (p.GetComponent<Player>() is Player pl)
        {
            move.velocity += (pl.transform.position - move.transform.position).xy().normalized * magnetSpeed * Time.deltaTime;
        }
    }
}
