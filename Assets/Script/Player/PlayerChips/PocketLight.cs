using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketLight : MonoBehaviour
{
    public GameObject PocketFlashlight;


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
