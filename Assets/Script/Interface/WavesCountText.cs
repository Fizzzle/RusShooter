using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavesCountText : MonoBehaviour
{
    public Text WavesCountT;
    public int Waves = 1;
    private EnemySpawn enemySpawn;

    private void Awake()
    {
        enemySpawn = GameObject.FindObjectOfType<EnemySpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        WavesCountT.text = "Waves : " + enemySpawn.WavesCountText;
    }
}
