using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HJH_RockManager : MonoBehaviour
{
    public GameObject rock;
    public GameObject groundObj;
    static public bool gg = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float randomTime;
    float time;
    // Update is called once per frame
    void Update()
    {
        if(gg == false)
        {
            if (HJH_PlayerMove.instance.mapSpeed > 0.1)
            {
                time += HJH_PlayerMove.instance.mapSpeed * Time.deltaTime;
            }
            if (time > randomTime)
            {
                int ran = Random.Range(0, 3);
                if (ran == 2)
                {
                    Instantiate(rock, new Vector3(18, -3.2f, 0), Quaternion.identity);
                }
                else if (ran == 1)
                {
                    Instantiate(groundObj, new Vector3(5, 0, 0), Quaternion.identity);
                }
                time = 0;
            }
        }
        
        
    }
}
