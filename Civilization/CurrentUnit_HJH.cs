using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentUnit_HJH : MonoBehaviour
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
    public HexCell startPosition, endPosition;
    public int movePower;
    public float moveSpeed;
    public bool upgradeWater = false;
    public bool cantFindWay = false;
    UnitStatus_HJH status;
    //1. 자기위치가 필요하다.
    //2. 클릭한 타일의 위치
    //3. 길찾기 
    public GameObject UI;
    public GameObject selectCircle;
    GameObject button;

    void SetMyCell()
    {
        HexCell[] cell = HexGrid.instance.cells; //필드에 있는 모든 셀의 정보를 가져온다.
        for (int i = 0; i < cell.Length; i++)
        {
            if (1 > Mathf.Abs(cell[i].transform.position.x - this.transform.position.x)) //Abs 절댓값
            {
                if (1 > Mathf.Abs(cell[i].transform.position.z - this.transform.position.z))
                {
                    myCell = cell[i];
                    cell[i].setUnit(this);
                }
            }
        }
    }
    public enum State
    {
        Idle,
        selected,
        move,

    }
    public State state;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SetMyCell", 0.1f);
        status = GetComponent<UnitStatus_HJH>();
        UI = GameObject.Find("Canvas_Pioneer");
        if (gameObject.name.Contains("Worker"))
        {
            button = UI.transform.GetChild(2).gameObject;
        }
        else if (gameObject.name.Contains("Pioneer"))
        {
            button = UI.transform.GetChild(3).gameObject;
        }
        else if (gameObject.name.Contains("Army"))
        {
            button = null;
        }

    }
    int move = 1;
    // Update is called once per frame
    void Update()
    {
        nowTurn = TurnManager_lyd.instance.turn;


        //+
        if (state == State.selected)
        {
            selectCircle.SetActive(true);
            selectedFuction(); //워커일때는 워커 ui가 뜨고, 개척자일때는 개척자 ui가 뜬다. start에서 둘 중에 한명만 켜지는것.
        }
        else if (state == State.Idle) //여기 이해안감.
        {
            selectCircle.SetActive(false);
            if (gameObject.name.Contains("Worker"))
            {
                GameObject[] unit = GameObject.FindGameObjectsWithTag("Worker");
                int a = 0; //count의 의미 (createBuilidng.cs 처럼 모든 애들의 idle일 때를 체크해주는 것.
                for (int i = 0; i < unit.Length; i++)
                {
                    CurrentUnit_HJH un = unit[i].GetComponent<CurrentUnit_HJH>();
                    if (un.state == CurrentUnit_HJH.State.Idle)
                    {
                        ++a;
                    }

                }
                if (a == unit.Length)
                {
                    button.SetActive(false);
                }

            }
            else if (gameObject.name.Contains("Pioneer"))
            {
                GameObject[] unit = GameObject.FindGameObjectsWithTag("Unit");
                int a = 0;
                for (int i = 0; i < unit.Length; i++)
                {
                    CurrentUnit_HJH un = unit[i].GetComponent<CurrentUnit_HJH>();
                    if (un.state == CurrentUnit_HJH.State.Idle)
                    {
                        ++a;
                    }

                }
                if (a == unit.Length)
                {
                    button.SetActive(false);
                }
            }


        }
        else if (state == State.move)
        {
            selectCircle.SetActive(false);
            StopAllCoroutines();
            StartCoroutine(MoveTile());
            state = State.Idle;

        }

    }
    IEnumerator MoveTile()
    {

            move = 1;
            while (true)
            {
                int end = FinalNodeList.Count;
                if (movePower + 1 < end)
                {
                    end = movePower + 1;
                }
                float x, y;
                if (FinalNodeList[move].y % 2 == 0)
                {
                    x = FinalNodeList[move].x * 17.32051f;
                    y = FinalNodeList[move].y * 15;
                }
                else
                {
                    x = FinalNodeList[move].x * 17.32051f + 8.160255f;
                    y = FinalNodeList[move].y * 15;
                }
                if (HexGrid.instance.cells[(HexGrid.instance.width * FinalNodeList[move].y) + FinalNodeList[move].x].EnemyUnit != null)
                {
                EnemyUnit_HJH enemy = HexGrid.instance.cells[(HexGrid.instance.width * FinalNodeList[move].y) + FinalNodeList[move].x].EnemyUnit;
                enemy.status.Damage(status.attackPower);
                break;

                }
                HexCell movedCell = HexGrid.instance.cells[(HexGrid.instance.width * FinalNodeList[move - 1].y) + FinalNodeList[move - 1].x];
                movedCell.setUnit(null);
                HexCell moveCell = HexGrid.instance.cells[(HexGrid.instance.width * FinalNodeList[move].y) + FinalNodeList[move].x];
                moveCell.setUnit(this);
                myCell = moveCell;
                transform.position += (new Vector3(x, transform.position.y, y) - transform.position).normalized * Time.smoothDeltaTime * moveSpeed;
                transform.rotation = Quaternion.LookRotation(new Vector3(x, transform.position.y, y) - transform.position).normalized;
                if (Vector3.Distance(new Vector3(x, transform.position.y, y), transform.position) < 1f)
                {
                    move++;
                    gameObject.transform.position = new Vector3(x, transform.position.y, y);

                }
                if (move == end)
                {
                    state = State.Idle;
                    move = 1;
                    yield break;
                }
                yield return null;
        }
        

    }



    private void selectedFuction()
    {
        if (gameObject.name.Contains("Pioneer") || gameObject.name.Contains("Worker"))
        {
            if (gameObject.name.Contains("Worker"))
            {
                button.transform.GetChild(0).gameObject.SetActive(true);
            }
            button.SetActive(true);
        }

    }
    public void StartPosition() //초기 위치
    {
        //1. 자기위치가 필요하다.
        HexCell[] cell = HexGrid.instance.cells; //필드에 있는 모든 셀의 정보를 가져온다.
        for (int i = 0; i < cell.Length; i++)
        {
            if (1 > Mathf.Abs(cell[i].transform.position.x - this.transform.position.x)) //Abs 절댓값
            {
                if (1 > Mathf.Abs(cell[i].transform.position.z - this.transform.position.z))
                {
                    startPosition = cell[i];
                }
            }
        }

    }
    [System.Serializable]
    public class Node
    {
        public Node(bool _isWall, int _x, int _y)
        {
            isWall = _isWall;
            x = _x;
            y = _y;
        }
        public bool isWall;
        public Node ParentNode;
        public int x, y, G, H;
        public int F { get { return G + H; } }
    }
    Vector2Int bottomLeft, topRight, startPos, targetPos;
    [SerializeField]
    public List<Node> FinalNodeList;
    int sizeX, sizeY;
    Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;
    public static int count;
    int nowTurn = -1, currentTurn = -1;
    public bool PathFinding(int miss)
    {
        StartPosition();
        if (endPosition.coordinates.X >= startPosition.coordinates.X && endPosition.coordinates.Z >= startPosition.coordinates.Z)
        {
            topRight = new Vector2Int(endPosition.coordinates.X + miss, endPosition.coordinates.Z + miss);
            bottomLeft = new Vector2Int(startPosition.coordinates.X - miss, startPosition.coordinates.Z - miss);
        }
        else if (endPosition.coordinates.X < startPosition.coordinates.X && endPosition.coordinates.Z >= startPosition.coordinates.Z)
        {
            topRight = new Vector2Int(startPosition.coordinates.X + miss, endPosition.coordinates.Z + miss);
            bottomLeft = new Vector2Int(endPosition.coordinates.X - miss, startPosition.coordinates.Z - miss);
        }
        else if (endPosition.coordinates.X < startPosition.coordinates.X && endPosition.coordinates.Z < startPosition.coordinates.Z)
        {
            bottomLeft = new Vector2Int(endPosition.coordinates.X - miss, endPosition.coordinates.Z - miss);
            topRight = new Vector2Int(startPosition.coordinates.X + miss, startPosition.coordinates.Z + miss);
        }
        else if (endPosition.coordinates.X >= startPosition.coordinates.X && endPosition.coordinates.Z < startPosition.coordinates.Z)
        {
            bottomLeft = new Vector2Int(startPosition.coordinates.X - miss, endPosition.coordinates.Z - miss);
            topRight = new Vector2Int(endPosition.coordinates.X + miss, startPosition.coordinates.Z + miss);
        }
        startPos = new Vector2Int(startPosition.coordinates.X, startPosition.coordinates.Z);
        targetPos = new Vector2Int(endPosition.coordinates.X, endPosition.coordinates.Z);
        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;
        NodeArray = new Node[sizeX, sizeY];
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                float x;
                if ((j + bottomLeft.y) % 2 == 0)
                {
                    x = (i + bottomLeft.x) * 17.32051f;
                }
                else
                {
                    x = (i + bottomLeft.x) * 17.32051f + 8.160255f;
                }
                foreach (Collider col in Physics.OverlapSphere(new Vector3(x, 0, (j + bottomLeft.y) * 15), 1f))
                {
                    if (cantFindWay == false)
                    {
                        if (upgradeWater == true)
                        {
                            if (col.gameObject.name.Contains("Mountain"))
                            {
                                isWall = true;
                            }
                        }

                        else
                        {
                            if (col.gameObject.name.Contains("Mountain") || col.gameObject.name.Contains("Water"))
                            {
                                isWall = true;
                            }
                        }
                    }

                }
                NodeArray[i, j] = new Node(isWall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }
        //print("startPos = " + startPos + "targetPos = " + targetPos + "sizeX = " + sizeX + "sizeY = " + sizeY + "topRight = " + topRight + "bottomLeft = " + bottomLeft);
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];
        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

        while (OpenList.Count > 0)
        {
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H)
                {
                    CurNode = OpenList[i];
                }
            }
            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();
                for (int i = 0; i < FinalNodeList.Count; i++)
                {
                    print(i + "번째는" + FinalNodeList[i].x + "," + FinalNodeList[i].y);
                }
                if (nowTurn != currentTurn)
                {
                    if (cantFindWay == false)
                    {
                        this.state = State.move;
                        currentTurn = nowTurn;
                    }

                }

                return true;
            }
            if (CurNode.y % 2 == 0)
            {
                OpenListAdd(CurNode.x, CurNode.y + 1);
                OpenListAdd(CurNode.x + 1, CurNode.y);
                OpenListAdd(CurNode.x, CurNode.y - 1);
                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                OpenListAdd(CurNode.x - 1, CurNode.y);
                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
            }
            else
            {
                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                OpenListAdd(CurNode.x + 1, CurNode.y);
                OpenListAdd(CurNode.x + 1, CurNode.y - 1);
                OpenListAdd(CurNode.x, CurNode.y - 1);
                OpenListAdd(CurNode.x - 1, CurNode.y);
                OpenListAdd(CurNode.x, CurNode.y + 1);
            }
            if (OpenList.Count == 0 && CurNode != TargetNode)
            {
                return false;
            }
        }
        return false;
    }

    void OpenListAdd(int checkX, int checkY)
    {
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + Convert.ToInt32(MathF.Round(HexMetrics.innerRadius));
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * Convert.ToInt32(MathF.Round(HexMetrics.innerRadius));
                NeighborNode.ParentNode = CurNode;
                OpenList.Add(NeighborNode);
            }
        }

    }
    private void OnDrawGizmos()
    {
        if (FinalNodeList.Count != 0)
        {
            for (int i = 0; i < FinalNodeList.Count - 1; ++i)
            {
                float x = 0, y = 0, x1 = 0, y1 = 0;

                Gizmos.color = Color.white;
                if (FinalNodeList[i].y % 2 == 0)
                {
                    x = FinalNodeList[i].x * 17.32051f;
                    y = FinalNodeList[i].y * 15;
                    x1 = FinalNodeList[i + 1].x * 17.32051f;
                    y1 = FinalNodeList[i + 1].y * 15;
                    if (FinalNodeList[i + 1].y % 2 == 1)
                    {
                        x1 = FinalNodeList[i + 1].x * 17.32051f + 8.160255f;
                        y1 = FinalNodeList[i + 1].y * 15;
                    }
                }
                else
                {
                    x = FinalNodeList[i].x * 17.32051f + 8.160255f;
                    y = FinalNodeList[i].y * 15;
                    x1 = FinalNodeList[i + 1].x * 17.32051f;
                    y1 = FinalNodeList[i + 1].y * 15;
                    if (FinalNodeList[i + 1].y % 2 == 1)
                    {
                        x1 = FinalNodeList[i + 1].x * 17.32051f + 8.160255f;
                        y1 = FinalNodeList[i + 1].y * 15;
                    }
                }

                Gizmos.DrawLine(new Vector3(x, 1, y), new Vector3(x1, 1, y1));
            }
        }
    }
}