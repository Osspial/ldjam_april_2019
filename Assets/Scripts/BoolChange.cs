using System;
using UnityEngine;

[Serializable]
public class BoolChange
{
    private bool? _ref = null;
    public bool b = false;

    public bool changed
    {
        get => b == _ref;
    }

    public BoolChange(bool b)
    {
        this.b = b;
    }

    public void Reset()
    {
        _ref = b;
    }
}
