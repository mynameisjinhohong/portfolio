using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HJH_CYM_FinishLine : MonoBehaviour
{
    public GameObject stop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * HJH_PlayerMove.instance.mapSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        stop.SetActive(true);
        
    }
}
