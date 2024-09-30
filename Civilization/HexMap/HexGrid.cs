using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public GameObject ui;
    // Start is called before the first frame update
    public int width = 6;
    public Text cellLabelPrefab;
    Canvas gridCanvas;
    public int height = 6;
    public HexCell[] cellPrefab;
    [SerializeField]
    public HexCell[] cells;
    HexMesh hexMesh;
    public Color defaultColor = Color.white;
    public static HexGrid instance = null;
    
    void Awake()
    {
        //ui.transform.SetAsLastSibling();
        if(instance == null)
        {
            instance = this;
        }
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        cells = new HexCell[height * width];
        
        for(int z = 0,i = 0; z < height; z++)
        {
            for(int x = 0; x<width; x++)
            {
                CreateCell(x, z, i++,0);        
            }
        }
        ui.SetActive(false);
    }
    void Start()
    {
        if (PlayerPrefs.GetString("Map") != null)
        {
            LoadMap();
        }
        //hexMesh.Triangulate(cells);
    }
    void CreateCell(int x, int z, int i, int what)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z/2) *(HexMetrics.innerRadius *2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);
        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab[what]);
        cell.transform.SetParent(transform,false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;
        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if (x < width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                }
            }
        }

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
        

    }
    public void SaveMap()
    {
        int[] num = new int[height * width];
        string strArr = "";
        for(int i = 0; i< num.Length; i++)
        {
            if (cells[i].name.Contains("Ground1"))
            {
                strArr += 0;
                
            }
            else if(cells[i].name.Contains("Ground2"))
            {
                strArr += 1;
            }
            else if (cells[i].name.Contains("Ground3"))
            {
                strArr += 2;
            }
            else if (cells[i].name.Contains("Ground4"))
            {
                strArr += 3;
            }
            else if (cells[i].name.Contains("Mountain"))
            {
                strArr += 4;
            }
            else if (cells[i].name.Contains("Water1"))
            {
                strArr += 5;
            }
            else if (cells[i].name.Contains("Water2"))
            {
                strArr += 6;
            }
            if (i < num.Length - 1)
            {
                strArr += ",";
            }
        }
        PlayerPrefs.SetString("Map", strArr);
        File.WriteAllText(Application.dataPath + "/map.txt", strArr);
    }

    public void LoadMap()
    {
        int a = 0;
        string dataStr = File.ReadAllText((Application.dataPath + "/map.txt"));
        if (dataStr == null || dataStr.Length < 1)
        {
            return;
        }
        //string[] dataArr = PlayerPrefs.GetString("Map").Split(',');
        string[] dataArr = dataStr.Split(',');
        int[] num = new int[dataArr.Length];
        for(int i =0; i< dataArr.Length; i++)
        {
            num[i] = System.Convert.ToInt32(dataArr[i]);
            if(cells[i] != null)
            {

                Destroy(cells[i].gameObject);
            }

        }
        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++, num[a]);
                a++;
            }
        }

    }

    // Update is called once per frame
    //public void ColorCell(Vector3 position,Color color)
    //{
    //    position = transform.InverseTransformPoint(position);
    //    HexCoordinates coordinates = HexCoordinates.FromPosition(position);
    //    int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
    //    HexCell cell = cells[index];
    //    cell.color = color;
    //    hexMesh.Triangulate(cells);
    //}
}