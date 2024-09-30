using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBuilding_HJH : MonoBehaviour
{
    public GameObject palace,playerGround;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    //+
    //함수를 만든다.-> 무슨 함수? 개척자UI가 나올수 있는 함수
    void PioneerUI()
    {
        //pioneerUI가 어디있는가. -> building에 있음. 
        //building의 상태가 selected일 때 true(ui가 보임), building의 상태가 idle일 는 false로 보인다 
        //building은 palace의 태그를 달고있다. 찾아준다.

        GameObject[] pioneerUI = GameObject.FindGameObjectsWithTag("Palace"); //모든애들을 검색해서 pionnerUI 그릇에 담는다.
        int count = pioneerUI.Length; //count의 의미는 빌딩의 갯수.
       
        //그 그릇에 들어가는 양만큼 FOR문을 돌린다.
        for (int i = 0; i < pioneerUI.Length; i++)
        {

            //palace의 상태가 selected이면 ui가 보여야한다.
            //첫번째 돈 빌딩 스크립트에 접근
            Building_HJH bu = pioneerUI[i].GetComponent<Building_HJH>();
            //만약 빌딩이 SELECTED상태이면 
            // 예를들어 여러가지들 중에 모든애들이 다 idle이여지만 ui를 꺼야한다. 
            if(bu != null)
            {
                if (bu.state == Building_HJH.State.selected)
                {
                    //UI를 켜준다.
                    GameObject ui = GameObject.Find("Canvas_Pioneer");
                    GameObject buu = ui.transform.GetChild(0).gameObject;
                    buu.SetActive(true);
                    count--; //selected일때 count의 갯수가 줄어든다.
                }
            }
            
        }
        //모든애들을 다 검사가 끝난후, 카운트가 변하지 않았다면, -> 모든애들이 idle이므로 ui를 꺼준다.
        if(count == pioneerUI.Length)
        {
            GameObject ui = GameObject.Find("Canvas_Pioneer");
            GameObject buu = ui.transform.GetChild(0).gameObject;
            buu.SetActive(false);
        }
        //찾기


        // ui.transform.SetAsLastSibling();
    }

    //+
    //노동자 ui가 나올수 있는 함수 -> 태그가 palace 인거를 다 찾아줘야한다. 
    void WorkerUI()
    {
        GameObject[] workerUI = GameObject.FindGameObjectsWithTag("Palace");
        int count = workerUI.Length;

        for(int i = 0; i < workerUI.Length; i++)
        {
            //빌딩이 selected일 때 selected된 애들만 노동자 버튼이 나와야한다. 
            Building_HJH bul = workerUI[i].GetComponent<Building_HJH>();
            if(bul.state == Building_HJH.State.selected)
            {
                GameObject ui = GameObject.Find("Canvas_Pioneer");
                GameObject uii = ui.transform.GetChild(1).gameObject;
                uii.SetActive(true);
                count--;
                   
            }
        }
        if(count == workerUI.Length)
        {
            GameObject ui = GameObject.Find("Canvas_Pioneer");
            GameObject uii = ui.transform.GetChild(1).gameObject;
            uii.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        PioneerUI();
        WorkerUI();
    }
    public void CreatePalace()
    {
        GameObject[] people = GameObject.FindGameObjectsWithTag("Unit");
        for(int i = 0; i<people.Length; ++i)
        {
            CurrentUnit_HJH unit = people[i].GetComponent<CurrentUnit_HJH>();
            GameObject[] build = GameObject.FindGameObjectsWithTag("Palace");
            float nearDis = Mathf.Infinity;
            for (int k = 0; k < build.Length; ++k)
            {
                if (Vector3.Distance(build[k].transform.position, people[i].transform.position) < nearDis)
                {
                    nearDis = Vector3.Distance(build[k].transform.position, people[i].transform.position);
                }
            }
            if (unit.state == CurrentUnit_HJH.State.selected && nearDis > 8.66025404f * 8)
            {
                GameObject building = Instantiate(palace, new Vector3(people[i].transform.position.x, palace.transform.position.y,people[i].transform.position.z), Quaternion.identity);
                
                Instantiate(playerGround, new Vector3(people[i].transform.position.x, 0.1f, people[i].transform.position.z), Quaternion.identity).transform.parent = building.transform;
                HexCell[] cell = HexGrid.instance.cells;
                
                for (int k = 0; k < cell.Length; ++k)
                {
                    if(Mathf.Abs((cell[k].transform.position.x - people[i].transform.position.x))<=1)
                    {
                        if(Mathf.Abs((cell[k].transform.position.z - people[i].transform.position.z)) <= 1)
                        {
                            Building_HJH buildingPos = building.GetComponent<Building_HJH>();
                            buildingPos.tileAmount = 7;
                            buildingPos.SetCell(cell[k]);
                            cell[k].SetPalace(building.GetComponent<Building_HJH>());
                            cell[k].setBuilding(buildingPos);
                            cell[k].setUnit(null);
                            cell[k].inPlayer = true;

                            HexCell neigh1 = cell[k].GetNeighbor(HexDirection.NE);
                            if (neigh1.gameObject.name.Contains("(Clone)"))
                            {
                                Instantiate(playerGround, new Vector3(neigh1.gameObject.transform.position.x,0.1f,neigh1.gameObject.transform.position.z), Quaternion.identity).transform.parent = building.transform;
                                neigh1.inPlayer = true;
                                neigh1.SetPalace(building.GetComponent<Building_HJH>());
                            }
                            HexCell neigh2 = cell[k].GetNeighbor(HexDirection.E);
                            if (neigh2.gameObject.name.Contains("(Clone)"))
                            {
                                Instantiate(playerGround, new Vector3(neigh2.gameObject.transform.position.x, 0.1f, neigh2.gameObject.transform.position.z), Quaternion.identity).transform.parent = building.transform;
                                neigh2.SetPalace(building.GetComponent<Building_HJH>());
                                neigh2.inPlayer = true;
                            }
                            HexCell neigh3 = cell[k].GetNeighbor(HexDirection.SE);
                            if (neigh3.gameObject.name.Contains("(Clone)"))
                            {
                                Instantiate(playerGround, new Vector3(neigh3.gameObject.transform.position.x, 0.1f, neigh3.gameObject.transform.position.z), Quaternion.identity).transform.parent = building.transform;
                                neigh3.SetPalace(building.GetComponent<Building_HJH>());
                                neigh3.inPlayer = true;
                            }
                            HexCell neigh4 = cell[k].GetNeighbor(HexDirection.SW);
                            if (neigh4.gameObject.name.Contains("(Clone)"))
                            {
                                neigh4.inPlayer = true;
                                neigh4.SetPalace(building.GetComponent<Building_HJH>());
                                Instantiate(playerGround, new Vector3(neigh4.gameObject.transform.position.x, 0.1f, neigh4.gameObject.transform.position.z), Quaternion.identity).transform.parent = building.transform;
                            }
                            HexCell neigh5 = cell[k].GetNeighbor(HexDirection.W);   
                            if (neigh5.gameObject.name.Contains("(Clone)"))
                            {
                                neigh5.SetPalace(building.GetComponent<Building_HJH>());
                                neigh5.inPlayer = true;
                                Instantiate(playerGround, new Vector3(neigh5.gameObject.transform.position.x, 0.1f, neigh5.gameObject.transform.position.z), Quaternion.identity).transform.parent = building.transform;
                            }
                            HexCell neigh6 = cell[k].GetNeighbor(HexDirection.NW);
                            if (neigh6.gameObject.name.Contains("(Clone)"))
                            {
                                neigh6.SetPalace(building.GetComponent<Building_HJH>());
                                neigh6.inPlayer = true;
                                Instantiate(playerGround, new Vector3(neigh6.gameObject.transform.position.x, 0.1f, neigh6.gameObject.transform.position.z), Quaternion.identity).transform.parent = building.transform;
                            }
                        }
                    }
                }
                Destroy(people[i]);
            }
        }
        
    }
}