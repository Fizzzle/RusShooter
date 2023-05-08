using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawn : MonoBehaviour
{
    [Header("LVL Settings")]
     public Player player;
     public GameObject curTarget;
     public GameObject[] enemy1;
    public List<GameObject> listEnemy = new List<GameObject>();
    public GameObject Zombies;

    [Header("Count Enemy")]
    public int enemyCountLVL = 5;
    // Start is called before the first frame update
    void Start()
    {
        EnemySpawnSetting();
        
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
        enemy1Count();
        for (int i = 0; i < enemyCountLVL; i++)
        {
            Vector3 Position = new Vector3(-12.4f, Random.Range(9f, 12f), -5);
            GameObject Enemy = Instantiate(enemy1[Random.Range(0,3)], Position, Quaternion.identity);
        }
    }
}
