using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building_HJH : MonoBehaviour
{
    HexCell myCell;
    public void SetCell(HexCell cell)
    {
        myCell = cell;
    }
    public HexCell GetCell()
    {
        return myCell;
    }
    public int people =1;
    int peopleCheck = 1;
    public int tileAmount = 7;
    int tileCheck = 8;
    public int culture = 1;
    int needCulture = 5;
    public GameObject peopleObject,playerGround;
    public int food = 0;
    int needFood = 5;
    public Text peopleText;
    int nowTurn, currentTurn = 0;

    //+
    public bool createUnit = true;
    public bool createWorker = true;

    public enum State
    {
        Idle,
        selected,

    }
    public State state;

    // Start is called before the first frame update
    void Start()
    {
        currentTurn = TurnManager_lyd.instance.turn;
    }

    
    
    // Update is called once per frame
    void Update()
    {
        #region 궁전관련
        if (gameObject.name.Contains("Palace"))
        {
            //+
            //만약 궁전짓기를 눌렀을 때 개척자유아이가 보인다.
            if (state == State.selected)
            {

                //pi
            }
            //그외에 다른 개척자를 눌렀을 때(=모든유닛을 검색했을 때 seleted가 하나라도 있으면) 개척자유아이는 보이지않는다. 
            if (state == State.Idle)
            {
                //PioneerUI(false);
            }




            TileCheck();
            PeopleCheck();
            string pText = "현재 인구 : " + people + "\n" + "성장까지 남은 식량 수 : " + (needFood - food) + "\n";
            if (people == 18)
            {
                pText = "현재 인구 : 18\n최대 인구에 도달했습니다";
            }
            string cText = "확장까지 남은 문화 수 : " + (needCulture - culture);
            if (tileAmount == 19)
            {
                cText = "도시가 최대로 확장되었습니다";
            }
            peopleText.text = pText + cText;
            CityGrowth();
            CityExtension();
            EatFood();
        }
        #endregion
    }
    #region 궁전관련
    void PeopleCheck()
    {

        if (people == 1 && peopleCheck == 1)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.NE);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 2 && peopleCheck == 2)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.E);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 3 && peopleCheck == 3)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.SE);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 4 && peopleCheck == 4)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.SW);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 5 && peopleCheck == 5)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.W);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 6 && peopleCheck == 6)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.NW);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 7 && peopleCheck == 7)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.NE).GetNeighbor(HexDirection.NE);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 8 && peopleCheck == 8)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.NE).GetNeighbor(HexDirection.E);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 9 && peopleCheck == 9)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.E).GetNeighbor(HexDirection.E);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 10 && peopleCheck == 10)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.E).GetNeighbor(HexDirection.SE);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 11 && peopleCheck == 11)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.SE).GetNeighbor(HexDirection.SE);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 12 && peopleCheck == 12)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.SE).GetNeighbor(HexDirection.SW);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 13 && peopleCheck == 13)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.SW).GetNeighbor(HexDirection.SW);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 14 && peopleCheck == 14)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.SW).GetNeighbor(HexDirection.W);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 15 && peopleCheck == 15)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.W).GetNeighbor(HexDirection.W);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 16 && peopleCheck == 16)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.W).GetNeighbor(HexDirection.NW);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 17 && peopleCheck == 17)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.NW).GetNeighbor(HexDirection.NW);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
        else if (people == 18 && peopleCheck == 18)
        {
            HexCell neigh = myCell.GetNeighbor(HexDirection.NW).GetNeighbor(HexDirection.NE);
            neigh.inPlayer = true;
            Instantiate(peopleObject, neigh.transform.position, Quaternion.identity).transform.parent = this.transform;
            peopleCheck++;
        }
    }
    void TileCheck()
    {
        if(tileAmount == 8 && tileCheck == 8)
        {
            HexCell neigh2 = myCell.GetNeighbor(HexDirection.NE).GetNeighbor(HexDirection.NE);
            neigh2.inPlayer = true;
            neigh2.SetPalace(this);
            Instantiate(playerGround, neigh2.transform.position + new Vector3(0,0.1f,0), Quaternion.identity).transform.parent = this.transform;
            tileCheck++;
        }
        else if (tileAmount == 9 && tileCheck == 9)
        {
            HexCell neigh2 = myCell.GetNeighbor(HexDirection.NE).GetNeighbor(HexDirection.E);
            neigh2.inPlayer = true;
            neigh2.SetPalace(this);
            Instantiate(playerGround, neigh2.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity).transform.parent = this.transform;
            tileCheck++;
        }
        else if (tileAmount == 10 && tileCheck == 10)
        {
            HexCell neigh2 = myCell.GetNeighbor(HexDirection.E).GetNeighbor(HexDirection.E);
            neigh2.inPlayer = true;
            neigh2.SetPalace(this);
            Instantiate(playerGround, neigh2.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity).transform.parent = this.transform;
            tileCheck++;
        }
        else if (tileAmount == 11 && tileCheck == 11)
        {
            HexCell neigh2 = myCell.GetNeighbor(HexDirection.E).GetNeighbor(HexDirection.SE);
            neigh2.inPlayer = true;
            neigh2.SetPalace(this);
            Instantiate(playerGround, neigh2.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity).transform.parent = this.transform;
            tileCheck++;
        }
        else if (tileAmount == 12 && tileCheck == 12)
        {
            HexCell neigh2 = myCell.GetNeighbor(HexDirection.SE).GetNeighbor(HexDirection.SE);
            neigh2.inPlayer = true;
            neigh2.SetPalace(this);
            Instantiate(playerGround, neigh2.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity).transform.parent = this.transform;
            tileCheck++;
        }
        else if (tileAmount == 13 && tileCheck == 13)
        {
            HexCell neigh2 = myCell.GetNeighbor(HexDirection.SE).GetNeighbor(HexDirection.SW);
            neigh2.inPlayer = true;
            neigh2.SetPalace(this);
            Instantiate(playerGround, neigh2.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity).transform.parent = this.transform;
            tileCheck++;
        }
        else if (tileAmount == 14 && tileCheck == 14)
        {
            HexCell neigh2 = myCell.GetNeighbor(HexDirection.SW).GetNeighbor(HexDirection.SW);
            neigh2.inPlayer = true;
            neigh2.SetPalace(this);
            Instantiate(playerGround, neigh2.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity).transform.parent = this.transform;
            tileCheck++;
        }
        else if (tileAmount == 15 && tileCheck == 15)
        {
            HexCell neigh2 = myCell.GetNeighbor(HexDirection.SW).GetNeighbor(HexDirection.W);
            neigh2.inPlayer = true;
            neigh2.SetPalace(this);
            Instantiate(playerGround, neigh2.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity).transform.parent = this.transform;
            tileCheck++;
        }
        else if (tileAmount == 16 && tileCheck == 16)
        {
            HexCell neigh2 = myCell.GetNeighbor(HexDirection.W).GetNeighbor(HexDirection.W);
            neigh2.inPlayer = true;
            neigh2.SetPalace(this);
            Instantiate(playerGround, neigh2.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity).transform.parent = this.transform;
            tileCheck++;
        }
        else if (tileAmount == 17 && tileCheck == 17)
        {
            HexCell neigh2 = myCell.GetNeighbor(HexDirection.W).GetNeighbor(HexDirection.NW);
            neigh2.inPlayer = true;
            neigh2.SetPalace(this);
            Instantiate(playerGround, neigh2.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity).transform.parent = this.transform;
            tileCheck++;
        }
        else if (tileAmount == 18 && tileCheck == 18)
        {
            HexCell neigh2 = myCell.GetNeighbor(HexDirection.NW).GetNeighbor(HexDirection.NW);
            neigh2.inPlayer = true;
            neigh2.SetPalace(this);
            Instantiate(playerGround, neigh2.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity).transform.parent = this.transform;
            tileCheck++;
        }
        else if (tileAmount == 19 && tileCheck == 19)
        {
            HexCell neigh2 = myCell.GetNeighbor(HexDirection.NW).GetNeighbor(HexDirection.NE);
            neigh2.inPlayer = true;
            neigh2.SetPalace(this);
            Instantiate(playerGround, neigh2.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity).transform.parent = this.transform;
            tileCheck++;
        }
    }
    
    void EatFood()
    {
        nowTurn = TurnManager_lyd.instance.turn;
        if (nowTurn != currentTurn)
        {
            food -= people;
            currentTurn = nowTurn;
        }
    }

    void CityGrowth()
    {
        if(food > needFood)
        {
            people++;
            food -= needFood;
            needFood = needFood * 2;
        }
    }

    void CityExtension()
    {
        if(culture > needCulture)
        {
            tileAmount++;
            culture -= needCulture;
            needCulture = needCulture * 2;
        }
    }
    #endregion
}