using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovent : MonoBehaviour
{
    // ДВвижение
    private float xMovent;
    private float zMovent;
    public Rigidbody rb;
    private float yRotation;
    
    
    [SerializeField]private Player player;
    public Vector3 moveDirection;
    public Camera playerCamera; // Ссылка на камеру, относительно которой будет происходить движение

    [Header("Run")]
    public float moveSpeed;
    private float moveSpeedCurrent = 5;
    private bool isRun;
    private Animator PlayerAnimator;
    
    
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();
        playerCamera = FindObjectOfType<Camera>();
    }
    
    private void FixedUpdate()
    {
        rb.AddForce(moveDirection * moveSpeed, ForceMode.VelocityChange );
    }

    // Update is called once per frame
    void Update()
    {
        playerMoventLook();
        //transform.position += moveDirection * moveSpeed * Time.deltaTime;
        
        // Анимации
        PlayerController();
        
    }

    void PlayerController()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            
            if (player.target != null)
            {
                PlayerAnimator.SetInteger("Move", 3);
                
            }
            else
            {
                    PlayerAnimator.SetInteger("Move", 1);
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        PlayerAnimator.SetInteger("Move", 2);
                        moveSpeed = 10;
                    }
                    else
                    {
                        moveSpeed = moveSpeedCurrent;
                    }
            }
            
        }
        else
        {
            PlayerAnimator.SetInteger("Move", 0);
            
        }
    }


     void playerMoventLook()
    {
        // Camera
        xMovent = Input.GetAxis("Horizontal");
        zMovent = Input.GetAxis("Vertical");
        
        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0;
        
        moveDirection = Vector3.Lerp(moveDirection, (Quaternion.LookRotation(cameraForward) * new Vector3(xMovent, 0, zMovent)), Time.deltaTime * moveSpeed);
        
        
        
        // Если есть ввод, поворачиваем персонажа в направлении движения
        
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
            if (player.target)
            {
                PlayerTargetRotation();
            }
            
        }
    }

    void PlayerTargetRotation()
    {
        
        
        Vector3 toTarget = player.target.transform.position - transform.position;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
        
        
        transform.rotation = Quaternion.LookRotation(toTargetXZ);
    }
    
}
