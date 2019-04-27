using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float createTime;
    public float lifetime = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        createTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - createTime > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
