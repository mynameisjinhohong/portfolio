using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HJH_RockMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    float rockSpeed;
    // Update is called once per frame
    void Update()
    {
        rockSpeed = HJH_PlayerMove.instance.mapSpeed;
        rockSpeed = rockSpeed * 4.1f;
        gameObject.transform.position += Vector3.left * rockSpeed * Time.deltaTime;
    }
}
