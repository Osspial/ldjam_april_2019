using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerPlayerEnterAggro : MonoBehaviour
{
    bool inTrigger = false;
    public UnityEvent onEnter;
    public UnityEvent onExit;

    bool entered = false;
    void Update()
    {
        if (inTrigger && AggroTracker.instance.aggros.Count != 0 && !entered)
        {
            onEnter.Invoke();
            entered = true;
        }
        else if ((AggroTracker.instance.aggros.Count == 0) && entered)
        {
            entered = false;
            onExit.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        inTrigger = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        inTrigger = false;
    }
}
