using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeUnityEvent : MonoBehaviour
{
    public UnityEvent e;

    public void Invoke()
    {
        e.Invoke();
    }
}
