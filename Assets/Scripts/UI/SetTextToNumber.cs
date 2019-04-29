using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SetTextToNumber : MonoBehaviour
{
    private Text text;
    public string prefix = "";
    public string suffix = "";
    public int number
    {
        set
        {
            if (text == null)
            {
                text = GetComponent<Text>();
            }
            text.text = prefix + value.ToString() + suffix;
        }
    }
}
