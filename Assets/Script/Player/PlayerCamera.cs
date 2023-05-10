using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-7.42f, 9.07f, -3f);
        transform.rotation = Quaternion.Euler(40f, 68f, 0f);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        CameraSettingWithPlayer();
    }

    void CameraSettingWithPlayer()
    {
        Vector3 position = target.position;
        position.y = target.transform.position.y + 10f;
        position.x = target.transform.position.x + -10f;
        position.z = target.transform.position.z - 4f;
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
}
