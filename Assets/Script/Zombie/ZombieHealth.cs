using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ZombieHealth : MonoBehaviour
{
    public Transform ScaleTransform;
    public Transform Target;
    private Transform _CameraTransform;
    
    private void Start()
    {
        _CameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.position = Target.position + Vector3.up * 0.2f;
        transform.rotation = _CameraTransform.rotation;
    }
    
    public void Setup(Transform target)
    {
        Target = target;
    }

    public void SetHealth(int ZombieHP, int MaxHealth)
    {
        float xScale = (float)ZombieHP / MaxHealth;
        xScale = Mathf.Clamp01(xScale);
        ScaleTransform.localScale = new Vector3(xScale, 1f, 1f);
    }
    
}
