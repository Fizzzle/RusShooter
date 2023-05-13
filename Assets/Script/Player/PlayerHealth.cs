using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    
    
    
    [Header("Health Settings")]
    public float health = 100f;
    public float lerpTime;
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image DamageImage;
    public Text HealthText;
    // Бессмертие 
    private bool _invulnerable = false;
    
    
    public Slider frontHealthBar;
    public Slider backHealthBar;
    public Image backHealthBarColor;

    void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        UpdateHealthUI();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakePlayerDamage(25);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            TakePlayerHealth(12);
        }
        
        
        HealthText.text = health.ToString() + " / " + maxHealth.ToString(); 
    }

    void UpdateHealthUI()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        
        
        float fillFrontHP = frontHealthBar.value;
        float fillBackgroundHP = backHealthBar.value;
        float hpFraction = health / maxHealth;
        if (fillBackgroundHP > hpFraction)
        {
            frontHealthBar.value = hpFraction;
            lerpTime += Time.deltaTime;
            backHealthBarColor.color = Color.red;
            float percentComplete = lerpTime / chipSpeed;
            percentComplete *= percentComplete;
            backHealthBar.value = Mathf.Lerp(fillBackgroundHP, hpFraction, percentComplete);
        }

        if (fillFrontHP < hpFraction)
        {
            backHealthBar.value = hpFraction;
            lerpTime += Time.deltaTime;
            backHealthBarColor.color = Color.green;
            float percentComplete = lerpTime / chipSpeed;
            percentComplete *= percentComplete;
            frontHealthBar.value = Mathf.Lerp(fillFrontHP, backHealthBar.value, percentComplete);
        }
    }

    public void TakePlayerDamage(int PlayerDamageValue)
    {
        if (_invulnerable == false)
        {
            health -= PlayerDamageValue;
            lerpTime = 0f;
            if (health <= 0)
            {
                health = 0;
                Die();
            }
            StartCoroutine("ShowEffectDamage");
            _invulnerable = true;
            Invoke("StopInvulnerable", 1f);
        }
        
    }
    void TakePlayerHealth(int PlayerHealthValue)
    {
        health += PlayerHealthValue;
        lerpTime = 0f;
    }

    void StopInvulnerable()
    {
        _invulnerable = false;
    }
    void Die()
    {
        Debug.Log("Game Over");
    }
    
    public IEnumerator ShowEffectDamage()
    {
        DamageImage.enabled = true;
        for (float t = 0.9f; t > 0f; t-= Time.deltaTime * 1f)
        {
            DamageImage.color = new Color(0.71f, 0f, 0f, t);
            yield return null;
        }
        DamageImage.enabled = false;
    }
    
}
