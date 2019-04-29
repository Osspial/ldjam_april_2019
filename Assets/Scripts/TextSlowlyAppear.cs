using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextSlowlyAppear : MonoBehaviour
{
    public float charPer = 0.05f;
    [TextArea]
    public string targetText;
    public Text text;
    public Animator hearse;
    public UnityEvent appearingEvent;
    public UnityEvent doneAppearingEvent;
    private bool? doneAppearingEventLastInvoke = null;

    public void Clear()
    {
        text.text = "";
    }

    private float lastAppear = -1000;
    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastAppear >= charPer)
        {
            text.text = targetText.Substring(0, Math.Min(targetText.Length, text.text.Length + 1));
            lastAppear = Time.time;
        }

        if (doneAppearingEventLastInvoke != doneAppearing)
        {
            doneAppearingEventLastInvoke = doneAppearing;
            if (doneAppearing)
            {
                doneAppearingEvent.Invoke();
            }
            else
            {
                appearingEvent.Invoke();
            }
        }
    }

    public bool doneAppearing
    {
        get => targetText.Length == text.text.Length;
    }
}
