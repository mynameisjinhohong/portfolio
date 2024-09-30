using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuilding_HJH : MonoBehaviour
{
    [SerializeField]
    HexCell myCell;
    public void SetCell(HexCell cell)
    {
        myCell = cell;
    }
    public HexCell GetCell()
    {
        return myCell;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}