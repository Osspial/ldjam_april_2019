using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggro : MonoBehaviour
{
    void OnDestroy()
    {
        AggroTracker.instance.aggros.Remove(this);
    }
}
