using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawn : MonoBehaviour
{
    [Header("LVL Settings")] 
    public Transform Player;
     public GameObject[] enemy1;
    public List<GameObject> listEnemy = new List<GameObject>();
    public GameObject Zombies;
    public Vector3[] SpawnPosition;

    [Header("Count Enemy")]
    public int enemyCountLVL = 2;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        EnemySpawnSetting();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
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
            if (SpawnDistance > 50)
            {
                transform.position = CurrentSpawnPosition;
            }
            else
            {
                while (GetSpawn)
                {
                    CurrentSpawnPosition = SpawnPosition[Random.Range(0, SpawnPosition.Length)];
                    SpawnDistance = Vector3.Distance(CurrentSpawnPosition, Player.transform.position);
                    if (SpawnDistance > 50)
                    {
                        transform.position = CurrentSpawnPosition;
                        GetSpawn = false;
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

        return closestEnemy;
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
        for (int i = 0; i < enemyCountLVL; i++)
        {
            Vector3 Position = new Vector3( transform.position.x, transform.position.y + Random.Range(1f, 3f),transform.position.z);
            GameObject Enemy = Instantiate(enemy1[Random.Range(0,3)], Position, Quaternion.identity);
        }
    }
}
