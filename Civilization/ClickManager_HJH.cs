using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickManager_HJH : MonoBehaviour
{
    public GameObject seletedUnit;
    public GameObject pionnerUI, workerUI;



    HexCoordinates hexCoordinates;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject ui1 = GameObject.Find("Pioneer_HJH(Clone)");

        //개척자와 노동자의 유닛이 없을 때 ui를 꺼준다.
        if (ui1 == null)
        {
            pionnerUI.SetActive(false);
        }

        GameObject ui2 = GameObject.Find("Worker_HJH(Clone)");
        if (ui2 == null)
        {
            workerUI.SetActive(false);
        }
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            int layer = 1 << LayerMask.NameToLayer("NoCheck");
            if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, ~layer))
            {
                //Debug.Log(hit.transform.name);
                GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
                //for (int i = 0; i < units.Length; ++i)
                //{
                //    CurrentUnit_HJH unit = units[i].GetComponent<CurrentUnit_HJH>();
                //    unit.state = CurrentUnit_HJH.State.Idle;
                //}
                if (hit.transform.gameObject.tag == "Unit")
                {
                    TurnOffPionner();
                    TurnOffPalace();
                    TurnOffWorker();
                    TurnOffMarket();
                    TurnOffArmy(); //+
                    TurnOffSuwon();

                    //여기까지 추가함.

                    CurrentUnit_HJH unit = hit.transform.gameObject.GetComponent<CurrentUnit_HJH>();
                    unit.state = CurrentUnit_HJH.State.selected;



                }

                //+
                if (hit.transform.gameObject.tag == "Worker")
                {
                    TurnOffPionner();
                    TurnOffPalace();
                    TurnOffMarket();
                    TurnOffWorker();
                    TurnOffArmy(); //+
                    TurnOffSuwon();
                    //여기까지 추가함.

                    CurrentUnit_HJH unit = hit.transform.gameObject.GetComponent<CurrentUnit_HJH>();
                    unit.state = CurrentUnit_HJH.State.selected;




                }
                if (hit.transform.gameObject.tag == "Tile")
                {
                    //2.. 목적지까지 거쳐야하는 타일을 구한다.
                    //모든 유닛을 검색한다.
                    GameObject[] unit = GameObject.FindGameObjectsWithTag("Unit");
                    //그 유닛중에서 selected인 유닛만 가져온다.
                    for (int i = 0; i < unit.Length; i++)
                    {
                        CurrentUnit_HJH selectedUnit = unit[i].GetComponent<CurrentUnit_HJH>();
                        if (selectedUnit.state == CurrentUnit_HJH.State.selected)
                        {
                            for (int q = 0; q < 5; q++)
                            {
                                HexCell cell = hit.transform.gameObject.GetComponent<HexCell>();
                                selectedUnit.endPosition = cell;
                                if (selectedUnit.PathFinding(q * 3))
                                {
                                    break;
                                }
                                if (q == 4)
                                {
                                    selectedUnit.cantFindWay = true;
                                    selectedUnit.PathFinding(q);
                                    for (int w = 0; w < selectedUnit.FinalNodeList.Count; w++)
                                    {
                                        Debug.Log(HexGrid.instance.cells[selectedUnit.FinalNodeList[selectedUnit.FinalNodeList.Count - 1 - w].x + (selectedUnit.FinalNodeList[selectedUnit.FinalNodeList.Count - 1 - w].y * 68)].gameObject.name);
                                        if (HexGrid.instance.cells[selectedUnit.FinalNodeList[selectedUnit.FinalNodeList.Count - 1 - w].x + (selectedUnit.FinalNodeList[selectedUnit.FinalNodeList.Count - 1 - w].y * 68)].gameObject.name.Contains("Ground"))
                                        {

                                            selectedUnit.endPosition = HexGrid.instance.cells[selectedUnit.FinalNodeList[selectedUnit.FinalNodeList.Count - 1 - w].x + (selectedUnit.FinalNodeList[selectedUnit.FinalNodeList.Count - 1 - w].y * 68)];
                                            break;
                                        }
                                    }
                                    selectedUnit.cantFindWay = false;

                                    selectedUnit.PathFinding(q);

                                }
                                CurrentUnit_HJH.count = 0;
                            }
                        }
                    }
                    GameObject[] unit1 = GameObject.FindGameObjectsWithTag("Worker");
                    //그 유닛중에서 selected인 유닛만 가져온다.
                    for (int i = 0; i < unit1.Length; i++)
                    {
                        CurrentUnit_HJH selectedUnit = unit1[i].GetComponent<CurrentUnit_HJH>();
                        if (selectedUnit.state == CurrentUnit_HJH.State.selected)
                        {
                            HexCell cell = hit.transform.gameObject.GetComponent<HexCell>();
                            selectedUnit.endPosition = cell;
                            for (int q = 0; q < 5; q++)
                            {
                                if (selectedUnit.PathFinding(q * 3))
                                {
                                    break;
                                }
                                if (q == 4)
                                {
                                    selectedUnit.cantFindWay = true;
                                    selectedUnit.PathFinding(q);
                                    for (int w = 0; w < selectedUnit.FinalNodeList.Count; w++)
                                    {
                                        Debug.Log(HexGrid.instance.cells[selectedUnit.FinalNodeList[selectedUnit.FinalNodeList.Count - 1 - w].x + (selectedUnit.FinalNodeList[selectedUnit.FinalNodeList.Count - 1 - w].y * 68)].gameObject.name);
                                        if (HexGrid.instance.cells[selectedUnit.FinalNodeList[selectedUnit.FinalNodeList.Count - 1 - w].x + (selectedUnit.FinalNodeList[selectedUnit.FinalNodeList.Count - 1 - w].y * 68)].gameObject.name.Contains("Ground"))
                                        {

                                            selectedUnit.endPosition = HexGrid.instance.cells[selectedUnit.FinalNodeList[selectedUnit.FinalNodeList.Count - 1 - w].x + (selectedUnit.FinalNodeList[selectedUnit.FinalNodeList.Count - 1 - w].y * 68)];
                                            break;
                                        }
                                    }
                                    selectedUnit.cantFindWay = false;

                                    selectedUnit.PathFinding(q);

                                }

                            }
                            CurrentUnit_HJH.count = 0;
                        }
                       
                    }



                }
                if(hit.transform.gameObject.tag == "ArcherGround")
                {
                    Debug.Log("???");
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
                    GameObject cl = hit.transform.gameObject;
                    HexCell cell = cl.GetComponentInParent<HexCell>();
                    if(cell.EnemyUnit != null)
                    {
                        cell.EnemyUnit.status.Damage(shootArcher.GetComponent<UnitStatus_HJH>().attackPower);
                    }
                    GameObject[] c = GameObject.FindGameObjectsWithTag("ArcherGround");
                    for(int i =0; i<c.Length; i++)
                    {
                        Destroy(c[i].gameObject);
                    }
                }
                //if (hit.transform.gameObject.tag == "Tile")
                //{
                //1. 선택이 되야한다.
                // 

                //3. 순서대로 다음 타일로 이동한다.
                //4.. 유닛이 이동해야한다.
                //5. 만약 목적지 타일로 이동하면 멈춘다.
                //}

                //+
                //만약 tag된애가 건물이다. 그러면 currentUnit 스크립트에서 유닛의 상태가 idle로 바뀐다. & building의 상태를 selected로 바꿔준다.
                //-> building의 상태가 selected로 바뀌면 개척자 스크립트가 생성되고 궁전짓기 ui는 사라진다.
                if (hit.transform.gameObject.tag == "Palace")
                {
                    //모든 palace를 idle로 바꾼다. 
                    TurnOffPionner();
                    TurnOffPalace();
                    TurnOffWorker();
                    TurnOffArmy();//+
                    TurnOffMarket();
                    TurnOffSuwon();

                    Building_HJH bul = hit.transform.gameObject.GetComponent<Building_HJH>();
                    bul.state = Building_HJH.State.selected;

                }

                //+
                if (hit.transform.gameObject.tag == "Army")
                {
                    TurnOffPionner();
                    TurnOffPalace();
                    TurnOffWorker();
                    TurnOffArmy();
                    TurnOffMarket();
                    TurnOffSuwon();

                    Army_HJH farm = hit.transform.gameObject.GetComponent<Army_HJH>();
                    farm.state = Army_HJH.State.Selected;
                    Building_HJH farm1 = hit.transform.gameObject.GetComponent<Building_HJH>();
                    farm1.state = Building_HJH.State.selected;
                }
                if (hit.transform.gameObject.tag == "Market")
                {
                    TurnOffPionner();
                    TurnOffPalace();
                    TurnOffWorker();
                    TurnOffArmy();
                    TurnOffMarket();
                    TurnOffSuwon();

                    Building_HJH farm = hit.transform.gameObject.GetComponent<Building_HJH>();
                    farm.state = Building_HJH.State.selected;
                    Market_HJH farm1 = hit.transform.gameObject.GetComponent<Market_HJH>();
                    farm1.state = Market_HJH.State.Selected;
                }
                if (hit.transform.gameObject.tag == "Suwon")
                {
                    TurnOffPionner();
                    TurnOffPalace();
                    TurnOffWorker();
                    TurnOffArmy();
                    TurnOffMarket();
                    TurnOffSuwon();

                    Building_HJH farm = hit.transform.gameObject.GetComponent<Building_HJH>();
                    farm.state = Building_HJH.State.selected;
                    Suwon_HJH farm1 = hit.transform.gameObject.GetComponent<Suwon_HJH>();
                    farm1.state = Suwon_HJH.State.Selected;
                }
            }
        }


    }


    public void TurnOffWorker()
    {
        GameObject[] unit2 = GameObject.FindGameObjectsWithTag("Worker");
        //모든 유닛을 idle 상태로 바꾸고 싶다.
        for (int i = 0; i < unit2.Length; i++)
        {
            CurrentUnit_HJH Unit = unit2[i].GetComponent<CurrentUnit_HJH>();
            Unit.state = CurrentUnit_HJH.State.Idle;
        }
    }

    public void TurnOffPionner()
    {
        //+
        GameObject[] unit1 = GameObject.FindGameObjectsWithTag("Unit");
        //모든 유닛을 idle 상태로 바꾸고 싶다.
        for (int i = 0; i < unit1.Length; i++)
        {
            CurrentUnit_HJH Unit = unit1[i].GetComponent<CurrentUnit_HJH>();
            Unit.state = CurrentUnit_HJH.State.Idle;
        }
    }

    public void TurnOffPalace()
    {
        //+
        GameObject[] building = GameObject.FindGameObjectsWithTag("Palace");
        for (int i = 0; i < building.Length; i++)
        {
            Building_HJH bul = building[i].GetComponent<Building_HJH>();
            bul.state = Building_HJH.State.Idle;
        }

    }
    public void TurnOffSuwon()
    {
        //+
        GameObject[] building = GameObject.FindGameObjectsWithTag("Suwon");
        for (int i = 0; i < building.Length; i++)
        {
            Suwon_HJH bul = building[i].GetComponent<Suwon_HJH>();
            bul.state = Suwon_HJH.State.Idle;
        }

    }
    public void TurnOffMarket()
    {
        //+
        GameObject[] building = GameObject.FindGameObjectsWithTag("Market");
        for (int i = 0; i < building.Length; i++)
        {
            Market_HJH bul = building[i].GetComponent<Market_HJH>();
            bul.state = Market_HJH.State.Idle;
        }

    }

    //+
    public void TurnOffArmy()
    {
        GameObject[] army = GameObject.FindGameObjectsWithTag("Army");
        for (int i = 0; i < army.Length; i++)
        {
            Army_HJH farm = army[i].GetComponent<Army_HJH>();
            farm.state = Army_HJH.State.Idle;
        }
    }

}