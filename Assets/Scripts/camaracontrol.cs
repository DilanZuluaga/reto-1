using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camaracontrol : MonoBehaviour
{

    public Transform player;

    public Vector3 ofsset;

    public float camaraSpeed = 10.0f;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        
        transform.position = player.position + ofsset;
    }
}
