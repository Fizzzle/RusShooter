using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floodlight : MonoBehaviour
{
    public GameObject FlooLightObject;
    private Light FloodLight;

    private void Start()
    {
        FloodLight = FlooLightObject.GetComponent<Light>();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            FloodLighting();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("HEY");
        }
    }

    public void FloodLighting()
    {
        StartCoroutine(FloodLightSkill());
    }
    
    IEnumerator FloodLightSkill()
    {
        FlooLightObject.SetActive(true);
        yield return new WaitForSeconds(1);
        FlooLightObject.SetActive(false);
    }
}
