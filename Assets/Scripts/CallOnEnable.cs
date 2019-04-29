using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CallOnEnable : MonoBehaviour
{
    public UnityEvent onEnable;

    void OnEnable()
    {
        onEnable.Invoke();
    }
}
