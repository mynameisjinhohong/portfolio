using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit_HJH : MonoBehaviour
{
    public int movePower;
    [SerializeField]
    int movePowerClone;
    bool moveOver = true;
    public UnitStatus_HJH status;
    [SerializeField]
    HexCell myCell;
    [SerializeField]
    bool unitNear = false;
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
        status = GetComponent<UnitStatus_HJH>();
        nowTurn = TurnManager_lyd.instance.turn;
        currentTurn = TurnManager_lyd.instance.turn;
        movePowerClone = movePower;
        Invoke("SetMyCell",0.01f);
    }
    void SetMyCell()
    {
        HexCell[] cell = HexGrid.instance.cells; //�ʵ忡 �ִ� ��� ���� ������ �����´�.
        for (int i = 0; i < cell.Length; i++)
        {
            if (1 > Mathf.Abs(cell[i].transform.position.x - this.transform.position.x)) //Abs ����
            {
                if (1 > Mathf.Abs(cell[i].transform.position.z - this.transform.position.z))
                {
                    myCell = cell[i];
                    cell[i].EnemyUnit = this;
                }
            }
        }
    }
    int nowTurn = 0, currentTurn = 0;
    int whereUnit = 0;
    // Update is called once per frame
    void Update()
    {
        if(myCell.state == HexCell.State.look)
        {
            transform.position = new Vector3(transform.position.x,10f , transform.position.z);
        }
        //else
        //{
        //    transform.position = new Vector3(transform.position.x, -20f, transform.position.z);
        //}
        currentTurn = TurnManager_lyd.instance.turn;
        if(currentTurn > nowTurn && moveOver == true && movePowerClone >0)
        {
            for(int j = 0; j< 6; j++)
            {
                if(myCell.GetNeighbor((HexDirection)j).getUnit() != null)
                {
                    unitNear = true;
                    whereUnit = j;
                    break;
                }
                else
                {
                    unitNear = false;
                }
            }
            if(unitNear == true)
            {
                UnitStatus_HJH enst = myCell.GetNeighbor((HexDirection)whereUnit).getUnit().gameObject.GetComponent<UnitStatus_HJH>();
                enst.Damage(status.attackPower);
                movePowerClone = 0;
            }
            else if(unitNear == false)
            {
                int i = Random.Range(0, 6);
                if (myCell.GetNeighbor((HexDirection)i))
                {
                    if (myCell.GetNeighbor((HexDirection)i).EnemyUnit == false && myCell.GetNeighbor((HexDirection)i).gameObject.name.Contains("Ground") && myCell.GetNeighbor((HexDirection)i).getUnit() == null)
                    {
                        StopAllCoroutines();

                        StartCoroutine(MoveTile(myCell.GetNeighbor((HexDirection)i)));
                    }
                }
            }

            
            
            
        }
        if(movePowerClone < 1)
        {
            nowTurn = currentTurn;
            movePowerClone = movePower;
        }

    }
    IEnumerator MoveTile(HexCell cell)
    {
        moveOver = false;
        while (true)
        {
            Vector3 target = (cell.transform.position - new Vector3(0, 20, 0)) - transform.position;
            if(myCell.state == HexCell.State.look)
            {
                target = cell.transform.position - transform.position;
            }
            transform.position += target.normalized * Time.deltaTime * 50;
            myCell.EnemyUnit = null;
            cell.EnemyUnit = this;
            myCell = cell;
            if (Vector3.Distance(transform.position, (cell.transform.position - new Vector3(0, 20, 0))) < 1f)
            {
                transform.position = (cell.transform.position - new Vector3(0, 20, 0));
                movePowerClone--;
                moveOver = true;
                yield break;
            }
            yield return null;
        }

            
        //movePowerClone = movePower;

        
    }

}