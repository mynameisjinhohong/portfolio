using CodingSystem_HJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalSet_cell : MonoBehaviour
{
    public PotalControl_Cell[] potalControl_Cells;
    public ParticleSystem[] potalActive;
    public Potal_Cell potal1;
    public Potal_Cell potal2;
    [SerializeField]
    bool active = false;
    public bool Active
    {
        get
        {
            return active;
        }
        set
        {
            active = value;
            for(int i =0; i<potalActive.Length; i++)
            {
                if(active)
                {
                    potalActive[i].Play();
                }
                else
                {
                    potalActive[i].Stop();
                }
            }   
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        for(int i =0; i< potalControl_Cells.Length; i++)
        {
            potalControl_Cells[i].potalSet_Cell = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FirstCheck()
    {
        for(int i =0; i<potalControl_Cells.Length; i++)
        {
            potalControl_Cells[i].firstCoding = false;
        }
    }
}
