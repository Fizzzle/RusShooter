using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private int min = 0;
    private int sec = 0;
    public Text TimerText;
    private int delta = 1;

    private void Start()
    {
        TimerText = GameObject.Find("Timer").GetComponent<Text>();
        StartCoroutine(ITimer());
    }
    
    IEnumerator ITimer()
    {
        while (true)
        {
            if (sec == 59)
            {
                min++;
                sec = -1;
                TimerText.color = Color.green;
            }

            if (sec == 0)
            {
                TimerText.color = Color.white;
            }

            sec += delta;
            TimerText.text = min.ToString("D2") + ":" + sec.ToString("D2");
            yield return new WaitForSeconds(1);
        }
    }
}
