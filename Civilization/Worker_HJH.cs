using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker_HJH : MonoBehaviour
{
    public int WorkPower;
    GameObject button;
    GameObject UI;
    // Start is called before the first frame update
    void Start()
    {
        //유니티 씬에서 
        UI = GameObject.Find("Canvas_Pioneer");
        button = UI.transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(WorkPower <= 0)
        {
            button.SetActive(false);
            Destroy(this.gameObject);
        }
    }
}