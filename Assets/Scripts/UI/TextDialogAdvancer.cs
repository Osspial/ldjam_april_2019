using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TextDialogAdvancer : MonoBehaviour
{
    public string[] lines;
    public int index = 0;
    public TextSlowlyAppear appear;
    public GameObject toggle;
    public UnityEvent done;

    public void Advance()
    {
        if (appear.doneAppearing)
        {
            appear.Clear();
            index += 1;
            if (index == lines.Length)
            {
                toggle?.SetActive(false);
                done.Invoke();
            }
            else if (index < lines.Length)
            {
                toggle?.SetActive(true);
                appear.targetText = lines[index];
            }
        }
    }
}
