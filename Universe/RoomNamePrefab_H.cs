using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomNamePrefab_H : MonoBehaviour
{
    public string roomName;
    public int myIdx;
    FileManager_L file;
    void Start()
    {
        file = GameObject.Find("FileManager").GetComponent<FileManager_L>();
        GetComponent<Button>().onClick.AddListener(ChangeLayer);
    }
    void ChangeLayer()
    {
        file.ChangeMap(myIdx);
        file.nowMapIdx = myIdx;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
