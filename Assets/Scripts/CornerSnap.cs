using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CornerSnap : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(
            Mathf.Round(transform.localScale.x),
            Mathf.Round(transform.localScale.y),
            Mathf.Round(transform.localScale.z)
        );
        var corner = transform.position - transform.localScale / 2;
        var cornerRounded = new Vector3(
            Mathf.Round(corner.x),
            Mathf.Round(corner.y),
            Mathf.Round(corner.z)
        );
        transform.position += cornerRounded - corner;
    }
}
