using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ActivateOnSeePlayer>() is ActivateOnSeePlayer a)
        {
            a.inNoticeTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<ActivateOnSeePlayer>() is ActivateOnSeePlayer a)
        {
            a.inNoticeTrigger = false;
        }
    }
}
