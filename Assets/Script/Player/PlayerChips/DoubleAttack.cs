using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAttack : MonoBehaviour
{
    private Player Player;
    private bool isDoubleShot = true;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            DoubleShoot();
        }
    }
    
    IEnumerator doubleShoot()
    {
        if (Player.target)
        {
            if (isDoubleShot)
            {
                isDoubleShot = false;
                Player.shootEffect.Play();
                GameObject newBullet = GameObject.Instantiate(Player.bullet, Player.shootElement.position, Quaternion.identity) as GameObject;
                newBullet.GetComponent<Bullet>().target = Player.target;
                yield return new WaitForSeconds(0.1f);
                GameObject newBulletTwo = GameObject.Instantiate(Player.bullet, Player.shootElement.position, Quaternion.identity) as GameObject;
                newBulletTwo.GetComponent<Bullet>().target = Player.target;
                yield return new WaitForSeconds(5);
                isDoubleShot = true;
            }
            
        }
        else
        {
            if (isDoubleShot)
            {
                isDoubleShot = false;
                Player.shootEffect.Play();
                GameObject newBullet = GameObject.Instantiate(Player.bullet, Player.shootElement.position, Quaternion.identity) as GameObject;
                newBullet.GetComponent<Bullet>().target = null;
                yield return new WaitForSeconds(0.05f);
                GameObject newBulletTwo = GameObject.Instantiate(Player.bullet, Player.shootElement.position, Quaternion.identity) as GameObject;
                newBulletTwo.GetComponent<Bullet>().target = null;
                yield return new WaitForSeconds(5);
                isDoubleShot = true;
            }
            
        }
    }

    
    public void DoubleShoot()
    {
        if (Player.target)
        {
            
            if (isDoubleShot)
            {
                Player.ShotSounds();
                Vector3 toTarget = Player.target.transform.position - transform.position;
                Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
                transform.rotation = Quaternion.LookRotation(toTargetXZ);
                StartCoroutine(doubleShoot());
            }
        }
        else
        {
            if (isDoubleShot)
            {
                StartCoroutine(doubleShoot());
            }
        }
                
    }
}
