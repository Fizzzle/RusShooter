using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class ZombieController : MonoBehaviour
{
    public GameObject ZombieTarget;
    public Transform zombieTarget;
    public GameObject ZombieDeadPrefab;
    public GameObject ChildrenTexture;
    public NavMeshAgent NavAgent;
    
    private Rigidbody rb;
    public float ZombieSpeed = 4f;
    public float ZombieRageSpeed = 6f;
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
        ZombieDeadPrefab = Resources.Load("Prefabs/Particle/ZombieDead") as GameObject;
        _CameraTransform = Camera.main.transform;
        //
        PlayerDamage = FindObjectOfType<Player>().damage;

        
        StartCoroutine(FollowTarget());
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
        
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
        {
            if (collision.gameObject.GetComponent<BulletPistol>())
            {
                int pistolDamage = collision.gameObject.GetComponent<BulletPistol>().DamageZombie + Random.Range(2, 5);
                int pistolCritDamage;
                int CritChance = Random.Range(0, 5);
                switch (CritChance)
                {
                    case 1:
                        Debug.Log("CRIT");
                        pistolCritDamage = pistolDamage * 2;
                        TakeDamage(pistolCritDamage);
                        ProjectileDamageVision(pistolCritDamage, 2);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FloodLight")
        {
            StartCoroutine(SleepOfFloodLight());
        }
    }

    IEnumerator SleepOfFloodLight()
    {
        ZombieAnimator.SetTrigger("Hit");
        NavAgent.speed = 0;
        yield return new WaitForSeconds(2);
        StartCoroutine(ZombieRage());
    }

    public void TakeDamage(int ZombieDamage)
    {
        ZombieHP -= ZombieDamage;
        _zombieHealth.SetHealth(ZombieHP, ZombiemaxHP);
        int HitRandom = Random.Range(0, 3);
        if (HitRandom == 1)
        {
            StartCoroutine(DamageSlow());
        }
        
        if (ZombieHP <= 0)
        {
            GameObject ZombieDead = Instantiate(ZombieDeadPrefab, transform.position, Quaternion.identity);
            NavAgent.speed = 0;
            ZombieAnimator.SetTrigger("Hit");
            Destroy(ChildrenTexture);
            Destroy(gameObject);
            Destroy(ZombieDead, 1f);
        }
    }

    IEnumerator DamageSlow()
    {
        if (!zombieRage)
        {
            NavAgent.speed  = 0;
            ZombieAnimator.SetTrigger("Hit");
            yield return new WaitForSeconds(2);
            StartCoroutine(ZombieRage());
        }
    }

    IEnumerator ZombieRage()
    {
        zombieRage = true;
        NavAgent.speed = ZombieRageSpeed;
        yield return new WaitForSeconds(5);
        zombieRage = false;
        NavAgent.speed = ZombieSpeed;
    }

    void ProjectileDamageVision(int pistolDamage, int index = 1)
    {
        Vector3 damagePos = new Vector3(transform.position.x, transform.position.y + 2.75f, transform.position.z);
        GameObject newProjectile = Instantiate(ProjectileDamage, damagePos, Quaternion.identity);
        if (index == 2)
        {
            newProjectile.GetComponentInChildren<TextMesh>().color = Color.red;
        }
        newProjectile.transform.LookAt(_CameraTransform.position);
        ProjectileDamage.GetComponentInChildren<ProjectileDamageText>().Damage = pistolDamage;
        Destroy(newProjectile, 1f);
    }
    
    public void ZombieWalk()
    {
        if (ZombieTarget != null)
        {
            StartCoroutine(Waiting());
            NavAgent.SetDestination(zombieTarget.transform.position);
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(0.3f);
    }
    
    // GPT
    private IEnumerator FollowTarget()
    {
        while (true)
        {
            Vector3 targetPosition = zombieTarget.position;
            if (IsOnNavMesh(targetPosition))
            {
                NavAgent.SetDestination(targetPosition);
            }
            else
            {
                Vector3 nearestNavMeshPosition = FindNearestNavMeshPosition(targetPosition);
                NavAgent.SetDestination(nearestNavMeshPosition);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private bool IsOnNavMesh(Vector3 position)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(position, out hit, 1f, NavMesh.AllAreas);
    }

    private Vector3 FindNearestNavMeshPosition(Vector3 position)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(position, out hit, 100f, NavMesh.AllAreas);
        return hit.position;
    }
    
    
    // gpt

    private void OnDestroy()
    {
        if (_zombieHealth != null)
        {
            Destroy(_zombieHealth.gameObject);
        }
    }
}
