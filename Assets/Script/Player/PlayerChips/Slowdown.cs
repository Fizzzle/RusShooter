using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowdown : MonoBehaviour
{
    [Header("Slowdown Settings")]
    public float TimeScale = 0.2f;

    public float TimeOnSlow = 2;

    public float maxSlowActiveTimer = 10f;
    [SerializeField]private float _currentSlowTime;
    private bool _isSlow;
    private float _startFixedDeltaTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _startFixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        TimeSlowDown();
    }

    IEnumerator SlowTimeMode()
    {
        Time.timeScale = TimeScale;
        yield return new WaitForSeconds(TimeOnSlow);
        Time.timeScale = 1f;
        _currentSlowTime = 0;
        _isSlow = false;
    }

    public void TimeSlowDown()
    {
        if (_isSlow)
        {
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(SlowTimeMode());
            }
            
        }
        else
        {
            _currentSlowTime += Time.deltaTime;
            if (_currentSlowTime >= maxSlowActiveTimer)
            {
                _isSlow = true;
            } 
        }

        Time.fixedDeltaTime = _startFixedDeltaTime * Time.timeScale;
    }
}
