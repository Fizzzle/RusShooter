using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floodlight : MonoBehaviour
{
    public GameObject FlooLightObject;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            FlooLightObjectCheck();
        }
    }
    
    void FlooLightObjectCheck()
    {
        if (FlooLightObject.active == true)
        {
            FlooLightObject.SetActive(false);
        }
        else
        {
            FlooLightObject.SetActive(true);
        }
    }
}
