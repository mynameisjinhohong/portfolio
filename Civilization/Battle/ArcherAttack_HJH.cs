using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttack_HJH : MonoBehaviour
{
    public GameObject attackCell;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTurn = TurnManager_lyd.instance.turn;
    }
    int currentTurn, nowTurn =0;
    public void AttackButton()
    {
        if(currentTurn > nowTurn)
        {
            GameObject[] a = GameObject.FindGameObjectsWithTag("Worker");
            List<GameObject> ahrcher = new List<GameObject>();
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].gameObject.name.Contains("Archer"))
                {
                    ahrcher.Add(a[i]);
                }
            }
            GameObject shootArcher = null;
            for (int i = 0; i < ahrcher.Count; i++)
            {
                if (ahrcher[i].GetComponent<CurrentUnit_HJH>().state == CurrentUnit_HJH.State.selected)
                {
                    shootArcher = ahrcher[i];
                }
            }
            if (shootArcher != null)
            {
                HexCell archercell = shootArcher.GetComponent<CurrentUnit_HJH>().GetCell();
                Instantiate(attackCell, archercell.transform.position, Quaternion.identity).transform.parent = archercell.transform;
                for (int i = 0; i < 6; i++)
                {
                    Instantiate(attackCell, archercell.GetNeighbor((HexDirection)i).transform.position + new Vector3(0, 0.11f, 0), Quaternion.identity).transform.parent = archercell.GetNeighbor((HexDirection)i).transform;
                    Instantiate(attackCell, archercell.GetNeighbor((HexDirection)i).GetNeighbor((HexDirection)i).transform.position + new Vector3(0, 0.11f, 0), Quaternion.identity).transform.parent = archercell.GetNeighbor((HexDirection)i).GetNeighbor((HexDirection)i).transform;
                    if (i < 5)
                    {
                        Instantiate(attackCell, archercell.GetNeighbor((HexDirection)i).GetNeighbor((HexDirection)i + 1).transform.position + new Vector3(0, 0.11f, 0), Quaternion.identity).transform.parent = archercell.GetNeighbor((HexDirection)i).GetNeighbor((HexDirection)i + 1).transform;
                    }
                    else
                    {
                        Instantiate(attackCell, archercell.GetNeighbor((HexDirection)i).GetNeighbor((HexDirection)0).transform.position + new Vector3(0, 0.11f, 0), Quaternion.identity).transform.parent = archercell.GetNeighbor((HexDirection)i).GetNeighbor((HexDirection)0).transform;
                    }
                }

            }
            nowTurn = currentTurn;
        }
        
    }
}