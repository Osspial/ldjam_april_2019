using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent e;
    public UnityEvent exit;

    void OnTriggerEnter2D(Collider2D c)
    {
        e.Invoke();
    }

    void OnTriggerExit2D(Collider2D c)
    {
        exit.Invoke();
    }
}
