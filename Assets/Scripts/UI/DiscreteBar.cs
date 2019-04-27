using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscreteBar : MonoBehaviour
{
    public GameObject[] icons;
    [SerializeField]
    private int Count = 4;
    public int count
    {
        get => Count;
        set => Count = value;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].SetActive(i < count);
        }
    }
}
