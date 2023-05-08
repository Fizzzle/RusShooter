using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text TimerText;

    // Update is called once per frame
    void Update()
    {
        TimerText.text = Time.fixedTime.ToString("00:00");
    }
}
