using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamageText : MonoBehaviour
{
    [HideInInspector]public int Damage;

    [HideInInspector]public TextMesh TextMesh;
    // Start is called before the first frame update
    void Start()
    {
        TextMesh = GetComponent<TextMesh>();
        TextMesh.text = "-" + Damage.ToString();
    }

    public void onAnimationOver()
    {
        Destroy(gameObject);
    }
}
