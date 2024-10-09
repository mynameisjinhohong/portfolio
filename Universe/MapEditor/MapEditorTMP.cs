using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using SFB;
using Dummiesman;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class MapEditorTmp : MonoBehaviour
{
    public static MapEditorTmp instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public enum ToolType
    {
        None,
        Eraser,
        Stamp,
        Dropper,
        Arrow
    }

    public enum PlacementType
    {
        Floor,
        UpperObject,
        Object,
        TileEffect,
        _3DObject
    }

    public enum TileEffectType
    {
        Portal,
        DefinedArea,
        Wall,
        SpawnPoint,
        FaceChat,
    }
    public enum ObjectType
    {
        Text,
        Image,
    }

    public ToolType toolType;
    public PlacementType placementType;
    public TileEffectType tileEffectType;
    public ObjectType objectType;

    public float floorTileZ;
    public float definedAreaZ;
    public float wallTileZ;

    public GameObject floorTilePrefab;
    public GameObject wallTilePrefab;
    public GameObject definedAreaPrefab;
    public GameObject portalPrefab;
    public GameObject spawnPrefab;
    public GameObject objectPrefab;
    public GameObject objectButtonPrefab;
    public GameObject faceChatPrefab;

    public Texture currClickedTileTexture;
    public float minOrthographicSize = 2f;
    public float maxOrthographicSize = 5f;
    public float scrollSpeed = 0.5f;
    public float arrowDragSpeed = 10f;
    public int width, height;
    public Transform grid;
    public Transform cursorSquare;
    public Transform tileParent;
    public Transform objectParent;
    public Transform _3DobjectParent;
    public Transform definedAreaParent;
    public Transform portalParent;
    public Transform objectButtonParent;
    public Transform faceChatParent;

    public Dropdown faceChatAreaName;

    public Dropdown definedAreaDropdown;
    public InputField inputFieldDefinedAreaName;

    public InputField inputFieldPortalOtherMapName;
    public InputField inputFieldPortalDefinedAreaName;

    public GameObject canvas;
    public MeshRenderer bg;
    public MeshRenderer fg;

    public GameObject panelPortalToDefinedArea;
    public GameObject panelPortalToOtherMap;
    public GameObject checkImageDefinedArea;
    public GameObject checkImageOtherMap;
    public GameObject checkImageKey;
    public GameObject checkImageInstant;
    public GameObject checkImageKey_DefinedArea;
    public GameObject checkImageInstant_DefinedArea;
    public GameObject scrollViewMyObject;
    public GameObject scrollViewMyUpperObject;


    public List<Vector3> spawnPointPosList; //로드 시, 리스트에 위치 넣어두고 랜덤 위치에 캐릭터 옮김
    //UI 클릭 시에는 맵 생성되지 않게
    int layerMaskUI;
    PortalInfo portalInfo;
    Vector3 gridStartPos;

    int tileLayerMask;
    Vector2 pastTilePos;
    Vector2 pastDefinedAreaPos;
    Vector2 pastFaceChatPos;
    Vector2 pastWallPos;
    Vector2 pastPortalPos;
    Vector2 pastSpawnPointPos;
    Vector2 mouseClickPos;
    public int mapCount;
    public GameObject quadBG;
    public GameObject quadFG;
    /*    public LineRenderer gridLineRenderer_X;
        public LineRenderer gridLineRenderer_Y;*/
    public FileManager_L fileManger;

    #region Variables for importing 3D Object
    string objPath = string.Empty;
    string error = string.Empty;
    string path = "";
    string texturePath = "";
    string savepth = "";
    char nl = '\n';
    public Text textPath;
    public GameObject loadedObject;
    #endregion

    public float zValueUpper = 100f;
    public float zValueLower = 0f;
    public float zValue3DObj = 50f;
    public float zValuePlayer = 1f;


    public List<Toggle> toggleListObjctUpperLower;

    public string currStamp3DObjName;
    public Texture2D currStamp3DTexture;

    public void SmallToggle()
    {
        quadBG.transform.localScale = new Vector3(60, 30, 1);
        quadFG.transform.localScale = new Vector3(60, 30, 1);
        float x = Mathf.Clamp(Camera.main.transform.position.x, -3f, 11f);
        float y = Mathf.Clamp(Camera.main.transform.position.y, 0f, 3f);
        Camera.main.transform.position = new Vector3(x, y, -100f);
        fileManger.mapSize = 0;
    }
    public void MideumToggle()
    {
        quadBG.transform.localScale = new Vector3(120, 60, 1);
        quadFG.transform.localScale = new Vector3(120, 60, 1);
        float x = Mathf.Clamp(Camera.main.transform.position.x, -33f, 41f);
        float y = Mathf.Clamp(Camera.main.transform.position.y, -15f, 18f);
        Camera.main.transform.position = new Vector3(x, y, -100f);
        fileManger.mapSize = 1;
    }
    public void BigToggle()
    {
        quadBG.transform.localScale = new Vector3(180, 90, 1);
        quadFG.transform.localScale = new Vector3(180, 90, 1);
        fileManger.mapSize = 2;
        float x = Mathf.Clamp(Camera.main.transform.position.x, -63f, 71f);
        float y = Mathf.Clamp(Camera.main.transform.position.y, -30f, 33f);
        Camera.main.transform.position = new Vector3(x, y, -100f);
    }
    // Start is called before the first frame update
    void Start()
    {
        //SetMap(SpaceInfo.spaceName);
        toolType = ToolType.Stamp;
        placementType = PlacementType.Floor;
        pastTilePos = Vector2.zero;
        pastDefinedAreaPos = Vector2.zero;
        pastPortalPos = Vector2.zero;
        pastSpawnPointPos = Vector2.zero;
        tileLayerMask = 1 << LayerMask.NameToLayer("Tile");
        gridStartPos = grid.transform.position;
        gridStartPos.x -= grid.transform.localScale.x * 0.5f;
        gridStartPos.y -= grid.transform.localScale.y * 0.5f;

        layerMaskUI = (1 << LayerMask.NameToLayer("UI"));

        /*for (int y = 0; y < height; y++)
        {
            //gridLineRenderer_X.positionCount += 2;
            gridLineRenderer_X.SetPosition(y * 2, new Vector3(gridStartPos.x, gridStartPos.y + y, gridStartPos.z));
            gridLineRenderer_X.SetPosition(y * 2 + 1, new Vector3(gridStartPos.x+width, gridStartPos.y + y, gridStartPos.z));
        }*/

        definedAreaDropdown.onValueChanged.AddListener(delegate
        {
            print(definedAreaDropdown.captionText.text);
        });

        for (int i = 0; i < toggleListObjctUpperLower.Count; i++)
        {
            toggleListObjctUpperLower[i].onValueChanged.AddListener(delegate { OnToggleSwitch(); });
        }
    }

    private void OnToggleSwitch()
    {
        if (toggleListObjctUpperLower[0].isOn)
        {
            placementType = PlacementType.UpperObject;
        }
        else
        {
            placementType = PlacementType.Object;
        }
        /*        for (int i = 0; i < toggleListObjctUpperLower.Count; i++)
                {
                    if (toggleListObjctUpperLower[i].isOn)
                    {
                        placementType = (PlacementType)i + 1;
                        //int를 enum형으로 변환
                        //toggleList 순서와 placementType upper lower 순서가 일치해야 함
                    }
                }*/
    }

    public void ReturnZero()
    {
        bg.material.SetTexture("_MainTex", null);
        GameObject parent;
        parent = GameObject.Find("TileParent");
        //Destroy(parent);
        int childCount = 0;
        childCount = parent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Debug.Log(parent.transform.childCount);
            Destroy(parent.transform.GetChild(i).gameObject);
            //Destroy(parent.transform.GetChild(0));
        }
        parent = GameObject.Find("PortalParent");
        childCount = parent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(parent.transform.GetChild(i).gameObject);
        }
        parent = GameObject.Find("DefinedAreaParent");
        childCount = parent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(parent.transform.GetChild(i).gameObject);
        }
        parent = GameObject.Find("WallParent");
        childCount = parent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(parent.transform.GetChild(i).gameObject);
        }
        parent = GameObject.Find("SpawnPointParent");
        childCount = parent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(parent.transform.GetChild(i).gameObject);
        }

        portalInfo = new PortalInfo();
        portalInfo.placeType = PortalInfo.PlaceType.DefinedArea;
        portalInfo.moveType = PortalInfo.MoveType.Key;
    }


    public void SetMap(string mapName)
    {
        #region 배경화면 가져오기
        string bgpath = Application.dataPath + "/Resources/Resources_H/MapData/" + mapName + ".txt";
        string jsonData = File.ReadAllText(bgpath);
        SpaceInfo spaceInfo = JsonUtility.FromJson<SpaceInfo>(jsonData);
        for (int i = 0; i < spaceInfo.mapList.Count; i++)
        {
            fileManger.mapinfos.Add(spaceInfo.mapList[i]);
            Dropdown.OptionData option = new Dropdown.OptionData();
            Debug.Log(fileManger.mapinfos[i].mapName);
            if (fileManger.mapinfos[i].mapName == null)
            {
                option.text = "Map" + i.ToString();
                fileManger.mapinfos[i].mapName = "Map" + i.ToString();
            }
            else
            {
                option.text = fileManger.mapinfos[i].mapName;
                //fileManger.mapinfos[i].mapName = "Map" + i.ToString();
            }
        }
        MapInfo info = spaceInfo.mapList[0];
        //MapInfo info = JsonUtility.FromJson<MapInfo>(jsonData);
        //quadBG.transform.localScale = new Vector3(info.mapWidth / 20, info.mapHeight / 20, 1);
        Texture2D bgTexture = new Texture2D(0, 0);
        bgTexture.LoadImage(info.backGroundImage);
        bg.material.SetTexture("_MainTex", bgTexture);
        Texture2D fgTexture = new Texture2D(0, 0);
        fgTexture.LoadImage(info.foreGroundImage);
        fg.material.shader = Shader.Find("Unlit/Transparent");
        fg.material.SetTexture("_MainTex", fgTexture);
        #endregion
        #region 타일 가져오기
        string tileParent = "TileParent";
        for (int i = 0; i < info.tileList.Count; i++)
        {
            Vector3 tilePos = info.tileList[i].position;
            Texture tileSprite = Resources.Load<Texture>("Resources_L/" + info.tileList[i].imageName);
            GameObject realTile = Instantiate(floorTilePrefab);
            realTile.transform.position = tilePos;
            realTile.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", tileSprite);
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
            area.transform.position = info.definedAreaList[i].positon;
            area.GetComponentInChildren<Text>().text = info.definedAreaList[i].areaName;
            area.transform.parent = areaParent.transform.GetChild(nameDic[info.definedAreaList[i].areaName]);
        }

        #endregion
        #region 벽 가져오기
        for (int i = 0; i < info.wallList.Count; i++)
        {
            Vector3 wallPos = info.wallList[i].positon;
            GameObject wall = Instantiate(wallTilePrefab);
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
                GameObject spawnPoint = Instantiate(spawnPrefab);
                spawnPoint.transform.parent = spawnPointParent;
                spawnPoint.transform.localPosition = info.spawnPointInfoList[i].position;
                spawnPointPosList.Add(spawnPoint.transform.localPosition);

            }
        }
        #endregion
        #region 화상채팅구역 가져오기
        GameObject facechatParent = GameObject.Find("FaceChatParent");
        for (int i = 0; i < info.faceChatList.Count; i++)
        {
            GameObject face = GameObject.Instantiate(faceChatPrefab);
            face.transform.position = info.faceChatList[i].postion;
            face.GetComponent<NewBehaviourScript>()._channelName = info.faceChatList[i].channelName;
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

            obj.transform.position = info._3DObjectList[i].pos;
            obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            Texture2D objTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            objTexture.LoadImage(info._3DObjectList[i].texture);
            MeshRenderer mr = obj.GetComponentInChildren<MeshRenderer>();
            if (mr)
            {
                mr.material.SetTexture("_MainTex", objTexture);
            }
            //obj.GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", Base64ToTexture2D(info._3DObjectList[i].texture));
            _3DObjectInfo_L _3DObjectInfo = obj.AddComponent<_3DObjectInfo_L>();
            _3DObjectInfo.pos = obj.transform.localPosition;
            if (mr)
            {
                _3DObjectInfo.texture = ((Texture2D)mr.material.mainTexture).EncodeToPNG();
            }
            _3DObjectInfo.objPath = (info._3DObjectList[i].objPath);
            obj.transform.parent = _3DobjectParent.transform;
            if (obj.transform.childCount > 0)
            {
                obj.transform.GetChild(0).tag = "_3DObject";
                obj.transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
            }
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x,
    obj.transform.localPosition.y,
    zValue3DObj);
        }

        #endregion
        //================
        #region 포탈값 초기화
        portalInfo = new PortalInfo();
        portalInfo.placeType = PortalInfo.PlaceType.DefinedArea;
        portalInfo.moveType = PortalInfo.MoveType.Key;
        #endregion
    }

    public void ChangeMap(MapInfo changeInfo)
    {
        MapInfo info = changeInfo;
        //MapInfo info = JsonUtility.FromJson<MapInfo>(jsonData);
        //quadBG.transform.localScale = new Vector3(info.mapWidth / 20, info.mapHeight / 20, 1);
        Texture2D bgTexture = new Texture2D(0, 0);
        bgTexture.LoadImage(info.backGroundImage);
        bg.material.SetTexture("_MainTex", bgTexture);
        #region 타일 가져오기
        string tileParent = "TileParent";
        if (info.tileList == null && info.backGroundImage == null)
        {
            return;
        }
        for (int i = 0; i < info.tileList.Count; i++)
        {
            Vector3 tilePos = info.tileList[i].position;
            Texture tileSprite = Resources.Load<Texture>("Resources_L/" + info.tileList[i].imageName);
            GameObject realTile = Instantiate(floorTilePrefab);
            realTile.transform.position = tilePos;
            realTile.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", tileSprite);
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
            area.transform.position = info.definedAreaList[i].positon;
            area.GetComponentInChildren<Text>().text = info.definedAreaList[i].areaName;
            area.transform.parent = areaParent.transform.GetChild(nameDic[info.definedAreaList[i].areaName]);
        }

        #endregion
        #region 벽 가져오기
        for (int i = 0; i < info.wallList.Count; i++)
        {
            Vector3 wallPos = info.wallList[i].positon;
            GameObject wall = Instantiate(wallTilePrefab);
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
                GameObject spawnPoint = Instantiate(spawnPrefab);
                spawnPoint.transform.parent = spawnPointParent;
                spawnPoint.transform.localPosition = info.spawnPointInfoList[i].position;
                spawnPointPosList.Add(spawnPoint.transform.localPosition);

            }
        }
        #endregion


        //================
        #region 포탈값 초기화
        portalInfo = new PortalInfo();
        portalInfo.placeType = PortalInfo.PlaceType.DefinedArea;
        portalInfo.moveType = PortalInfo.MoveType.Key;
        #endregion
    }

    void UiOnOff()
    {
        if (objSkill.value == 0)
        {

            for (int i = 0; i < skillUI.Length; i++)
            {
                skillUI[i].SetActive(false);
            }
        }
        else if (objSkill.value == 1)
        {

            for (int i = 0; i < skillUI.Length; i++)
            {
                skillUI[i].SetActive(false);
            }
            skillUI[0].SetActive(true);
        }
        else if (objSkill.value == 2)
        {

            for (int i = 0; i < skillUI.Length; i++)
            {
                skillUI[i].SetActive(false);
            }
            skillUI[0].SetActive(true);
        }
        else if (objSkill.value == 2)
        {

            for (int i = 0; i < skillUI.Length; i++)
            {
                skillUI[i].SetActive(false);
            }
            skillUI[1].SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        /*        for(int x = 0; x < width; x++)
                {
                    for(int y = 0; y < height; y++)
                    {
                        //lineRenderer.SetPosition()

                        Debug.DrawLine(new Vector3(gridStartPos.x + x, gridStartPos.y + y, gridStartPos.z),
                            new Vector3(gridStartPos.x + x + 1, gridStartPos.y + y, gridStartPos.z), Color.red);
                        Debug.DrawLine(new Vector3(gridStartPos.x + x, gridStartPos.y + y, gridStartPos.z),
            new Vector3(gridStartPos.x + x, gridStartPos.y + y + 1, gridStartPos.z), Color.red);
                    }
                }
        */
        objSkill.onValueChanged.AddListener(delegate { UiOnOff(); });


        MoveCursorSquare();

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= scrollWheel * Time.deltaTime * scrollSpeed;

        if (Camera.main.orthographicSize > maxOrthographicSize)
            Camera.main.orthographicSize = maxOrthographicSize;
        if (Camera.main.orthographicSize < minOrthographicSize)
            Camera.main.orthographicSize = minOrthographicSize;

        switch (toolType)
        {
            case ToolType.Stamp:
                if (placementType == PlacementType.Floor)
                {
                    Stamp();
                }
                else if (placementType == PlacementType.TileEffect)
                {
                    TileEffect();
                }
                else if (placementType == PlacementType.Object)
                {
                    if (objectType == ObjectType.Text)
                    {
                        StampObjectText();
                    }
                    else if (objectType == ObjectType.Image)
                    {
                        StampObjectImage(false);
                    }
                }
                else if (placementType == PlacementType.UpperObject)
                {
                    if (objectType == ObjectType.Image)
                    {
                        StampObjectImage(true);
                    }
                }
                else if (placementType == PlacementType._3DObject)
                {
                    Stamp3DObject();
                }
                break;
            case ToolType.Eraser:
                Erase();
                break;
            case ToolType.Dropper:
                Dropper();
                break;
            case ToolType.Arrow:
                Arrow();
                break;
        }
    }
    public void SelectText()
    {
        objectType = ObjectType.Text;
    }
    #region 오브젝트 이미지 가져오기
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

    GameObject objButton;
    public void SelectImage()
    {
        objectType = ObjectType.Image;
        path = WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "png", false));
        //path = EditorUtility.OpenFilePanel("Show all images(.png)", "", "png");
        if (path.Length > 0)
        {
            objButton = Instantiate(objectButtonPrefab);
            objButton.transform.parent = objectButtonParent;
            objButton.GetComponent<ObjButton_H>().type = ObjButton_H.Type._2D;
            StartCoroutine(GetTexture());
        }
    }
    IEnumerator GetTexture()
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
            Texture2D texture2D = (Texture2D)myTexture;
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            objButton.GetComponent<Image>().sprite = sprite;

        }
    }
    #endregion
    public Texture2D objImage;
    public InputField objName;
    public Dropdown objSkill;
    public GameObject[] skillUI;
    public Toggle objInteraction;
    public InputField objURL;
    public Texture2D objChangeImage;

    #region Functions for importing 3D Object

    public void OnClickGetOBJPath()
    {
        //path = EditorUtility.OpenFilePanel("Show all images(.png)", "", "png");
        objPath = WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "obj", false));

        string[] spstring = objPath.Split(nl);
        string realPath = spstring[0];
        if (!File.Exists(realPath))
        {
            Debug.LogError("File doesn't exist.");
        }
        else
        {
            print("OBJ Loaded");
            loadedObject = new OBJLoader().Load(realPath);
            loadedObject.transform.position = Vector3.zero;
            //loadedObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            loadedObject.SetActive(false);
            //GameObject newObj = Instantiate(loadedObject,objectParent); 잘 찍히는지 테스트
            //loadedObject = new OBJLoader().Load(realPath);
            error = string.Empty;
        }
    }


    public void OnClickSetTexture()
    {
        //texturePath = EditorUtility.OpenFilePanel("Show all images(.png)", "", "png");
        texturePath = WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
        StartCoroutine(IESetTexture());
    }

    IEnumerator IESetTexture()
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + texturePath);

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
            /*            if (Directory.Exists(savepth) == false)
                        {
                            Directory.CreateDirectory(savepth);
                        }*/
            //File.WriteAllBytes(savepth + "/mapData.png", textuerData);
            objTexture = convertedTexture;
            /*            if (loadedObject)
                            loadedObject.GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", convertedTexture);
                        else
                            print("loaded object is null");*/
        }
    }
    Texture2D objTexture;

    string ReadTxt(string filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        string value = "";

        if (fileInfo.Exists)
        {
            StreamReader reader = new StreamReader(filePath);
            value = reader.ReadToEnd();
            reader.Close();
        }

        else
            value = "파일이 없습니다.";

        return value;
    }

    #endregion

    [Header("Test for 3d thumbnail")]
    public RawImage thumbnailParent;
    public RawImage thumbnailChild;

    public void OnClickCreate3DObj()
    {
        GameObject objButton = Instantiate(objectButtonPrefab, objectButtonParent);

        Texture2D texture = RuntimePreviewGenerator.GenerateModelPreview(loadedObject.transform);
        Rect rect = new Rect(0, 0, texture.width, texture.height);

        objButton.GetComponent<Image>().sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

        //button에 3d object의 이름 저장
        objButton.GetComponent<ObjButton_H>()._3DObjectName = loadedObject.name;
        objButton.GetComponent<ObjButton_H>().type = ObjButton_H.Type._3D;
        objButton.GetComponent<ObjButton_H>()._3DObjectTexture = objTexture;
        //3d object의 이름으로 Resources/.../어떤 폴더에 object를 byte[]화 하여 저장
        string _3DObjectPath = Application.persistentDataPath + "/" + loadedObject.name + ".obj";
        //byte[] _3DObjectByteArray = SerializeToByteArray(loadedObject.transform.GetChild(0));

        print("3D Object path : " + _3DObjectPath);
        StreamWriter sw = File.CreateText(_3DObjectPath);

        string[] spstring = objPath.Split(nl);
        string realPath = spstring[0];

        FileStream test = new FileStream(realPath, FileMode.Open);

        StreamReader testSR = new StreamReader(test);
        string objText = (testSR.ReadToEnd());

        sw.WriteLine(objText);
        testSR.Close();
        sw.Close();
        //Stamp로 3d object 찍으면, _3DObjectParent의 자식으로 넣어두었다가
        //저장할 때 object를 txt형식으로 변환하여 저장해두기
        //그럼 List<byte[]> _3DObjectList 로 해두어야 겠네
    }//obj를 byte로 해서 불러오기



    void Stamp3DObject()
    {
        if (loadedObject != null)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {

                    if (hitInfo.transform.tag == "Object")
                    {
                        return;
                    }
                    if (hitInfo.transform.tag == "BackGround")
                    {
                        int x = (int)hitInfo.point.x;
                        int y = (int)hitInfo.point.y;
                        if (pastTilePos == new Vector2(x, y))
                        {
                            print("already exists");
                            return;
                        }
                        pastTilePos = new Vector2(x, y);
                        GameObject obj = Instantiate(loadedObject);
                        obj.SetActive(true);
                        obj.transform.position = new Vector3(x, y, 0);
                        obj.GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", currStamp3DTexture);
                        if (obj.transform.childCount > 0)
                        {
                            obj.transform.GetChild(0).tag = "_3DObject";
                            obj.transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
                        }
                        //obj.transform.localScale = new Vector3(objImage.width /1920, objImage.height/1080, 0);
                        obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                        obj.transform.parent = _3DobjectParent;
                        obj.transform.localPosition = new Vector3(obj.transform.localPosition.x,
    obj.transform.localPosition.y,
    zValue3DObj);
                        /*                     obj.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", (Texture)objImage);*/

                        FileStream test = new FileStream(Application.persistentDataPath + "/" + currStamp3DObjName + ".obj", FileMode.Open);

                        StreamReader testSR = new StreamReader(test);
                        string objText = (testSR.ReadToEnd());

                        _3DObjectInfo_L _3DObjectInfo = obj.AddComponent<_3DObjectInfo_L>();
                        _3DObjectInfo._3DObjectByteArray = System.Text.Encoding.UTF8.GetBytes(objText);
                        _3DObjectInfo.objPath = System.Text.Encoding.UTF8.GetBytes(Application.persistentDataPath + "/" + currStamp3DObjName + ".obj");
                        _3DObjectInfo.pos = obj.transform.position;
                        _3DObjectInfo.texture = ((Texture2D)obj.GetComponentInChildren<MeshRenderer>().material.mainTexture).EncodeToPNG();
                        _3DObjectInfo.myName = obj.name;
                        testSR.Close();
                    }
                }
            }
        }
    }

    void StampObjectImage(bool isUpper)
    {
        if (objImage != null)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {

                    if (hitInfo.transform.tag == "Object")
                    {
                        return;
                    }
                    if (hitInfo.transform.tag == "BackGround")
                    {
                        int x = (int)hitInfo.point.x;
                        int y = (int)hitInfo.point.y;
                        if (pastTilePos == new Vector2(x, y))
                        {
                            print("already exists");
                            return;
                        }
                        pastTilePos = new Vector2(x, y);
                        GameObject obj = Instantiate(objectPrefab);
                        obj.transform.position = new Vector3(x, y, 0);
                        //obj.transform.localScale = new Vector3(objImage.width /1920, objImage.height/1080, 0);
                        obj.transform.parent = objectParent;
                        Rect rect = new Rect(0, 0, objImage.width, objImage.height);
                        
                        obj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Sprite.Create(objImage, rect, new Vector2(0.5f, 0.5f));
                        //obj.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", (Texture)objImage);



                        ObjectInfo objinfo = obj.transform.GetChild(0).GetComponent<ObjectInfo_H>().objectInfo;
                        objinfo.Position = obj.transform.position;
                        objinfo.image = objImage.EncodeToPNG();

                        objinfo.objType = ObjectInfo.ObjectType.Image;
                        objinfo.objName = objName.text;

                        if (isUpper == true)
                        {
                            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x,
    obj.transform.localPosition.y,
    zValueUpper);
                            objinfo.upperObj = true;
                        }
                        else
                        {
                            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x,
    obj.transform.localPosition.y,
    zValueLower);
                            objinfo.upperObj = false;
                        }

                        if (objInteraction.isOn)
                        {
                            objinfo.interactionType = ObjectInfo.InteractionType.pressF;
                        }
                        else
                        {
                            objinfo.interactionType = ObjectInfo.InteractionType.touch;
                        }
                        if (objSkill.value == 0)
                        {
                            objinfo.objSkill = ObjectInfo.ObjectSkill.nomalObj;
                        }
                        else if (objSkill.value == 1)
                        {
                            objinfo.objSkill = ObjectInfo.ObjectSkill.urlObj;
                            objinfo.urlSkill = objURL.text;
                        }
                        else if (objSkill.value == 2)
                        {
                            objinfo.objSkill = ObjectInfo.ObjectSkill.talkingObj;
                            objinfo.talkingSkill = objURL.text;
                        }
                        else if (objSkill.value == 3)
                        {
                            objinfo.objSkill = ObjectInfo.ObjectSkill.changeObj;
                        }
                    }
                }
            }
        }
    }

    public void OnClickCreateTextObj()
    {
        objectType = ObjectType.Text;
    }

    public void ButtonTextObj()
    {
        inputFieldTextObj.gameObject.SetActive(true);
        scrollViewMyObject.SetActive(false);
    }
    public InputField inputFieldTextObj;
    public GameObject textObj;
    void StampObjectText()
    {
        if (inputFieldTextObj.text.Length > 0)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {

                    if (hitInfo.transform.tag == "Object")
                    {
                        return;
                    }
                    if (hitInfo.transform.tag == "BackGround")
                    {
                        int x = (int)hitInfo.point.x;
                        int y = (int)hitInfo.point.y;
                        if (pastTilePos == new Vector2(x, y))
                        {
                            print("already exists");
                            return;
                        }
                        pastTilePos = new Vector2(x, y);
                        GameObject obj = Instantiate(textObj);
                        obj.transform.position = new Vector3(x, y, 0);
                        obj.GetComponent<TextMesh>().text = inputFieldTextObj.text;
                    }
                }
            }
        }
    }
    void TileEffect()
    {
        switch (tileEffectType)
        {
            case TileEffectType.Portal:
                if (portalInfo.placeType == PortalInfo.PlaceType.DefinedArea)
                {
                    if (inputFieldPortalDefinedAreaName.text.Length < 1)
                    {
                        break;
                    }

                    if (Input.GetMouseButton(0))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        RaycastHit hitInfo;
                        /*                        if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, layerMaskUI))
                                                {
                                                    break;
                                                }*/

                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            Debug.Log(hitInfo.transform.tag);

                            int x = (int)hitInfo.point.x;
                            int y = (int)hitInfo.point.y;
                            if (pastPortalPos == new Vector2(x, y))
                            {
                                print("already exists");
                                return;
                            }

                            GameObject tile = Instantiate(portalPrefab);
                            tile.transform.SetParent(portalParent);
                            tile.transform.localPosition = new Vector3(x, y, definedAreaZ);
                            Portal2D_L portal2D = tile.GetComponent<Portal2D_L>();
                            portal2D.portalInfo = new PortalInfo();
                            portal2D.portalInfo.definedAreaName = inputFieldPortalDefinedAreaName.text;
                            portal2D.portalInfo.moveType = portalInfo.moveType;
                            portal2D.portalInfo.placeType = portalInfo.placeType;
                            portal2D.portalInfo.position = tile.transform.localPosition;

                            pastPortalPos.x = x;
                            pastPortalPos.y = y;
                        }
                    }
                }
                else if (portalInfo.placeType == PortalInfo.PlaceType.OtherMap)
                {
                    if (inputFieldPortalOtherMapName.text.Length < 1)
                    {
                        break;
                    }
                    if (Input.GetMouseButton(0))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        RaycastHit hitInfo;
                        /*                        if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, layerMaskUI))
                                                {
                                                    break;
                                                }*/

                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            Debug.Log(hitInfo.transform.tag);

                            int x = (int)hitInfo.point.x;
                            int y = (int)hitInfo.point.y;
                            if (pastPortalPos == new Vector2(x, y))
                            {
                                print("already exists");
                                return;
                            }

                            GameObject tile = Instantiate(portalPrefab);
                            tile.transform.SetParent(portalParent);
                            tile.transform.localPosition = new Vector3(x, y, definedAreaZ);
                            Portal2D_L portal2D = tile.GetComponent<Portal2D_L>();
                            portal2D.portalInfo = new PortalInfo();
                            portal2D.portalInfo.mapName = inputFieldPortalOtherMapName.text;
                            portal2D.portalInfo.moveType = portalInfo.moveType;
                            portal2D.portalInfo.placeType = portalInfo.placeType;
                            portal2D.portalInfo.position = tile.transform.localPosition;

                            pastPortalPos.x = x;
                            pastPortalPos.y = y;
                        }
                    }
                }
                break;
            case TileEffectType.DefinedArea:
                if (inputFieldDefinedAreaName.text.Length < 1)
                {//지정 영역의 이름이 입력되어 있지 않으면 안 함
                    break;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Transform littleParent2 = definedAreaParent.Find(inputFieldDefinedAreaName.text);
                    if (littleParent2 == null)
                    {
                        littleParent2 = new GameObject(inputFieldDefinedAreaName.text).transform;
                        littleParent2.SetParent(definedAreaParent);
                        littleParent2.localPosition = Vector3.zero;
                    }
                }
                if (Input.GetMouseButton(0))
                {
                    Transform littleParent = definedAreaParent.Find(inputFieldDefinedAreaName.text);
                    if (littleParent)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        RaycastHit hitInfo;

                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            Debug.Log(hitInfo.transform.tag);

                            int x = (int)hitInfo.point.x;
                            int y = (int)hitInfo.point.y;
                            if (pastDefinedAreaPos == new Vector2(x, y))
                            {
                                print("already exists");
                                return;
                            }

                            GameObject tile = Instantiate(definedAreaPrefab);
                            tile.transform.SetParent(littleParent);
                            tile.transform.localPosition = new Vector3(x, y, definedAreaZ);
                            tile.GetComponentInChildren<Text>().text = inputFieldDefinedAreaName.text;

                            pastDefinedAreaPos.x = x;
                            pastDefinedAreaPos.y = y;


                        }

                    }
                }
                break;
            case TileEffectType.Wall:
                if (Input.GetMouseButton(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        int x = (int)hitInfo.point.x;
                        int y = (int)hitInfo.point.y;
                        if (pastWallPos == new Vector2(x, y))
                        {
                            print("already exists");
                            return;
                        }
                        /*                    if(Physics.OverlapBox(new Vector3(x,y,floorTileZ),new Vector3(0.5f,0.5f,0.5f),Quaternion.identity,tileLayerMask).Length>0){
                                                print("Already Exists");
                                                return;
                                                //이미 타일이 있는 경우
                                            }*/
                        GameObject tile = Instantiate(wallTilePrefab);
                        tile.transform.SetParent(GameObject.Find("WallParent").transform);
                        tile.transform.localPosition = new Vector3(x, y, wallTileZ);
                        pastWallPos.x = x;
                        pastWallPos.y = y;
                    }
                }
                break;
            case TileEffectType.SpawnPoint:
                if (Input.GetMouseButton(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        int x = (int)hitInfo.point.x;
                        int y = (int)hitInfo.point.y;
                        if (pastSpawnPointPos == new Vector2(x, y))
                        {
                            print("already exists");
                            return;
                        }
                        /*                    if(Physics.OverlapBox(new Vector3(x,y,floorTileZ),new Vector3(0.5f,0.5f,0.5f),Quaternion.identity,tileLayerMask).Length>0){
                                                print("Already Exists");
                                                return;
                                                //이미 타일이 있는 경우
                                            }*/
                        GameObject tile = Instantiate(spawnPrefab);
                        tile.transform.SetParent(GameObject.Find("SpawnPointParent").transform);
                        tile.transform.localPosition = new Vector3(x, y, wallTileZ);
                        pastSpawnPointPos.x = x;
                        pastSpawnPointPos.y = y;
                    }
                }
                break;
            case TileEffectType.FaceChat:
                if (Input.GetMouseButton(0))
                {
                    Transform littleParent = faceChatParent.Find(faceChatAreaName.options[faceChatAreaName.value].text);
                    if (littleParent)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        RaycastHit hitInfo;

                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            int x = (int)hitInfo.point.x;
                            int y = (int)hitInfo.point.y;
                            if (pastFaceChatPos == new Vector2(x, y))
                            {
                                print("already exists");
                                return;
                            }
                            GameObject tile = Instantiate(faceChatPrefab);
                            tile.transform.SetParent(littleParent);
                            tile.transform.localPosition = new Vector3(x, y, definedAreaZ);
                            tile.GetComponent<NewBehaviourScript>()._channelName = faceChatAreaName.options[faceChatAreaName.value].text;
                            pastFaceChatPos.x = x;
                            pastFaceChatPos.y = y;
                        }

                    }
                }
                break;
        }
    }

    void MoveCursorSquare()
    {
        Vector2 currMousePos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        currMousePos.x = (int)currMousePos.x;
        currMousePos.y = (int)currMousePos.y;

        cursorSquare.position = currMousePos;
    }

    void Arrow()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseClickPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 dir = Camera.main.ScreenToViewportPoint((Vector2)Input.mousePosition - mouseClickPos);
            Vector3 move = dir * arrowDragSpeed * Time.deltaTime;
            Camera.main.transform.Translate(-move);
        }
        if (fileManger.mapSize == 0)
        {
            float x = Mathf.Clamp(Camera.main.transform.position.x, -3f, 11f);
            float y = Mathf.Clamp(Camera.main.transform.position.y, 0f, 3f);
            Camera.main.transform.position = new Vector3(x, y, -100f);
        }
        else if (fileManger.mapSize == 1)
        {
            float x = Mathf.Clamp(Camera.main.transform.position.x, -33f, 41f);
            float y = Mathf.Clamp(Camera.main.transform.position.y, -15f, 18f);
            Camera.main.transform.position = new Vector3(x, y, -100f);
        }
        else if (fileManger.mapSize == 2)
        {
            float x = Mathf.Clamp(Camera.main.transform.position.x, -63f, 71f);
            float y = Mathf.Clamp(Camera.main.transform.position.y, -30f, 33f);
            Camera.main.transform.position = new Vector3(x, y, -100f);
        }
    }

    void Dropper()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.tag == "Tile")
                {
                    currClickedTileTexture = hitInfo.transform.GetComponent<MeshRenderer>().material.mainTexture;
                    toolType = ToolType.Stamp;
                }
            }
        }
    }

    void Erase()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                print(hitInfo.transform.gameObject.name);

                if (hitInfo.transform.tag == "Tile" || hitInfo.transform.tag == "Object")
                {
                    Destroy(hitInfo.transform.gameObject);
                }
                if (hitInfo.transform.tag == "_3DObject")
                {
                    Destroy(hitInfo.transform.parent.gameObject);
                }
            }
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 터치한 좌표를 가져 옴
            Ray2D ray2D = new Ray2D(wp, Vector2.zero);

            RaycastHit2D hit2D = Physics2D.Raycast(Vector2.zero, wp);

            if (hit2D)
            {
                if (hit2D.transform.tag == "Tile" || hit2D.transform.tag == "Object")
                {
                    Destroy(hit2D.transform.gameObject);
                }
            }
        }
    }

    void Stamp()
    {
        if (Input.GetMouseButton(0))
        {
            switch (placementType)
            {
                case PlacementType.Floor:
                    CreateTile();
                    break;

            }



        }
    }

    void CreateTile()
    {
        if (currClickedTileTexture)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log(hitInfo.transform.tag);

                if (hitInfo.transform.tag == "BackGround")
                {
                    int x = (int)hitInfo.point.x;
                    int y = (int)hitInfo.point.y;
                    if (pastTilePos == new Vector2(x, y))
                    {
                        print("already exists");
                        return;
                    }
                    /*                    if(Physics.OverlapBox(new Vector3(x,y,floorTileZ),new Vector3(0.5f,0.5f,0.5f),Quaternion.identity,tileLayerMask).Length>0){
                                            print("Already Exists");
                                            return;
                                            //이미 타일이 있는 경우
                                        }*/
                    GameObject tile = Instantiate(floorTilePrefab);
                    tile.transform.SetParent(tileParent);
                    tile.transform.localPosition = new Vector3(x, y, floorTileZ);
                    tile.GetComponent<MeshRenderer>().material.mainTexture = currClickedTileTexture;
                    pastTilePos.x = x;
                    pastTilePos.y = y;

                }
                else if (hitInfo.transform.tag == "Tile")
                {
                    hitInfo.transform.GetComponent<MeshRenderer>().material.mainTexture = currClickedTileTexture;
                }
                //이미 오브젝트 있는지 확인?
                //이전 위치 저장

            }
        }
    }

    #region 버튼 OnClick 함수

    public void OnClickBtnFloor()
    {
        placementType = PlacementType.Floor;
        TurnOnUi(1);
        print("On Click Btn Floor");
    }


    public void OnClickBtnEraser()
    {
        toolType = ToolType.Eraser;
    }

    public void OnClickBtnStamp()
    {
        toolType = ToolType.Stamp;
    }

    public void OnClickBtnDropper()
    {
        toolType = ToolType.Dropper;
    }

    public void OnClickBtnArrow()
    {
        toolType = ToolType.Arrow;
    }

    public void OnClickBtnFloorExample()
    {
        currClickedTileTexture = ConvertSpriteToTexture(EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite);
    }

    public void OnClickBtnTileEffect()
    {
        placementType = PlacementType.TileEffect;
        TurnOnUi(0);
    }
    public void OnClickBtnObject()
    {
        placementType = PlacementType.Object;
        TurnOnUi(2);
    }
    public void OnClickBtnUpperObject()
    {
        placementType = PlacementType.UpperObject;
        TurnOnUi(3);
    }

    public void OnClickBtnMyObject()
    {
        bool b = scrollViewMyObject.activeSelf ? false : true;
        scrollViewMyObject.SetActive(b);
        inputFieldTextObj.gameObject.SetActive(false);
    }

    public void OnClickBtnMyUpperObject()
    {
        bool b = scrollViewMyUpperObject.activeSelf ? false : true;
        scrollViewMyUpperObject.SetActive(b);
    }

    public void OnClickBtnPortalToDefinedArea()
    {
        portalInfo.placeType = PortalInfo.PlaceType.DefinedArea;
        panelPortalToDefinedArea.SetActive(true);
        panelPortalToOtherMap.SetActive(false);

        checkImageDefinedArea.SetActive(true);
        checkImageOtherMap.SetActive(false);
    }

    public void OnClickBtnPortalToOtherMap()
    {
        portalInfo.placeType = PortalInfo.PlaceType.OtherMap;
        panelPortalToDefinedArea.SetActive(false);
        panelPortalToOtherMap.SetActive(true);

        checkImageDefinedArea.SetActive(false);
        checkImageOtherMap.SetActive(true);
    }

    public void OnClickBtnPortalMoveKey()
    {
        portalInfo.moveType = PortalInfo.MoveType.Key;
        checkImageKey.SetActive(true);
        checkImageInstant.SetActive(false);
    }

    public void OnClickBtnPortalMoveInstant()
    {
        portalInfo.moveType = PortalInfo.MoveType.Instant;
        checkImageKey.SetActive(false);
        checkImageInstant.SetActive(true);
    }

    public void OnClickBtnPortalDefinedMoveKey()
    {
        portalInfo.moveType = PortalInfo.MoveType.Key;
        checkImageKey_DefinedArea.SetActive(true);
        checkImageInstant_DefinedArea.SetActive(false);
    }

    public void OnClickBtnPortalDefinedMoveInstant()
    {
        portalInfo.moveType = PortalInfo.MoveType.Instant;
        checkImageKey_DefinedArea.SetActive(false);
        checkImageInstant_DefinedArea.SetActive(true);
    }

    void TurnOnUi(int index)
    {
        for (int i = 0; i < canvas.transform.childCount - 1; i++)
        {
            canvas.transform.GetChild(i).gameObject.SetActive(false);
        }
        canvas.transform.GetChild(index).gameObject.SetActive(true);

    }
    public void OnClickBtnPortal()
    {
        tileEffectType = TileEffectType.Portal;
    }

    public void OnClickBtnDefinedArea()
    {
        tileEffectType = TileEffectType.DefinedArea;
    }
    public void OnClickBtnWall()
    {
        tileEffectType = TileEffectType.Wall;
    }
    public void OnClickBtnFaceChat()
    {
        tileEffectType = TileEffectType.FaceChat;
    }
    public void OnClickBtnSpawnPoint()
    {
        tileEffectType = TileEffectType.SpawnPoint;
    }

    #endregion

    public Texture ConvertSpriteToTexture(Sprite sprite)
    {
        try
        {
            if (sprite.rect.width != sprite.texture.width)
            {
                int x = Mathf.FloorToInt(sprite.textureRect.x);
                int y = Mathf.FloorToInt(sprite.textureRect.y);
                int width = Mathf.FloorToInt(sprite.textureRect.width);
                int height = Mathf.FloorToInt(sprite.textureRect.height);

                Texture2D newText = new Texture2D(width, height);
                Color[] newColors = sprite.texture.GetPixels(x, y, width, height);

                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            }
            else
                return sprite.texture;
        }
        catch
        {
            return sprite.texture;
        }
    }
}
