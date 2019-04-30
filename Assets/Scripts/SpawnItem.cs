using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public GameObject spawn;
    public GameObject parent;

    public void Spawn()
    {
        Instantiate(spawn);
        spawn.transform.position = transform.position;
        spawn.transform.parent = parent.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
