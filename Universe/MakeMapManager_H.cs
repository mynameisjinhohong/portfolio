using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[System.Serializable]
public class MapDataWhole
{
    public string status;
    public string message;
    public MapData data;
}
[System.Serializable]
public class GetMapDataWhole
{
    public string status;
    public string message;
    public List<MapData> data;
}
[System.Serializable]
public class MapData
{
    public int spaceCode;
    public int memberCode;
    public string spaceName;
    public string spaceIntro;
    public string spaceType;
    public string sapcePassword;
    public string spaceMapinfo;
    public int spaceLike;
    public string spaceThumbnail;
    public int spaceReport;
    public int spaceWarning;
    public string spaceTag1;
    public string spaceTag2;
    public string spaceTag3;
}
[System.Serializable]
public class MapPutData
{
    public int spaceCode;
    public string spaceName;
    public string spaceIntro;
    public string spaceType;
    public string sapcePassword;
    public string spaceTag1;
    public string spaceTag2;
    public string spaceTag3;
}
[System.Serializable]
public class AIResPondWord
{
    public int status;
    public string message;
    public bool data;
}

public class MakeMapManager_H : MonoBehaviour
{
    public string addSpaceURL = "/spaces/maps/upload";
    public string putSpaceURL = "/spaces/upload";
    public ToggleController controller;
    public InputField roomPasswordInputField;
    public InputField roomNameInputField;
    public InputField roomIntroductionInputField;
    Button makeButton;
    public int whatMap;
    public Sprite passwordOn;
    public Sprite passwordOff;
    Image myImage;
    [Header("로딩")]
    public GameObject panelLoading;


    MajorRoomManager_L majorRoomManager;
    private string reportURL = "/auths/hate";

    bool nameRight = false;
    bool introRight = false;

    GameObject nameWrong;
    GameObject introWrong;
    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<Image>();
        majorRoomManager = GameObject.Find("MajorRoomManager").GetComponent<MajorRoomManager_L>();
        makeButton = gameObject.transform.Find("MakeMapButton").GetComponent<Button>();
        roomNameInputField.onEndEdit.AddListener(CheckRightWord);
        roomIntroductionInputField.onEndEdit.AddListener(CheckRightWordIntro);
        nameWrong = gameObject.transform.Find("NameWrong").gameObject;
        introWrong = gameObject.transform.Find("IntroWrong").gameObject;
    }
    public void CheckRightWord(string s)
    {
        StartCoroutine(WebRequest_ReportPostName(s));
    }
    public void CheckRightWordIntro(string s)
    {
        StartCoroutine(WebRequest_ReportPostIntro(s));
    }
    public void SetWhatMap(int what)
    {
        whatMap = what;
    }
    // Update is called once per frame
    void Update()
    {
        if (controller.isOn)
        {
            roomPasswordInputField.gameObject.SetActive(true);
            roomPasswordInputField.text = "";
            myImage.sprite = passwordOn;
        }
        else
        {
            roomPasswordInputField.gameObject.SetActive(false);
            roomPasswordInputField.text = "";
            myImage.sprite = passwordOff;
        }

        if(nameRight && introRight)
        {
            makeButton.interactable = true;
        }
        else
        {
            makeButton.interactable = false;
        }
    }
    public void MakeMap()
    {
        //로딩씬
        panelLoading.SetActive(true);

        if(whatMap == 0)
        {
            //string jsonData = File.ReadAllText(Application.dataPath + "/Resources/Resources_H/MapData/PresetMap0.txt");
            TextAsset jsonDatatxt = Resources.Load<TextAsset>("Resources_H/MapData/PresetMap0");
            string jsonData = jsonDatatxt.text; 
            SpaceInfo info = JsonUtility.FromJson<SpaceInfo>(jsonData);
            info.spaceIntroduction = roomIntroductionInputField.text;
            info.passWord = roomPasswordInputField.text;
            info.spaceName = roomNameInputField.text;
            string jsonMap = JsonUtility.ToJson(info, true);
            StartCoroutine(WebRequest_PostSpaceURL(jsonMap));
            //File.WriteAllText(Application.dataPath + "/Resources/Resources_H/MapData" + "/" + SpaceInfo.spaceName + ".txt", jsonMap);
            //System.IO.Directory.GetCurrentDirectory()
        }
        else
        {
            switch (whatMap)
            {
                case 1:
                    //File.Copy(Application.dataPath + "/Resources/Resources_H/MapData/PresetMap1.txt", Application.dataPath + "/Resources/Resources_H/MapData/"+ roomNameInputField.text+".txt");
                    //string jsonData = File.ReadAllText(Application.dataPath + "/Resources/Resources_H/MapData/PresetMap1.txt");
                    TextAsset jsonDatatxt = Resources.Load<TextAsset>("Resources_H/MapData/PresetMap1");
                    string jsonData = jsonDatatxt.text;
                    SpaceInfo spaceInfo = JsonUtility.FromJson<SpaceInfo>(jsonData);
                    spaceInfo.spaceName = roomNameInputField.text;
                    //spaceInfo.makerID = GameManager.instance.userinfo.id;
                    spaceInfo.passWord = roomPasswordInputField.text;
                    spaceInfo.spaceIntroduction = roomIntroductionInputField.text;
                    string jsonMap = JsonUtility.ToJson(spaceInfo, true);
                    StartCoroutine(WebRequest_PostSpaceURL(jsonMap));
                    // File.WriteAllText(Application.dataPath + "/Resources/Resources_H/MapData" + "/" + SpaceInfo.spaceName + ".txt", jsonMap);
                    break;
                case 2:
                    //File.Copy(Application.dataPath + "/Resources/Resources_H/MapData/PresetMap2.txt", Application.dataPath + "/Resources/Resources_H/MapData/" + roomNameInputField.text + ".txt");
                    //string jsonData1 = File.ReadAllText(Application.dataPath + "/Resources/Resources_H/MapData/PresetMap2.txt");
                    TextAsset jsonDatatxt1 = Resources.Load<TextAsset>("Resources_H/MapData/PresetMap2");
                    string jsonData1 = jsonDatatxt1.text;
                    SpaceInfo spaceInfo1 = JsonUtility.FromJson<SpaceInfo>(jsonData1);
                    spaceInfo1.spaceName = roomNameInputField.text;
                    //spaceInfo.makerID = GameManager.instance.userinfo.id;
                    spaceInfo1.passWord = roomPasswordInputField.text;
                    spaceInfo1.spaceIntroduction = roomIntroductionInputField.text;
                    string jsonMap1 = JsonUtility.ToJson(spaceInfo1, true);
                    StartCoroutine(WebRequest_PostSpaceURL(jsonMap1));
                    //File.WriteAllText(Application.dataPath + "/Resources/Resources_H/MapData" + "/" + SpaceInfo.spaceName + ".txt", jsonMap1);
                    break;
            }
                
        }

       

    }

    public IEnumerator WebRequest_PostSpaceURL(string json)
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddBinaryData("file", System.Text.Encoding.UTF8.GetBytes(json));
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();        
        form.Add(new MultipartFormFileSection(json,"file.txt")); 
        
        using (UnityWebRequest request = UnityWebRequest.Post(GameManager.instance.url + addSpaceURL, wwwForm))
        {
            request.SetRequestHeader("Authorization", GameManager.instance.userinfo.token);
            //request.SetRequestHeader("Content-Type","multipart/form-data");
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                MapDataWhole result = JsonUtility.FromJson<MapDataWhole>(System.Text.Encoding.Default.GetString(request.downloadHandler.data));
                Debug.Log("스페이스 포스트 성공!" + result.data.spaceCode);
                SpaceInfo.spaceURL = result.data.spaceMapinfo;
                SpaceInfo.spaceCode = result.data.spaceCode;
                StartCoroutine(WebRequest_PutSpaceURL(result.data));
            }
            request.Dispose();
        }
    }
    public IEnumerator WebRequest_PutSpaceURL(MapData data)
    {
        data.spaceIntro = roomIntroductionInputField.text;
        data.sapcePassword = roomPasswordInputField.text;
        data.spaceName = roomNameInputField.text;
        if (SceneManager.GetActiveScene().name.Contains("AIMinorRoom_H") || SceneManager.GetActiveScene().name.Contains("CSMajorRoom_H"))
        {
            data.spaceType = "computer";
        }
        else if (SceneManager.GetActiveScene().name.Contains("ArtMajorRoom_L")|| SceneManager.GetActiveScene().name.Contains("PaintingMinorRoom_L"))
        {
            data.spaceType = "art";
        }
        GameManager.instance.mapData = data;
        MapPutData mapPut = new MapPutData();
        mapPut.spaceCode = data.spaceCode;
        mapPut.spaceName = data.spaceName;
        mapPut.spaceIntro = data.spaceIntro;
        mapPut.spaceType = data.spaceType;
        mapPut.spaceTag1 = data.spaceTag1;
        mapPut.spaceTag2 = data.spaceTag2;
        mapPut.spaceTag3 = data.spaceTag3;
        //GameManager.instance.mapData.spaceCode = mapPut.spaceCode;
        //GameManager.instance.mapData.spaceName = mapPut.spaceName;
        //GameManager.instance.mapData.spaceIntro = mapPut.spaceIntro;
        //GameManager.instance.mapData.spaceType = mapPut.spaceType;
        //GameManager.instance.mapData.sapcePassword = mapPut.sapcePassword;
        //GameManager.instance.mapData.spaceTag1 = mapPut.spaceTag1;
        //GameManager.instance.mapData.spaceTag2 = mapPut.spaceTag2;
        //GameManager.instance.mapData.spaceTag3 = mapPut.spaceTag3;
        string form = JsonUtility.ToJson(mapPut);
        using (UnityWebRequest request = UnityWebRequest.Put(GameManager.instance.url + putSpaceURL, form))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            //request.SetRequestHeader("Authorization", GameManager.instance.userinfo.token);
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("맵정보 추가 성공!");
                majorRoomManager.GoUserRoomScene();
                //SceneManager.LoadScene("RoomScene_H");
            }
            request.Dispose();
        }
    }
    public IEnumerator WebRequest_ReportPostName(string chat)
    {
        WWWForm www = new WWWForm();
        //www.AddBinaryData("text", System.Text.Encoding.UTF8.GetBytes(chat));
        www.AddField("text", chat);
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormFileSection(chat,"text"));
        Debug.Log(GameManager.instance.url + reportURL);
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
                nameRight = ai.data;

                nameWrong.SetActive(!ai.data);
                Debug.Log("AI혐오표현 통신 성공!" + ai.data);
            }
            request.Dispose();
        }
    }
    public IEnumerator WebRequest_ReportPostIntro(string chat)
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
                introRight = ai.data;

                introWrong.SetActive(!ai.data);
                Debug.Log("AI혐오표현 통신 성공!");
            }
            request.Dispose();
        }
    }
    public void XButton()
    {
        gameObject.SetActive(false);
    }
    public  void TurnOnMakeMap()
    {
        gameObject.SetActive(true);
    }
}