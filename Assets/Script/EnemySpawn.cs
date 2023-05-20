using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawn : MonoBehaviour
{
    [Header("LVL Settings")] 
    public Transform Player;
     public GameObject[] enemy1;
     public GameObject[] enemyBig;
    public List<GameObject> listEnemy = new List<GameObject>();
    public GameObject Zombies;
    public Vector3[] SpawnPosition;
    public int WavesCount = 2;
    private bool isWaveRunning = false;
    public int WavesCountText = 1;
    

    [Header("Count Enemy")]
    public int enemyCountLVL = 2;

    public int enemyBigCountLVL = 1; 
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
         EnemySpawnSetting();
    }

    private IEnumerator WavesCoroutine()
    {
        if (isWaveRunning)
            yield break;

        isWaveRunning = true;

        while (listEnemy.Count == 0)
        {
            yield return new WaitForSeconds(2);
            WavesCount++;
            WavesCountText++;
            Debug.Log(listEnemy.Count);
            WavesEnemy();
        }

        isWaveRunning = false;
    }
    
    void WavesEnemy()
    {
        for (int i = 0; i <= WavesCount; i++)
        {
             EnemySpawnSetting();
        }
    }

    public void SpawnPositionVector()
    {
        
        if (SpawnPosition.Length > 1)
        {
            Vector3 CurrentSpawnPosition = SpawnPosition[Random.Range(0, SpawnPosition.Length)];
            float SpawnDistance =  Vector3.Distance(CurrentSpawnPosition, Player.transform.position);
            bool GetSpawn = true;
            int checkFatalErrorWhile = 1;
            if (SpawnDistance > 20)
            {
                
                transform.position = CurrentSpawnPosition;
            }
            else
            {
                while (GetSpawn)
                {
                    CurrentSpawnPosition = SpawnPosition[Random.Range(0, SpawnPosition.Length)];
                    SpawnDistance = Vector3.Distance(CurrentSpawnPosition, Player.transform.position);
                    checkFatalErrorWhile++;
                    if (SpawnDistance > 50)
                    {
                        transform.position = CurrentSpawnPosition;
                        GetSpawn = false;
                    }

                    if (checkFatalErrorWhile >= 50)
                    {
                        GetSpawn = false;
                        Debug.Log("Невозможно заспавнить");
                    }
                }
            }
        }
    }


    public GameObject GetAttackClosest(Vector3 point)
    {
        float minDistance = Mathf.Infinity;
        GameObject closestEnemy = null;
        for (int i = 0; i < listEnemy.Count; i++)
        {
            if (listEnemy[i] != null)
            {
                float distance = Vector3.Distance(point, listEnemy[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                
                    closestEnemy = listEnemy[i];
                }
            } else if (listEnemy[i] == null)
            {
                listEnemy.RemoveAt(i);
            }
        }

        if (listEnemy.Count == 0)
        {
            if (!isWaveRunning)
            { 
                StartCoroutine(WavesCoroutine());
            }
        }

        return closestEnemy;
    }

    void enemyBigCount()
    {
        enemyBig = new GameObject[3];
        enemyBig[0] = Resources.Load("Prefabs/ZombiePunching") as GameObject;
        enemyBig[1] = Resources.Load("Prefabs/ZombieRunner") as GameObject;
        enemyBig[2] = Resources.Load("Prefabs/ZombieRunnerFemale") as GameObject;
    }
    
    void enemy1Count()
    {
        enemy1 = new GameObject[3];
        enemy1[0] = Resources.Load("Prefabs/ZombieFirstFemale") as GameObject;
        enemy1[1] = Resources.Load("Prefabs/ZombieFirstMan") as GameObject;
        enemy1[2] = Resources.Load("Prefabs/ZombieFirstManTwo") as GameObject;
    }
    
    public void EnemySpawnSetting()
    {
        SpawnPositionVector();
        enemy1Count();
        enemyBigCount();
        for (int i = 0; i < enemyCountLVL; i++)
        {
            Vector3 Position = new Vector3( transform.position.x, transform.position.y + Random.Range(1f, 3f),transform.position.z);
            GameObject Enemy = Instantiate(enemy1[Random.Range(0,3)], Position, Quaternion.identity);
        }

        for (int i = 0; i < enemyBigCountLVL; i++)
        {
            Vector3 Position = new Vector3( transform.position.x, transform.position.y + Random.Range(1f, 3f),transform.position.z);
            GameObject Enemy = Instantiate(enemyBig[Random.Range(0,3)], Position, Quaternion.identity);
        }
    }
}
