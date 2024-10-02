using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyRoomManager_H : MonoBehaviour
{
    public string GetSpaceInfoURL = "/spaces";
    public GameObject mainRoomPrefab;
    public Transform myRoomParent;
    public MainManager_L mainManager;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WebRequest_GetMapData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator WebRequest_GetMapData()
    {
        ///members/item
        using (UnityWebRequest request = UnityWebRequest.Get(GameManager.instance.url + GetSpaceInfoURL))
        {
            Debug.Log(GameManager.instance.url + GetSpaceInfoURL);
            //request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", GameManager.instance.userinfo.token);
            Debug.Log(GameManager.instance.userinfo.token);
            yield return request.SendWebRequest();
            Debug.Log("??");
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("내 맵 받아오기 성공!  " + System.Text.Encoding.Default.GetString(request.downloadHandler.data));
                GetMapDataWhole maps = JsonUtility.FromJson<GetMapDataWhole>(System.Text.Encoding.Default.GetString(request.downloadHandler.data));
                for(int i =0; i<maps.data.Count; i++)
                {
                    GameObject go = Instantiate(mainRoomPrefab, myRoomParent);
                    go.GetComponent<MajorName_L>().majorSceneName = "RoomScene_H";
                    go.GetComponent<MajorName_L>().mapData = maps.data[i];
                    go.GetComponent<Button>().onClick.AddListener(mainManager.OnClickMajor);
                    StartCoroutine(GetImage(maps.data[i].spaceThumbnail,go.transform.GetChild(1).GetComponent<Image>()));
                    go.transform.GetChild(2).GetComponent<Text>().text = maps.data[i].spaceName;
                    if(i > 8)
                    {
                        break;
                    }
                }    
            }
        }
    }
    void SetMapData(MapData mapData )
    {
        GameManager.instance.mapData = mapData;
    }

    IEnumerator GetImage(string url,Image image)
    {
        UnityWebRequest ww = UnityWebRequestTexture.GetTexture(url);
        yield return ww.SendWebRequest();
        if (ww.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(ww.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)ww.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            image.sprite = sprite;
        }
    }
}
