using System;
using UnityEngine;

[Serializable]
public class IntChange
{
    private int? _ref = null;
    public int num = 0;

    public bool changed
    {
        get => num == _ref;
    }

    public void Reset()
    {
        _ref = num;
    }

    public IntChange(int i)
    {
        num = i;
    }
}
