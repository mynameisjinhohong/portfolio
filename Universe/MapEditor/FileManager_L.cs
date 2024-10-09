using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using SFB;
using Photon.Pun;
using Photon.Realtime;

public class MapUpdate
{
    public int status;
    public string message;
    public string data;
}

public class FileManager_L : MonoBehaviourPunCallbacks
{
    public string testMapName;

    private string reportURL = "/auths/hate";
    public string changeSpaceURL = "/spaces/maps/update/";
    public string setBGMURL = "/spaces/musics/upload";
    public string deleteBGMURL = "/spaces/musics/delete";
    public string BGMGetURL = "/spaces/music";
    public string putSpaceInfo = "/spaces/upload";

    public MeshRenderer bg;
    public MeshRenderer fg;
    string path = "";
    string savepth;
    public int mapSize = 0;
    public MapEditor_L mapEditor;
    public List<MapInfo> mapinfos;
    public List<RoomNamePrefab_H> rooms;
    //public Dropdown mapDropdown;
    public InputField mapNameInputField;
    public Toggle mapSmallToggle;

    public InputField mapTagInputField1;
    public InputField mapTagInputField2;
    public InputField mapTagInputField3;
    public int nowMapIdx = 0;
    public int mapSu;
    [Header("�ε�")]
    public GameObject panelLoading;

    [Header("�� ���̾�")]
    public GameObject roomLayerPrefab;
    public Transform roomLayerParent;

    public Text wrongWordText;
    // Start is called before the first frame update
    void Start()
    {
        //mapinfos.Add(new MapInfo());
        //mapDropdown.onValueChanged.AddListener(ChangeMap);
        if(SpaceInfo.spaceURL == null)
        {
            SpaceInfo.spaceURL = testMapName;
        }
        savepth = Application.dataPath + "/Resources/Resources_H/MapData";
        //mapNameInputField.onSubmit.AddListener(MakeMap);
        StartCoroutine(GetBGMData());
    }
    IEnumerator GetBGMData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(GameManager.instance.url + BGMGetURL))
        {
            request.SetRequestHeader("spaceCode", GameManager.instance.mapData.spaceCode.ToString());
            Debug.Log(GameManager.instance.mapData.spaceCode.ToString());
            Debug.Log(GameManager.instance.url + BGMGetURL);
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                MusicURLWhole result = JsonUtility.FromJson<MusicURLWhole>(System.Text.Encoding.Default.GetString(request.downloadHandler.data));
                StartCoroutine(GetBGM(result.data.musicUrl));
            }
        }
    }
    IEnumerator GetBGM(string url)
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(request);
                GetComponent<AudioSource>().clip = myClip;
                GetComponent<AudioSource>().Play();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void MapMakeInputFieldOn()
    {
        mapNameInputField.gameObject.SetActive(true);
    }
    public void MapMakeInputFieldOff()
    {
        mapNameInputField.gameObject.SetActive(false);
    }
    public string WriteResult(string[] paths)
    {
        string result = "";
        if (paths.Length == 0)
        {
            return "";
        }
        foreach (string p in paths)
        {
            result += p + "\n";
        }
        return result;
    }
    AudioClip bgm;

    public void SetBgmButton()
    {
        path = WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
        StartCoroutine(SetBGM());
    }
    IEnumerator SetBGM()
    {
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + path, AudioType.MPEG);
        Debug.Log("file:///" + path);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("���� ����");
            bgm = DownloadHandlerAudioClip.GetContent(www);
            GetComponent<AudioSource>().clip = bgm;
            GetComponent<AudioSource>().Play();
            StartCoroutine(WebRequest_PostBGMURL(www.downloadHandler.data));
        }
    }
    public IEnumerator WebRequest_PostBGMURL(byte[] data)
    {
        //AudioSource go = GetComponent<AudioSource>();
        //float[] test = new float[go.clip.samples * go.clip.channels];
        //go.clip.GetData(test, 0);
        //Debug.Log(test.Length);
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddBinaryData("file", data);
        //wwwForm.AddBinaryData("file",)
        using (UnityWebRequest request = UnityWebRequest.Post(GameManager.instance.url + setBGMURL, wwwForm))
        {
            request.uploadHandler.contentType = "multipart/form-data";
            //request.SetRequestHeader("Authorization", GameManager.instance.userinfo.token);
            request.SetRequestHeader("spaceCode", GameManager.instance.mapData.spaceCode.ToString());
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("BGM올리기 성공!");

                //SceneManager.LoadScene("RoomScene_H");
                //StartCoroutine(WebRequest_PutSpaceInfo());
            }
            request.Dispose();
        }
    }

    public IEnumerator WebRequest_DeleteBGMURL()
    {
        using (UnityWebRequest request = UnityWebRequest.Post(GameManager.instance.url + deleteBGMURL,""))
        {
            //request.uploadHandler.contentType = "multipart/form-data";
            request.SetRequestHeader("spaceCode", GameManager.instance.mapData.spaceCode.ToString());
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("BGM삭제 성공!");
                GetComponent<AudioSource>().clip = null;
                //SceneManager.LoadScene("RoomScene_H");
                //StartCoroutine(WebRequest_PutSpaceInfo());
            }
            request.Dispose();
        }
    }
    public void DeleteBGM()
    {
        StartCoroutine(WebRequest_DeleteBGMURL());
    }
    public void OnClickDeleteBG()
    {
        Texture2D tex = new Texture2D(1, 1);
        bg.material.SetTexture("_MainTex", tex);
    }
    public void OnClickSetBG()
    {
        //path = EditorUtility.OpenFilePanel("Show all images(.png)", "", "png");
        path = WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
        StartCoroutine(IESetBG());
    }
    public void OnClickDeleteFG()
    {
        Texture2D tex = new Texture2D(1, 1);
        fg.material.SetTexture("_MainTex", tex);
    }

    public void OnClickSetFG()
    {
        path = WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
        //path = EditorUtility.OpenFilePanel("Show all images(.png)", "", "png");

        StartCoroutine(IESetFG());
    }

    IEnumerator IESetBG()
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Texture2D convertedTexture = (Texture2D)myTexture;
            byte[] textuerData = convertedTexture.EncodeToPNG();
            if (Directory.Exists(savepth) == false)
            {
                Directory.CreateDirectory(savepth);
            }
            File.WriteAllBytes(savepth + "/mapData.png", textuerData);
            bg.material.shader = Shader.Find("Unlit/Texture");
            bg.material.SetTexture("_MainTex", convertedTexture);
        }
    }

    IEnumerator IESetFG()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Texture2D convertedTexture = (Texture2D)myTexture;
            if(convertedTexture.width < 9)
            {
                fg.material.shader = Shader.Find("UI/Unlit/Transparent");
            }
            else
            {
                fg.material.shader = Shader.Find("Unlit/Transparent");
            }
            fg.material.SetTexture("_MainTex", convertedTexture);
        }
    }
    public void ChangeMap(int option)
    {
        for(int i =0; i < rooms.Count; i++)
        {
            rooms[i].gameObject.GetComponent<Image>().color = new Color(1, 1, 1,1);
        }
        rooms[option].GetComponent<Image>().color = new Color(0.5f, 0.3f, 0.4f, 0.3f);
        SaveMap(mapinfos[nowMapIdx].mapName,nowMapIdx);
        Debug.Log(nowMapIdx);
        nowMapIdx = option;
        Debug.Log(nowMapIdx);
        mapEditor.ReturnZero();
        Debug.Log(mapinfos[option].mapName);
        mapEditor.ChangeMap(mapinfos[option]);
    }

    public void SaveSpace()
    {

        string allText = mapTagInputField1.text + "\n" + mapTagInputField2.text + "\n" + mapTagInputField3.text + "\n";
        for(int i =0; i<mapinfos.Count; i++)
        {
            allText += mapinfos[i].mapName + "\n";
        }
        StartCoroutine(WebRequest_ReportPost(allText));
        //�� ���� ����ǥ�� üũ �ϰ� ���� �� �ؿ��͵� ����

    }
    public IEnumerator WebRequest_ReportPost(string chat)
    {
        WWWForm www = new WWWForm();
        //www.AddBinaryData("text", System.Text.Encoding.UTF8.GetBytes(chat));
        www.AddField("text", chat);
        using (UnityWebRequest request = UnityWebRequest.Post(GameManager.instance.url + reportURL, www))
        {
            //request.SetRequestHeader("Authorization", GameManager.instance.userinfo.token);
            //request.SetRequestHeader("Content-Type", "application/json");
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                AIResPondWord ai = JsonUtility.FromJson<AIResPondWord>(request.downloadHandler.text);
                if (ai.data)
                {
                    StartCoroutine(WebRequest_PutSpaceInfo());
                }
                else
                {
                    wrongWordText.gameObject.SetActive(true);
                }
                Debug.Log("����ǥ�� ���� ����!" + ai.data);
            }
            request.Dispose();
        }
    }
    public void MakeMap()
    {
        if (mapSu < 3 && mapNameInputField.text.Length > 0)
        {
            mapSu++;
            MapInfo newMap = new MapInfo();

            mapinfos.Add(newMap);
            GameObject go = Instantiate(roomLayerPrefab);
            go.transform.SetParent(roomLayerParent, true);
            RoomNamePrefab_H room = go.GetComponent<RoomNamePrefab_H>();
            room.myIdx = mapSu-1;
            room.roomName = mapNameInputField.text;
            rooms.Add(room);
            for (int i = 0; i < rooms.Count; i++)
            {
                rooms[i].gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
            }
            go.GetComponent<Image>().color = new Color(0.5f, 0.3f, 0.4f, 0.3f);
            go.transform.GetChild(0).GetComponent<Text>().text = mapNameInputField.text;
            mapNameInputField.text = "";
            //SaveMap(mapinfos[nowMapIdx].mapName, nowMapIdx);
            mapEditor.ReturnZero();

            newMap.mapName = mapNameInputField.text;
            newMap.mapSize = (MapInfo.MapSize)mapSize;
            newMap.backGroundImage = new byte[0];
            newMap.foreGroundImage = new byte[0];
            mapEditor.ChangeMap(newMap);
            //mapEditor.ChangeMap(mapinfos[mapinfos.Count -1]);
            nowMapIdx = mapinfos.Count - 1;
            //Dropdown.OptionData option = new Dropdown.OptionData();
            //option.text = txt;
            //mapDropdown.options.Add(option);
            //mapDropdown.value = mapDropdown.options.Count - 1;
        }
    }

    public void SaveButton()
    {
        SaveMap(mapinfos[nowMapIdx].mapName, nowMapIdx);
    }

    public IEnumerator WebRequest_PostSpaceURL(string json)
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddBinaryData("file", System.Text.Encoding.UTF8.GetBytes(json));
        byte[] forms = wwwForm.data;
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormFileSection(json, "file.txt"));
        using (UnityWebRequest request = UnityWebRequest.Post(GameManager.instance.url + changeSpaceURL +SpaceInfo.spaceCode,wwwForm))
        {
            request.uploadHandler.contentType = "multipart/form-data";
            //request.SetRequestHeader("Authorization", GameManager.instance.userinfo.token);
            //request.SetRequestHeader("Content-Type","multipart/form-data");
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(GameManager.instance.url + changeSpaceURL + SpaceInfo.spaceCode);
                Debug.Log(request.error);
            }
            else
            {
                MapUpdate result = JsonUtility.FromJson<MapUpdate>(System.Text.Encoding.Default.GetString(request.downloadHandler.data));
                SpaceInfo.spaceURL = result.data;
                JoinUserRoom();
                //SceneManager.LoadScene("RoomScene_H");
                //StartCoroutine(WebRequest_PutSpaceInfo());
            }
            request.Dispose();
        }
    }

    void JoinUserRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 14;
        roomOptions.IsVisible = true;
        print("JoinUserRoom");
        PhotonNetwork.JoinOrCreateRoom(SpaceInfo.spaceURL, roomOptions, null);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        if (SceneManager.GetActiveScene().name == "MapEditorScene_H")
            PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        if (SceneManager.GetActiveScene().name == "MapEditorScene_H")
            JoinUserRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.AutomaticallySyncScene = false;
        print("FileManager 1");
        SceneManager.LoadScene("RoomScene_H");


    }

    public IEnumerator WebRequest_PutSpaceInfo()
    {
        MapPutData mapPut = new MapPutData();
        mapPut.spaceName = GameManager.instance.mapData.spaceName;
        mapPut.spaceCode = GameManager.instance.mapData.spaceCode;
        mapPut.sapcePassword = GameManager.instance.mapData.sapcePassword;
        mapPut.spaceType = GameManager.instance.mapData.spaceType;
        mapPut.spaceIntro = GameManager.instance.mapData.spaceIntro;
        mapPut.spaceTag1 = mapTagInputField1.text;
        mapPut.spaceTag2 = mapTagInputField2.text;
        mapPut.spaceTag3 = mapTagInputField3.text;
        string json = JsonUtility.ToJson(mapPut);
        GameManager.instance.mapData.spaceTag1 = mapPut.spaceTag1;
        GameManager.instance.mapData.spaceTag2 = mapPut.spaceTag2;
        GameManager.instance.mapData.spaceTag3 = mapPut.spaceTag3;
        using (UnityWebRequest request = UnityWebRequest.Put(GameManager.instance.url + putSpaceInfo, json))
        {
            //request.uploadHandler.contentType = "multipart/form-data";
            //request.SetRequestHeader("Authorization", GameManager.instance.userinfo.token);
            request.SetRequestHeader("Content-Type", "application/json");
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(GameManager.instance.url + changeSpaceURL + SpaceInfo.spaceCode);
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("맵 데이터 올리기 성공!");
                SpaceInfo spaceInfo = new SpaceInfo();
                spaceInfo.mapList = mapinfos;
                string jsonMap = JsonUtility.ToJson(spaceInfo);
                StartCoroutine(WebRequest_PostSpaceURL(jsonMap));
            }
            request.Dispose();
        }
    }
    public void SaveMap(string mapName,int whatInfo)
    {
        panelLoading.SetActive(true);

        #region �� ���� ����
        MapInfo backGroundInfo = new MapInfo();
        backGroundInfo.mapName = mapName;
        Texture2D mainTex = (Texture2D)bg.material.mainTexture;


        backGroundInfo.backGroundImage = mainTex.EncodeToPNG();
        Texture2D foreMainTex = (Texture2D)fg.material.mainTexture;
        backGroundInfo.foreGroundImage = foreMainTex.EncodeToPNG();
        string path = Application.dataPath + "/Resources/Resources_H/MapData";
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }

        #endregion
        #region �� �븻 Ÿ�� ����
        GameObject tileParent = GameObject.Find("TileParent");
        int a = tileParent.transform.childCount;
        List<TileInfo> tileInfos = new List<TileInfo>();
        for (int i =0; i < a; i++)
        {
            GameObject tile = tileParent.transform.GetChild(i).gameObject;
            TileInfo_H tileInfo_h = tile.GetComponent<TileInfo_H>();
            TileInfo tileInfo = tileInfo_h.tileInfo;

                tileInfo.imageName = tile.GetComponent<SpriteRenderer>().sprite.name;
            tileInfo.position = tile.transform.position;
            tileInfos.Add(tileInfo);
        }
        backGroundInfo.tileList = tileInfos;
#endregion
#region �� �������� ����
        GameObject definedArea = GameObject.Find("DefinedAreaParent");
        List<string> nameList = new List<string>();
        List<DefinedAreaInfo> definedAreaInfos = new List<DefinedAreaInfo>();
        for(int i =0; i<definedArea.transform.childCount; i++)
        {
            GameObject gm = definedArea.transform.GetChild(i).gameObject;
            nameList.Add(gm.name);
            for(int j =0; j<gm.transform.childCount; j++)
            {
                DefinedAreaInfo de = new DefinedAreaInfo();
                de.areaName = gm.name;
                de.positon = gm.transform.GetChild(j).localPosition;
                definedAreaInfos.Add(de);
            }
        }
        backGroundInfo.definedAreaList = definedAreaInfos;
        backGroundInfo.areaName = nameList;
#endregion
#region �� ��Ż ����
        GameObject portal = GameObject.Find("PortalParent");
        if(portal != null)
        {
            List<PortalInfo> portalInfoList = new List<PortalInfo>();

            for (int i = 0; i < portal.transform.childCount; i++)
            {

                Portal2D_L portalL = portal.transform.GetChild(i).GetComponent<Portal2D_L>();
                portalInfoList.Add(portalL.portalInfo);
                //portalL.portalInfo.position = portal.transform.GetChild(i).localPosition;

            }
            backGroundInfo.portalList = portalInfoList;
        }

#endregion
#region �� ��(�̵��Ұ��� ����)����
        GameObject wall = GameObject.Find("WallParent");
        List<WallInfo> wallInfos = new List<WallInfo>();
        for(int i =0; i<wall.transform.childCount; i++)
        {
            WallInfo wallInfo = new WallInfo();
            wallInfo.positon = wall.transform.GetChild(i).position;
            wallInfos.Add(wallInfo);
        }
        backGroundInfo.wallList = wallInfos;
#endregion
#region ���� ���� ����
        Transform spawnPointParent = GameObject.Find("SpawnPointParent").transform;
        if (spawnPointParent)
        {
            List<SpawnPointInfo> spawnPointInfoList = new List<SpawnPointInfo>();
            for(int i = 0; i < spawnPointParent.childCount; i++)
            {
                SpawnPointInfo info = new SpawnPointInfo();
                info.position = spawnPointParent.GetChild(i).localPosition;
                spawnPointInfoList.Add(info);

            }
            backGroundInfo.spawnPointInfoList = spawnPointInfoList;
        }
#endregion
#region ������Ʈ ����
        GameObject objectP = GameObject.Find("ObjectParent");
        List<ObjectInfo> objectInfos = new List<ObjectInfo>();
        for(int i =0; i < objectP.transform.childCount; i++)
        {
            objectInfos.Add(objectP.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ObjectInfo_H>().objectInfo);
        }
        backGroundInfo.objectList = objectInfos;
        #endregion
        #region ȭ��ä�� ����
        GameObject facechatParent = GameObject.Find("FaceChatParent");
        List<FaceChatInfo> facechatInfos = new List<FaceChatInfo>();
        for(int i =0; i<facechatParent.transform.childCount; i++)
        {
            GameObject go = facechatParent.transform.GetChild(i).gameObject;
            for(int j =0; j<go.transform.childCount; j++)
            {
                FaceChatInfo info = new FaceChatInfo();
                info.channelName = go.name;
                info.postion = go.transform.GetChild(j).localPosition;
                facechatInfos.Add(info);
            }
        }
        backGroundInfo.faceChatList = facechatInfos;
        #endregion
        #region �ʻ����� ����
        Debug.Log(mapSize);
        switch (mapSize)
        {
            case 0:
                backGroundInfo.mapSize = MapInfo.MapSize.Small;
                break;
            case 1:
                backGroundInfo.mapSize = MapInfo.MapSize.Mideum;
                break;
            case 2:
                backGroundInfo.mapSize = MapInfo.MapSize.Big;
                break;
        }
        #endregion

        #region 3D ������Ʈ ����
        GameObject _3DobjectP = GameObject.Find("_3DObjectParent");
        print(_3DobjectP.name);
        List<_3DObjectInfoClass> _3DObjectInfoClassList = new List<_3DObjectInfoClass>();
        for (int i = 0; i < _3DobjectP.transform.childCount; i++)
        {
            _3DObjectInfoClass new3DObjectInfo = new _3DObjectInfoClass();
            _3DObjectInfo_L this3DObjectInfo = _3DobjectP.transform.GetChild(i).GetComponent<_3DObjectInfo_L>();
            new3DObjectInfo.myName = this3DObjectInfo.myName;
            new3DObjectInfo.texture = this3DObjectInfo.texture;
            new3DObjectInfo.objPath = this3DObjectInfo.objPath;
            new3DObjectInfo.pos = this3DObjectInfo.pos;
            new3DObjectInfo._3DObjectByteArray = this3DObjectInfo._3DObjectByteArray;

            _3DObjectInfoClassList.Add(new3DObjectInfo);
            print(i+" : "+_3DobjectP.transform.GetChild(i).name);
            //_3DobjectInfos.Add(_3DobjectP.transform.GetChild(i).GetComponent<_3DObjectInfo_L>());
        }
        backGroundInfo._3DObjectList = _3DObjectInfoClassList;
        #endregion
        mapinfos[whatInfo] = backGroundInfo;
        //mapinfos.Add(backGroundInfo);
        //spaceInfo.mapList = mapinfos;
        //string jsonMap = JsonUtility.ToJson(spaceInfo);
        //File.WriteAllText(path + "/" + SpaceInfo.spaceName + ".txt", jsonMap);
    }
}
