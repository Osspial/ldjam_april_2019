using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnSeePlayer : MonoBehaviour
{
    public BoolEvent activate;
    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PathfindController.instance.Passable(transform.position, player.transform.position))
        {
            activate.Invoke(true);
        }
    }
}
