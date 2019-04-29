using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivateOnSeePlayer : MonoBehaviour
{
    public bool inNoticeTrigger = false;
    public BoolEvent isVisible;
    Player player;

    private bool lastVisibleInvokeVal = false;
    public LayerMask viewLayerMask;
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            if (PathfindController.instance.Passable(transform.position, player.transform.position, viewLayerMask))
            {
                if (inNoticeTrigger)
                {
                    lastVisibleInvokeVal = true;
                    isVisible.Invoke(true);
                }
            }
            else if (lastVisibleInvokeVal)
            {
                lastVisibleInvokeVal = false;
                isVisible.Invoke(false);
            }
        }
    }
}
