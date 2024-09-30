using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell_HJH : MonoBehaviour
{
    #region 적 요소
    public bool EnemyGround;
    public bool EnemyPalace;
    public EnemyUnit_HJH EnemyUnit;
    public bool EnemyMarket;
    public bool EnemySuwon;
    public bool EnemyArmy;
    #endregion

    public HexCoordinates coordinates;
    public Color color;
    public enum State
    {
        notlooked,
        look,
        looked,
    }
    public State state;
    GameObject FogOfWar;
    Material FogMat;
    float fogColor = 0f;
    Building_HJH palace;
    public void SetPalace(Building_HJH h)
    {
        palace = h;
    }
    public Building_HJH GetPalace()
    {
        return palace;
    }
    public bool inPlayer;

    //+
    [SerializeField]
    CurrentUnit_HJH unit; //각 타일에 어떤 유닛이 있는지
    [SerializeField]
    Building_HJH building; //각 타일에 어떤 빌딩이 있는지 -> 유닛이 생성되거나 빌딩이 생성되면 다 바꿔줘야함.

    [SerializeField]
    public HexCell[] neighbors;
    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }
    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        if (gameObject.transform.childCount > 0)
        {
            neighbors[(int)direction] = cell;
            cell.neighbors[(int)direction.Opposite()] = this;
        }
    }
    public void setBuilding(Building_HJH h)
    {
        building = h;
    }
    public Building_HJH getBuilding()
    {
        return building;
    }

    // Start is called before the first frame update
    void Start()
    {

        if (gameObject.transform.childCount > 0)
        {
            FogOfWar = gameObject.transform.GetChild(0).gameObject;
            FogMat = FogOfWar.GetComponent<Renderer>().material;
        }

    }

    // Update is called once per frame
    void Update()
    {
        #region 전장의 안개
        if (FogOfWar != null)
        {
            for (int i = 0; i < neighbors.Length; ++i)
            {
                if (neighbors[i] != null)
                {
                    if (neighbors[i].getUnit() != null)
                    {
                        state = State.look;
                    }
                }
            }
            if (state == State.look)
            {
                int count = 0;
                for (int i = 0; i < neighbors.Length; ++i)
                {
                    if (neighbors[i] != null)
                    {
                        if (neighbors[i].getUnit() == null)
                        {
                            count++;
                        }
                    }
                }
                if (count == neighbors.Length)
                {
                    state = State.looked;
                }
            }
            if (unit != null)
            {
                state = State.look;
            }
            if (state == State.look)
            {
                Color c = FogMat.color;
                c.a = 0f;
                FogMat.color = c;
            }
            else if (state == State.looked)
            {
                Color c = FogMat.color;
                fogColor += Time.deltaTime * 0.5f;
                if (fogColor < 0.5f)
                {
                    c.a = fogColor;
                    FogMat.color = c;
                }
            }
            if (building != null)
            {
                for (int i = 0; i < neighbors.Length; ++i)
                {
                    for (int k = 0; k < neighbors.Length; ++k)
                    {
                        neighbors[i].GetNeighbor((HexDirection)k).state = State.look;

                    }
                }
            }
        }
        #endregion
        if(state == State.look && EnemyPalace == true)
        {
            GameObject eg = gameObject.transform.GetChild(2).gameObject;
            eg.transform.position = new Vector3(eg.transform.position.x,7.5f,eg.transform.position.z);
        }
        if(state == State.look && EnemyGround == true)
        {
            GameObject ep = gameObject.transform.GetChild(1).gameObject;
            ep.transform.position = new Vector3(ep.transform.position.x, 0.1f, ep.transform.position.z);
        }
        if (state == State.look && EnemyArmy == true)
        {
            GameObject eg = gameObject.transform.GetChild(2).gameObject;
            eg.transform.position = new Vector3(eg.transform.position.x, 7.5f, eg.transform.position.z);
        }
        if (state == State.look && EnemySuwon == true)
        {
            GameObject eg = gameObject.transform.GetChild(2).gameObject;
            eg.transform.position = new Vector3(eg.transform.position.x, 7.5f, eg.transform.position.z);
        }
        if (state == State.look && EnemyMarket == true)
        {
            GameObject eg = gameObject.transform.GetChild(2).gameObject;
            eg.transform.position = new Vector3(eg.transform.position.x, 7.5f, eg.transform.position.z);
        }
    }
    public void setUnit(CurrentUnit_HJH h)
    {
        unit = h;
    }
    public CurrentUnit_HJH getUnit()
    {
        if (unit != null)
        {
            return unit;

        }
        else
        {
            return null;
        }
    }

}