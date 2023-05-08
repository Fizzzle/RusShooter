using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Bullet : MonoBehaviour
{
    public GameObject EffectPrefab;
    public GameObject EnemyHitPrefab;
    private Rigidbody rbBullet;
    private float speedBullet = 55f;
    public Transform target;        
    private Vector3 player;        
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.forward;      
        rbBullet = GetComponent<Rigidbody>();                                            
        EffectPrefab = Resources.Load("Prefabs/Particle/BulletHit") as GameObject;
        EnemyHitPrefab = Resources.Load("Prefabs/Particle/EnemyHit") as GameObject;
        Destroy(gameObject, 1f);
    }

    private void Update()
    {
        BulletBehavior();
    }

    void BulletBehavior()                                                                                                                                                                             
         {                                                                                                                                                                                                 
             if (target)                                                                                                                                                                                   
             {                                                                                                                                                                                             
                 transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speedBullet);                                                                              
             }                                                                                                                                                                                             
             else                                                                                                                                                                                          
             {                                                                                                                                                                                             
                 rbBullet.velocity = player * speedBullet;                                                                                                                                                 
             }                                                                                                                                                                                             
         }                                                                                                                                                                                                 
                                                                                                                                                                                                   

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Enemy")
        { 
            Instantiate(EnemyHitPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(EffectPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
