using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class ZombieController : MonoBehaviour
{
    public GameObject ZombieTarget;
    public Transform zombieTarget;
    public GameObject ZombieDeadPrefab;
    public GameObject ChildrenTexture;
    
    private Rigidbody rb;
    public float ZombieSpeed = 4f;
    public int ZombieAttack = 10;
    public float distance;
    public int ZombieHP = 100;
    private int ZombiemaxHP;
    private bool zombieRage = false;

    private Vector3 downForce = new Vector3(0, -10f, 0);
    private Transform _CameraTransform;
    public GameObject ProjectileDamage;
    public GameObject HealthBarPrefab;
    private ZombieHealth _zombieHealth;
    public Animator ZombieAnimator;
    private int PlayerDamage;

    private void Start()
    {
        //Добавляет сам себя в список врагов 
        FindObjectOfType<EnemySpawn>().listEnemy.Add(gameObject);
        rb = GetComponent<Rigidbody>();
        ZombiemaxHP = ZombieHP;
        zombieTarget = GameObject.FindWithTag("Player").transform;
        ZombieTarget = GameObject.FindWithTag("Player");
        ZombieAnimator = gameObject.GetComponent<Animator>();
        GameObject healthBar = Instantiate(HealthBarPrefab);
        _zombieHealth = healthBar.GetComponent<ZombieHealth>();
        _zombieHealth.Setup(transform);
        ZombieDeadPrefab = Resources.Load("Prefabs/Particle/ZombieDeadSmoke") as GameObject;
        _CameraTransform = Camera.main.transform;
        //
        PlayerDamage = FindObjectOfType<Player>().damage;

    }

    void Update()
    {
        distance = Vector3.Distance(gameObject.transform.position, ZombieTarget.transform.position);
        
        ZombieAttackTrigger();
    }

    private void FixedUpdate()
    {
        ZombieMoveLookRb();
    }

    public void ZombieAttackTrigger()
    {
        if (distance <= 2.5f)
        {
            ZombieAnimator.SetBool("Attack", true);
            StartCoroutine(DamageSlow());
        }
        else
        {
            ZombieAnimator.SetBool("Attack", false);
        }
    }
    

    public void ZombieMoveLookRb()
    {
        Vector3 direction = (zombieTarget.position - transform.position).normalized;
        Vector3 directionXZ = new Vector3(direction.x, 0f, direction.z);
        Quaternion lookRotation = Quaternion.LookRotation(directionXZ);
        rb.MoveRotation(lookRotation);

        // Двигаем объект к цели через Rigidbody
        rb.MovePosition(transform.position + directionXZ * ZombieSpeed * Time.deltaTime);
        rb.AddForce(downForce, ForceMode.Acceleration);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
        {
            if (collision.gameObject.GetComponent<BulletPistol>())
            {
                int pistolDamage = collision.gameObject.GetComponent<BulletPistol>().DamageZombie + Random.Range(2, 5);
                int CritChance = Random.Range(0, 5);
                switch (CritChance)
                {
                    case 1:
                        Debug.Log("CRIT");
                        TakeDamage(pistolDamage * 2);
                        ProjectileDamageVision(pistolDamage);
                        break;
                    default:
                        Debug.Log("NotCrit");
                        TakeDamage(pistolDamage);
                        ProjectileDamageVision(pistolDamage);
                        break;
                }
            }
        }

        if (collision.rigidbody != null)
        {
            if (collision.rigidbody.GetComponent<PlayerHealth>())
            {
                collision.rigidbody.GetComponent<PlayerHealth>().TakePlayerDamage(ZombieAttack);
            }
        }
    }

    public void TakeDamage(int ZombieDamage)
    {
        ZombieHP -= ZombieDamage;
        _zombieHealth.SetHealth(ZombieHP, ZombiemaxHP);
        StartCoroutine(DamageSlow());
        if (ZombieHP <= 0)
        {
            Instantiate(ZombieDeadPrefab,gameObject.transform);
            ZombieSpeed = 0;
            ZombieAnimator.SetTrigger("Hit");
            Destroy(ChildrenTexture);
            Destroy(gameObject, 0.5f);
        }
    }

    IEnumerator DamageSlow()
    {
        if (!zombieRage)
        {
            ZombieSpeed = 0;
            ZombieAnimator.SetTrigger("Hit");
            yield return new WaitForSeconds(2);
            StartCoroutine(ZombieRage());
        }
    }

    IEnumerator ZombieRage()
    {
        zombieRage = true;
        ZombieSpeed = 5;
        yield return new WaitForSeconds(5);
        zombieRage = false;
        ZombieSpeed = 4;
    }

    void ProjectileDamageVision(int pistolDamage)
    {
        Vector3 damagePos = new Vector3(transform.position.x, transform.position.y + 2.75f, transform.position.z);
        
        GameObject newProjectile = Instantiate(ProjectileDamage, damagePos, Quaternion.identity);
        newProjectile.transform.LookAt(_CameraTransform.position);
        ProjectileDamage.GetComponentInChildren<ProjectileDamageText>().Damage = pistolDamage;
    }
    

    private void OnDestroy()
    {
        if (_zombieHealth != null)
        {
            Destroy(_zombieHealth.gameObject);
        }
    }
}
