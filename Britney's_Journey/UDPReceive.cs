using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

[System.Serializable]
public class Data
{
    public Vector2 pos1;
    public Vector2 pos2;
    public Vector2 pos3;
    public int updown;
    public float degree;
    public float distance;
}
[System.Serializable]
public class DataList
{
    public List<Data> dataList = new List<Data>();
}


public class UDPReceive : MonoBehaviour
{
    public static UDPReceive instance;
    public DataList dtList = new DataList();
    public Data dt = new Data();
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    Thread receiveThread;
    UdpClient client;
    public int port = 5052;
    public bool startRecieving = true;
    public bool printToConsole = false;
    public string data;


    public void Start()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void Update()
    {
        Data dd = new Data();
        CopyValue(dt, dd);

        if (dtList.dataList.Count > 30)
        {
            dtList.dataList.RemoveAt(0);
            dtList.dataList.Add(dd);
        }
        else
        {
            dtList.dataList.Add(dd);
        }
    }

    void CopyValue(Data origin, Data copy)
    {
        copy.degree = origin.degree;
        copy.distance = origin.distance;
        copy.pos1 = origin.pos1;
        copy.pos2 = origin.pos2;
        copy.pos3 = origin.pos3;
        copy.updown = origin.updown;
    }


    // receive thread
    private void ReceiveData()
    {
        //IPEndPoint anyI = new IPEndPoint(IPAddress.Any, 0);
        //print(string.Format("My IP  {0}:{1}", anyI.Address, anyI.Port ));
        client = new UdpClient(port);
        while (startRecieving)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                

                byte[] dataByte = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataByte);
                string[] dataArr = data.Split(',');
                dt.pos1 = new Vector2(float.Parse(dataArr[0]),float.Parse(dataArr[1]));
                dt.pos2 = new Vector2(float.Parse(dataArr[2]), float.Parse(dataArr[3]));
                dt.pos3 = new Vector2(float.Parse(dataArr[4]), float.Parse(dataArr[5]));
                dt.updown = int.Parse(dataArr[6]);
                dt.degree = Vector2.Angle((dt.pos1-dt.pos2).normalized,(dt.pos3-dt.pos2).normalized);
                dt.distance = Vector2.Distance(dt.pos2, dt.pos3);
                //if (printToConsole) { print(data); }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }
    public float degreeSpeed = 1;
    public float CalculateDegree() //d1 각도계산
    {
        if(dtList.dataList.Count > 1)
        {
            float a = dtList.dataList[30].degree - dtList.dataList[0].degree;
            return Mathf.Abs(a) * degreeSpeed;
        }
        else
        {
            return 0;
        }
    }
    public float distanceSpeed = 1;
    public float CalculateDistance()
    {
        if (dtList.dataList.Count > 1)
        {
            float a = dtList.dataList[30].distance - dtList.dataList[0].distance;
            return Mathf.Abs(a) * distanceSpeed;
        }
        else
        {
            return 0;
        }
    }
}