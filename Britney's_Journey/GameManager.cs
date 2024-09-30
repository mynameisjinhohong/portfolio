using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        
    }
    public float time = 0;
    public float d1Speed = 1;
    public DataList dataList = new DataList();
    public Data data = new Data();
    public float MapSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        string jsonData = File.ReadAllText((Application.dataPath + "/tmpJson.txt"));
        data = JsonUtility.FromJson<Data>(jsonData);
        //data.d1 += 2f;
        jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText((Application.dataPath + "/tmpJson.txt"), jsonData);
        if (dataList.dataList.Count > 1)
        {
            dataList.dataList.RemoveAt(0);
            dataList.dataList.Add(data);
        }
        else
        {
            dataList.dataList.Add(data);
        }
        
        
    }
    #region 테스트용
    public void Up()
    {
        string jsonData = File.ReadAllText((Application.dataPath + "/tmpJson.txt"));
        data = JsonUtility.FromJson<Data>(jsonData);
        data.updown = 1;
        jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText((Application.dataPath + "/tmpJson.txt"), jsonData);
    }
    public void Down()
    {
        string jsonData = File.ReadAllText((Application.dataPath + "/tmpJson.txt"));
        data = JsonUtility.FromJson<Data>(jsonData);
        data.updown = -1;
        jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText((Application.dataPath + "/tmpJson.txt"), jsonData);
    }
    public void Run()
    {
        string jsonData = File.ReadAllText((Application.dataPath + "/tmpJson.txt"));
        data = JsonUtility.FromJson<Data>(jsonData);
        data.updown = 0;
        jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText((Application.dataPath + "/tmpJson.txt"), jsonData);
    }

    #endregion

    public float CalculateDegree() //d1 각도계산
    {
        //float a = dataList.dataList[1].d1 - dataList.dataList[0].d1;
        //return Mathf.Abs(a) * d1Speed;
        return 2;
    }



}
