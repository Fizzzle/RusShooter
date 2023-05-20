using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    
    [Header("Element Settings")]
    public Transform shootElement;
    public GameObject bullet;
    public Transform target;
    public ParticleSystem shootEffect;
    public AudioSource ShotSound;

    
    [Header("Attack Settings")]
    public Player player;
    public GameObject curTarget;
    private GameObject ClosestEnemy;
    public EnemySpawn EnemySpawn;
    private bool isShot = false;
    public int damage;
    private float distance;
    public float distanceAttack = 11f;
    public float ShotPeriod = 0.5f;
    private float _timer;

    // Update is called once per frame
    void Update()
    {

        if (EnemySpawn != null)
        {
            ClosestEnemy = EnemySpawn.GetAttackClosest(player.transform.position);
        }
        
        
        // Ближайщие враги и сама атака
        
        if (ClosestEnemy != null)
        {
            distance = Vector3.Distance(ClosestEnemy.transform.position, player.transform.position);
            CheckEnemy();
        }


        isShotCheck();
        if (Input.GetMouseButton(0))
        {
            Attack();
        }
        StopAttack();

        
    }


    IEnumerator shoot()
    {
        if (target)
        {
            if (isShot)
            {
                shootEffect.Play();
                GameObject newBullet = GameObject.Instantiate(bullet, shootElement.position, Quaternion.identity) as GameObject;
                newBullet.GetComponent<Bullet>().target = target;
                yield return new WaitForSeconds(ShotPeriod);
            }
            
        }
        else
        {
            if (isShot)
            {
                shootEffect.Play();
                GameObject newBullet = GameObject.Instantiate(bullet, shootElement.position, Quaternion.identity) as GameObject;
                newBullet.GetComponent<Bullet>().target = null;
                yield return new WaitForSeconds(ShotPeriod);
            }
            
        }
    }

    public void CheckEnemy()
    {
        if (distance <= distanceAttack)
        {
            if (ClosestEnemy.tag == "Enemy")
            {
                player.target = ClosestEnemy.transform;
                curTarget = ClosestEnemy;
            }
        }
    }

    public void Attack()
    {
        if (isShot)
        {
            ShotSounds();
        }
        if (target)
        {
            
            if (isShot)
            {
                _timer = 0;
                Vector3 toTarget = target.transform.position - transform.position;
                Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
                transform.rotation = Quaternion.LookRotation(toTargetXZ);
                StartCoroutine(shoot());
            }
        }
        else
        {
            if (isShot)
            {
                _timer = 0;
                StartCoroutine(shoot());
            }
        }
                
    }

    public void StopAttack()
    {
        if (distance > distanceAttack)
        {
            player.target = null;
        }
    }

    void isShotCheck()
    {
        _timer += Time.unscaledDeltaTime;
        if (_timer > ShotPeriod)
        {
            isShot = true;
        }
        else
        {
            isShot = false;
        }
    }

    public void ShotSounds()
    {
        ShotSound.pitch = (Random.Range(0.85f, 1f));
        ShotSound.Play();
    }
    

}
