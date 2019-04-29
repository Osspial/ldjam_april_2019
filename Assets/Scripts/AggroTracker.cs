using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroTracker : MonoBehaviour
{
    public static AggroTracker instance;
    public HashSet<Aggro> aggros = new HashSet<Aggro>();
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
