using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public GameObject save;

    private GameObject saved;

    void Start()
    {
        Save();
    }

    public void Save()
    {
        if (saved)
        {
            Destroy(saved);
        }
        saved = Instantiate(save);
        saved.SetActive(false);
    }

    public void Load()
    {
        Destroy(save);
        save = Instantiate(saved);
        save.SetActive(true);
    }
}
