using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketLight : MonoBehaviour
{
    public GameObject PocketFlashlight;

    private void Start()
    {
        PocketFlashlight = GameObject.FindWithTag("PocketLight");
        PocketFlashlight.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PocketFlashlightCheck();
        }
    }
    
    void PocketFlashlightCheck()
    {
        if (PocketFlashlight.active == true)
        {
            PocketFlashlight.SetActive(false);
        }
        else
        {
            PocketFlashlight.SetActive(true);
        }
    }
}
