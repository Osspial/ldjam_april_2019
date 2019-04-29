using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearseAnimation : MonoBehaviour
{
    public void SetTalkState(float t)
    {
        GetComponent<Animator>().SetFloat("TalkState", t);
    }
}
