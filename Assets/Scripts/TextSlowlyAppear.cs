using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextSlowlyAppear : MonoBehaviour
{
    public float charPer = 0.05f;
    public string targetText;
    Text text;

    void Start()
    {
        text = GetComponent<Text>();
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
    }
}
