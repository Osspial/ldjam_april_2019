using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextSlowlyAppear : MonoBehaviour
{
    public float charPer = 0.05f;
    [TextArea]
    public string targetText;
    public Text text;
    public Animator hearse;
    public BoolEvent doneAppearingEvent;
    private bool doneAppearingEventLastInvoke = false;

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
            if (!doneAppearing)
            {
                hearse.SetFloat("TalkState", 1);
            }
        }
        else if (doneAppearing)
        {
            hearse.SetFloat("TalkState", 0.5f);
        }

        if (doneAppearingEventLastInvoke != doneAppearing)
        {
            doneAppearingEventLastInvoke = doneAppearing;
            doneAppearingEvent.Invoke(doneAppearing);
        }
    }

    public bool doneAppearing
    {
        get => targetText.Length == text.text.Length;
    }
}
