using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Dummiesman;
using UnityEngine.Networking;

[System.Serializable]
public class SpaceInfo
{
    public List<MapInfo> mapList;
    public string makerID;
    public static string spaceURL;
    public static int spaceCode;
    public string spaceName;
    public string spaceIntroduction;
    public string passWord;
}
[System.Serializable]
public class FaceChatInfo
{
    public string channelName;
    public Vector3 postion;
}

[System.Serializable]
public class MapInfo
{
    public static string nowMapName;
    public string mapName;
    public enum MapSize
    {
        Big,
        Mideum,
        Small
    }
    public MapSize mapSize;
    public byte[] backGroundImage;
    public byte[] foreGroundImage;
    public List<TileInfo> tileList;
    public List<string> areaName;
    public List<WallInfo> wallList;
    public List<PortalInfo> portalList;
    public List<DefinedAreaInfo> definedAreaList;
    public List<ObjectInfo> objectList;
    public List<SpawnPointInfo> spawnPointInfoList;
    public List<_3DObjectInfoClass> _3DObjectList;
    public List<FaceChatInfo> faceChatList;

    public MapInfo()
    {
        tileList = new List<TileInfo>();
        areaName = new List<string>();
        wallList = new List<WallInfo>();
        portalList = new List<PortalInfo>();
        definedAreaList = new List<DefinedAreaInfo>();
        objectList = new List<ObjectInfo>();
        spawnPointInfoList = new List<SpawnPointInfo>();
        _3DObjectList = new List<_3DObjectInfoClass>();
        faceChatList = new List<FaceChatInfo>();
    }
}
[System.Serializable]
public class ObjectInfo
{
    public ObjectInfo()
    {
        objectSize = new Vector2(1f, 1f);
    }

    public string objName;
    public int objWidth = 1;
    public int objHeight = 1;
    public Vector3 Position;
    public Vector2 objectSize;

    public enum ObjectType
    {
        Text,
        Image,
    }
    public ObjectType objType;
    public bool upperObj;
    public byte[] image;
    public string text;
    public enum ObjectSkill
    {
        nomalObj,
        urlObj,
        changeObj,
        talkingObj,
    }
    public ObjectSkill objSkill;
    public string urlSkill;
    public byte[] changeSkill;
    public string talkingSkill;
    //애니메이션 어케해야할지 고민중
    public string textSkill;
    public byte[] imageSkill;

    public enum InteractionType
    {
        pressF,
        touch,
    }
    public InteractionType interactionType;
}
[System.Serializable]
public class WallInfo
{
    public Vector3 positon;
}
[System.Serializable]
public class DefinedAreaInfo
{
    public string areaName;
    public Vector3 positon;
}

[System.Serializable]
public class TileInfo
{
    public Vector3 position;
    public string imageName;
}

[System.Serializable]
public class SpawnPointInfo
{
    public Vector3 position;
}

[System.Serializable]
public class Userinfo
{
    public string id;
    public string token;
    public string password;
    public string name;
    public string nickname;
    public string phoneNumber;
    public string departmentName;
    public int memberCap;
    public int memberCherry;
    

    public AvatarSet avatarSet; //아바타 뭐 입었는지

    public string majorConcentration;
    public string majorKeyword;
    public string majorSpecialty;
    public string majorHopePath;

    public string[] URLName = new string[5];
    public string[] URLs = new string[5];
}

[System.Serializable]
public class UserAvatar
{
    public int memberCode;
    public int itemHairCode;
    public int itemFaceCode;
    public int itemClothCode;
}
[System.Serializable]
public class MusicURLWhole
{
    public int status;
    public string message;
    public MusicURL data;
}

[System.Serializable]
public class MusicURL
{
    public int spaceCode;
    public string musicUrl;
}

/*[System.Serializable]
public class _3DObjectInfo
{
    public string 
}*/
public class RoomManager_H : MonoBehaviourPunCallbacks
{
    public static RoomManager_H instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public SpaceInfo spaceInfo;
    //public MapInfo mapInfo;
    public GameObject wallPrefab;
    public GameObject tilePrefab;
    public GameObject definedAreaPrefab;
    public GameObject portalPrefab;
    public GameObject spawnPointPrefab;
    public GameObject objectPrefab;
    public GameObject facechatPrefab;

    public List<Vector3> spawnPointPosList; //로드 시, 리스트에 위치 넣어두고 랜덤 위치에 캐릭터 옮김
    public Material invisible;
    public MeshRenderer bg;
    public GameObject quadBG;
    public GameObject quadFG;
    public MeshRenderer fg;

    public GameObject player;
    public Transform playerSpawnPos;

    public float zValueUpper = 100f;
    public float zValueLower = 0f;
    public float zValue3DObj = 50f;
    public float zValuePlayer = 1f;

    bool isTokenSet = false;
    CharacterMove_H characterMove;
    GameManager gameManager;
    GameManager_L gameManager_L;

    public bool goLobby = false;
    public bool goInfo = false;
    public bool goMapEditor = false;

    public string BGMGetURL = "/spaces/music";

    [Header("로딩")]
    public GameObject panelLoading;

    IEnumerator GetMapData()
    {
        using(UnityWebRequest request = UnityWebRequest.Get(SpaceInfo.spaceURL))
        {
            yield return request.SendWebRequest();
            if(request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                SpaceInfo result = JsonUtility.FromJson<SpaceInfo>(System.Text.Encoding.Default.GetString(request.downloadHandler.data));
                spaceInfo = result;
                print("GetMapData 끝");
                SetMap();
            }
        }

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
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url,AudioType.MPEG))
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
    // Start is called before the first frame update
    void Start()
    {
        panelLoading.SetActive(true);
        
        PhotonNetwork.IsMessageQueueRunning = true;
        print("Player Length : " + PhotonNetwork.PlayerList.Length);
        StartCoroutine(GetMapData());
        StartCoroutine(GetBGMData());
        print("방에 있나요 "+PhotonNetwork.InRoom);
        print("방 이름 : " + PhotonNetwork.CurrentRoom.Name);
        //Debug.Log(SpaceInfo.spaceURL);
        //string bgpath = Application.dataPath + "/Resources/Resources_H/MapData/"+ SpaceInfo.spaceURL +".txt";
        //string jsonData = File.ReadAllText(bgpath);
        //spaceInfo = JsonUtility.FromJson<SpaceInfo>(jsonData);
        upperUI = GameObject.FindGameObjectWithTag("UpperUI").GetComponent<UpperUI_H>();

    }

    UpperUI_H upperUI;
    void SetMap() {


        #region 배경화면 가져오기
        //MapInfo info = JsonUtility.FromJson<MapInfo>(jsonData);
        MapInfo info = new MapInfo();
        for (int i = 0; i < spaceInfo.mapList.Count; i++)
        {
            Debug.Log("이거야 이거~~~~~    " + spaceInfo.mapList[i].mapName + "  " + MapInfo.nowMapName);
            //Debug.Log(spaceInfo.mapList[i].mapName);
            if (spaceInfo.mapList[i].mapName == MapInfo.nowMapName)
            {
                info = spaceInfo.mapList[i];
                Debug.Log("mynameis" + MapInfo.nowMapName);
                break;
            }
            if (i == spaceInfo.mapList.Count - 1)
            {
                info = spaceInfo.mapList[0];
                MapInfo.nowMapName = info.mapName;
            }
        }
        //mapInfo = info;
        if (info.mapSize == MapInfo.MapSize.Big)
        {
            quadBG.transform.localScale = new Vector3(180, 90, 1);
            quadFG.transform.localScale = new Vector3(180, 90, 1);
        }
        else if (info.mapSize == MapInfo.MapSize.Mideum)
        {
            quadBG.transform.localScale = new Vector3(120, 60, 1);
            quadFG.transform.localScale = new Vector3(120, 60, 1);
        }
        else
        {
            quadBG.transform.localScale = new Vector3(60, 30, 1);
            quadFG.transform.localScale = new Vector3(60, 30, 1);
        }
        Texture2D bgTexture = new Texture2D(0, 0);
        bgTexture.LoadImage(info.backGroundImage);
        if (bgTexture.width < 9)
        {
            Texture2D tex = Resources.Load<Texture2D>("Resources_H/MapData/bg(grey)");
            bg.material.SetTexture("_MainTex", tex);
        }
        else
        {
            bg.material.SetTexture("_MainTex", bgTexture);
        }
        bg.material.shader = Shader.Find("Unlit/Texture");
        Texture2D fgTexture = new Texture2D(0, 0);
        fgTexture.LoadImage(info.foreGroundImage);
        fg.material.shader = Shader.Find("Unlit/Transparent");
        if(fgTexture.width < 9)
        {
            Texture2D tex = Resources.Load<Texture2D>("Resources_H/MapData/bg(trans)");
            fg.material.SetTexture("_MainTex", tex);
        }
        else
        {
            
            fg.material.SetTexture("_MainTex", fgTexture);
        }
        #endregion
        #region 타일 가져오기
        string tileParent = "TileParent";
        for (int i = 0; i < info.tileList.Count; i++)
        {
            Vector3 tilePos = info.tileList[i].position;
            //Texture tileSprite = Resources.Load<Texture>("Resources_L/" + info.tileList[i].imageName);
            Sprite tileSprite = Resources.Load<Sprite>("Resources_L/MapEditor/" + info.tileList[i].imageName);
            GameObject realTile = Instantiate(tilePrefab);
            realTile.transform.position = tilePos;
            //realTile.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", tileSprite);
            realTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
            realTile.transform.parent = GameObject.Find(tileParent).transform;
        }
        #endregion
        #region 포탈 가져오기
        Transform portalParent = GameObject.Find("PortalParent").transform;
        if (portalParent)
        {
            for (int i = 0; i < info.portalList.Count; i++)
            {
                //Vector3 tilePos = info.portalList[i].position;
                GameObject myPortal = Instantiate(portalPrefab);
                Destroy(myPortal.transform.GetChild(0).gameObject);
                Destroy(myPortal.GetComponent<MeshRenderer>());
                Destroy(myPortal.GetComponent<MeshFilter>());
                Portal2D_L portal2D = myPortal.GetComponent<Portal2D_L>();
                portal2D.portalInfo = new PortalInfo();
                myPortal.transform.parent = portalParent;
                myPortal.transform.localPosition = info.portalList[i].position;

                portal2D.portalInfo.position = myPortal.transform.localPosition;
                portal2D.portalInfo.placeType = info.portalList[i].placeType;
                portal2D.portalInfo.moveType = info.portalList[i].moveType;
                portal2D.portalInfo.definedAreaName = info.portalList[i].definedAreaName;
                portal2D.portalInfo.mapName = info.portalList[i].mapName;
            }
        }
        #endregion
        #region 지정구역 가져오기
        GameObject areaParent = GameObject.Find("DefinedAreaParent");
        Dictionary<string, int> nameDic = new Dictionary<string, int>();
        for (int i = 0; i < info.areaName.Count; i++)
        {
            GameObject child = new GameObject(info.areaName[i]);
            child.transform.parent = areaParent.transform;
            nameDic.Add(info.areaName[i], i);
        }
        for (int i = 0; i < info.definedAreaList.Count; i++)
        {
            GameObject area = Instantiate(definedAreaPrefab);
            Destroy(area.transform.GetChild(0).gameObject);
            Destroy(area.GetComponent<MeshRenderer>());
            Destroy(area.GetComponent<MeshFilter>());
            area.transform.position = info.definedAreaList[i].positon;
            area.GetComponentInChildren<Text>().text = info.definedAreaList[i].areaName;
            area.transform.parent = areaParent.transform.GetChild(nameDic[info.definedAreaList[i].areaName]);
        }

        #endregion
        #region 벽 가져오기
        for (int i = 0; i < info.wallList.Count; i++)
        {
            Vector3 wallPos = info.wallList[i].positon;
            GameObject wall = Instantiate(wallPrefab);
            Destroy(wall.GetComponent<MeshRenderer>());
            Destroy(wall.GetComponent<MeshFilter>());
            wall.transform.position = wallPos;
            //wall.GetComponent<MeshRenderer>().material.mainTexture = invisibleTexture;
            wall.transform.parent = GameObject.Find("WallParent").transform;
        }
        #endregion
        #region 스폰 지점 가져오기
        Transform spawnPointParent = GameObject.Find("SpawnPointParent").transform;
        if (spawnPointParent)
        {
            spawnPointPosList = new List<Vector3>();

            for (int i = 0; i < info.spawnPointInfoList.Count; i++)
            {
                GameObject spawnPoint = Instantiate(spawnPointPrefab);
                Destroy(spawnPoint.transform.GetChild(0).gameObject);
                Destroy(spawnPoint.GetComponent<MeshRenderer>());
                Destroy(spawnPoint.GetComponent<MeshFilter>());
                spawnPoint.transform.parent = spawnPointParent;
                spawnPoint.transform.localPosition = info.spawnPointInfoList[i].position;
                spawnPointPosList.Add(spawnPoint.transform.localPosition);

            }
        }
        int random = Random.Range(0, spawnPointPosList.Count);
        #endregion
        #region 화상채팅구역 가져오기
        GameObject facechatParent = GameObject.Find("FaceChatParent");
        for (int i = 0; i < info.faceChatList.Count; i++)
        {
            GameObject face = GameObject.Instantiate(facechatPrefab);
            Destroy(face.GetComponent<SpriteRenderer>());
            face.transform.position = info.faceChatList[i].postion;
            //face.GetComponent<NewBehaviourScript>()._channelName = info.faceChatList[i].channelName;
            face.transform.parent = facechatParent.transform.GetChild(int.Parse(info.faceChatList[i].channelName) - 1);
        }
        #endregion
        #region 오브젝트 가져오기
        GameObject objectParent = GameObject.Find("ObjectParent");
        for (int i = 0; i < info.objectList.Count; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.transform.position = info.objectList[i].Position;
            Debug.Log("야!!!");
            obj.transform.GetChild(0).gameObject.GetComponent<ObjectInfo_H>().objectInfo = info.objectList[i];
            obj.transform.parent = objectParent.transform;
            if (info.objectList[i].upperObj)
            {
                obj.transform.localPosition = new Vector3(obj.transform.localPosition.x,
    obj.transform.localPosition.y,
    zValueUpper);
            }
            else
            {
                obj.transform.localPosition = new Vector3(obj.transform.localPosition.x,
    obj.transform.localPosition.y,
    zValueLower);
            }
        }
        #endregion

        #region 3D 오브젝트 가져오기
        GameObject _3DobjectParent = GameObject.Find("_3DObjectParent");
        for (int i = 0; i < info._3DObjectList.Count; i++)
        {
            string path = Application.persistentDataPath + "/" + info._3DObjectList[i].myName + ".obj";
            StreamWriter sw = File.CreateText(path);
            sw.WriteLine(System.Text.Encoding.Default.GetString(info._3DObjectList[i]._3DObjectByteArray));
            sw.Close();
            GameObject obj = new OBJLoader().Load(path);
            obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            obj.transform.position = info._3DObjectList[i].pos;
            Texture2D objTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            objTexture.LoadImage(info._3DObjectList[i].texture);
            MeshRenderer mr = obj.GetComponentInChildren<MeshRenderer>();
            if (mr)
            {
                mr.material.SetTexture("_MainTex", objTexture);
            }
            //obj.GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", Base64ToTexture2D(info._3DObjectList[i].texture));
            Debug.Log("야!!!");
            _3DObjectInfo_L _3DObjectInfo = obj.AddComponent<_3DObjectInfo_L>();
            _3DObjectInfo.pos = obj.transform.localPosition;
            if (mr)
            {
                _3DObjectInfo.texture = ((Texture2D)mr.material.mainTexture).EncodeToPNG();
            }
            //        _3DObjectInfo.texture= ((Texture2D)obj.GetComponentInChildren<MeshRenderer>().material.mainTexture).EncodeToPNG();
            _3DObjectInfo.objPath = (info._3DObjectList[i].objPath);
            obj.transform.parent = _3DobjectParent.transform;
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x,
    obj.transform.localPosition.y,
    zValue3DObj);
            if (obj.transform.childCount > 0)
            {
                obj.transform.GetChild(0).tag = "_3DObject";
                obj.transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
            }
        }

        #endregion
        if (spawnPointPosList.Count < 1)
        {
            player = PhotonNetwork.Instantiate("$Main_Character_1_0_2D", playerSpawnPos.position, Quaternion.identity);
            upperUI.myCharacter = player;
            print("RoomManager 1");
        }
        else
        {
            Debug.Log(random);
            Debug.Log(spawnPointPosList.Count);
            player = PhotonNetwork.Instantiate("$Main_Character_1_0_2D", spawnPointPosList[random], Quaternion.identity);
            upperUI.myCharacter = player;

            print("RoomManager 2");

        }
        print("Room Manager room name : " + PhotonNetwork.CurrentRoom.Name);
        GameManager.instance.myCharacter = player;
        PhotonNetwork.IsMessageQueueRunning = true;
        characterMove = player.GetComponent<CharacterMove_H>();
        characterMove.enabled = true;
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, zValuePlayer);


        if (spawnPointPosList.Count > 0)
        {
            int randPos = Random.Range(0, spawnPointPosList.Count);
            player.transform.position = spawnPointPosList[randPos];
        }
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager_L = GameObject.Find("GameManager_L").GetComponent<GameManager_L>();
        gameManager_L.myToken = gameManager.userinfo.token;
        gameManager_L.characterMove = characterMove;

        isTokenSet = false;

        panelLoading.SetActive(false);
        //스폰 지점에 캐릭터 위치
    }
        [Header("3D 오브젝트 회전")]
    GameObject _3DObjectToDrag;
    public float objectRotSpeed = 10f;
    public float lerpSpeed = 1.0f;
    public float lerpDecreaseSpeed = 15f;
    float rotMagnitude = 0f;
    Vector3 speed = new Vector3();
    Vector3 avgSpeed = new Vector3();
    bool isDragging = false;

    // Update is called once per frame
    void Update()

    {
        if (characterMove)
        {
            if (isTokenSet == false)
            {
                if (characterMove.idx > -1)
                {
                    if (gameManager_L.isIdxSet)
                    {
                        print(gameManager.userinfo.token);
                        photonView.RPC("RPCSetToken", RpcTarget.AllBuffered, gameManager.userinfo.token, characterMove.idx);
                        isTokenSet = true;
                    }
                }


            }
        }
        //_3DObject = tag
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log(hitInfo.transform.tag);

                if (hitInfo.transform.tag == "_3DObject")
                {
                    _3DObjectToDrag = hitInfo.transform.gameObject;
                }
                else
                {
                    _3DObjectToDrag = null;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_3DObjectToDrag)
            {
                _3DObjectToDrag = null;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (_3DObjectToDrag)
            {
                _3DObjectToDrag.transform.Rotate(0f, -Input.GetAxis("Mouse X") * objectRotSpeed, 0f, Space.World);
                _3DObjectToDrag.transform.Rotate(-Input.GetAxis("Mouse Y") * objectRotSpeed, 0f, 0f);
            }
        }


        


        #region lerp로 부드럽게 회전하려 했는데 보류
        /*        if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hitInfo;

                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        Debug.Log(hitInfo.transform.tag);

                        if (hitInfo.transform.tag == "_3DObject")
                        {
                            _3DObjectToDrag = hitInfo.transform.gameObject;
                            isDragging = true;
                        }
                        else
                        {
                            rotMagnitude = 0f;
                            _3DObjectToDrag = null;
                        }
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (_3DObjectToDrag)
                    {
                        //_3DObjectToDrag = null;
                        //speed = avgSpeed;
                        //float i = Time.deltaTime * lerpSpeed;
                        //speed = Vector3.Lerp(speed, Vector3.zero, i);
                        print("3d object mouse up");
                        isDragging = false;
                    }
                }

                if (Input.GetMouseButton(0))
                {
                    if (_3DObjectToDrag)
                    {
                        speed = new Vector3(-Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
                        if (speed.magnitude > 0f)
                            rotMagnitude = speed.magnitude;
                        //avgSpeed = Vector3.Lerp(avgSpeed, speed, Time.deltaTime * 2);
                        //_3DObjectToDrag.transform.Rotate(0f, -Input.GetAxis("Mouse X") * objectRotSpeed, 0f, Space.World);
                        //_3DObjectToDrag.transform.Rotate(-Input.GetAxis("Mouse Y") * objectRotSpeed, 0f, 0f);
                    }
                }

                if (_3DObjectToDrag)
                {
                    if (isDragging == false)
                    {
                        rotMagnitude = Mathf.Lerp(rotMagnitude*2f, 0f, lerpDecreaseSpeed * Time.deltaTime);
                        print("rot Magnitude : " + rotMagnitude);
                    }
                    _3DObjectToDrag.transform.Rotate(Camera.main.transform.up * speed.x * rotMagnitude, Space.World);
                    _3DObjectToDrag.transform.Rotate(Camera.main.transform.right * speed.y * rotMagnitude, Space.World);
                }*/
        #endregion
    }



    #region byte[] to Texture

    private Texture2D Base64ToTexture2D(byte[] imageData)
    {

        int width, height;
        GetImageSize(imageData, out width, out height);

        // 매프레임 new를 해줄경우 메모리 문제 발생 -> 멤버 변수로 변경
        Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false, true);

        texture.hideFlags = HideFlags.HideAndDontSave;
        texture.filterMode = FilterMode.Point;
        texture.LoadImage(imageData);
        texture.Apply();

        return texture;
    }
    private void GetImageSize(byte[] imageData, out int width, out int height)
    {
        width = ReadInt(imageData, 3 + 15);
        height = ReadInt(imageData, 3 + 15 + 2 + 2);
    }

    private int ReadInt(byte[] imageData, int offset)
    {
        return (imageData[offset] << 8 | imageData[offset + 1]);
    }

    #endregion

    [PunRPC]
    void RPCSetToken(string token, int idx)
    {
        StartCoroutine(IESetToken(token, idx));
        /*        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                print("player 없음");
                print("제 idx는요.." + idx);
                for(int i = 0; i < players.Length; i++)
                {
                    print("if문 안 들어감");
                    print(i + "번째 player의 idx는 " + players[i].GetComponent<CharacterMove_H>().idx);
                    if (players[i].GetComponent<CharacterMove_H>().idx == idx)
                    {
                        print("찾으려는 idx " + idx);
                        print("idx 찾음" + players[i].GetComponent<CharacterMove_H>().idx);
                        players[i].GetComponent<CharacterMove_H>().myToken = token;
                        gameManager_L.isIdxSet = false;
                    }
                }*/
    }

    IEnumerator IESetToken(string token, int idx)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        //idx set 될때까지 잠시 대기
        yield return new WaitForSeconds(2f);
        print("player 없음");
        print("제 idx는요.." + idx);
        for (int i = 0; i < players.Length; i++)
        {
            print("if문 안 들어감");
            print(i + "번째 player의 idx는 " + players[i].GetComponent<CharacterMove_H>().idx);
            if (players[i].GetComponent<CharacterMove_H>().idx == idx)
            {
                print("찾으려는 idx " + idx);
                print("idx 찾음" + players[i].GetComponent<CharacterMove_H>().idx);
                players[i].GetComponent<CharacterMove_H>().myToken = token;
                gameManager_L.isIdxSet = false;
            }
        }
        yield break;
    }

    public void GoInfoScene()
    {
        goInfo = true;
        PhotonNetwork.LeaveRoom();
        //SceneManager.LoadScene("MyInfoScene_L");
    }

    public void GoLobbyScene()
    {
        goLobby = true;
        PhotonNetwork.LeaveRoom();
        //SceneManager.LoadScene("LobbyScene_L");
    }

    public void GoMapEditorScene()
    {
        goMapEditor = true;
        panelLoading.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        if (PhotonNetwork.IsConnectedAndReady)
            PhotonNetwork.JoinLobby();
        print("방 나왔음");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        /*        if (majorRoomManager.player)
                    PhotonNetwork.Destroy(majorRoomManager.player);*/

        print("Lobby Manager Lobby Name : " + PhotonNetwork.CurrentLobby.Name);

        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (goInfo)
            {
                SceneManager.LoadScene("MyInfoScene_L");
            }
            else if (goLobby)
            {
                SceneManager.LoadScene("LobbyScene_L");

            }
            else if (goMapEditor)
            {
                SceneManager.LoadScene("MapEditorScene_H");
            }
        }


        print("OnJoinedLobby 호출");

        //print("로비 안에 있나요?? : " + PhotonNetwork.InLobby);

    }
}