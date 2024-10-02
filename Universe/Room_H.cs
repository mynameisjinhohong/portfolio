using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Room_H : MonoBehaviour
{
    public string roomURL;
    public Text roomNameText;
    public string roomInfoText;
    public MapData mapData;
    public bool Set = false;
    public Text recommendText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(mapData != null && Set == false)
        {
            roomNameText.text = mapData.spaceName;
            roomInfoText = mapData.spaceIntro;
            roomURL = mapData.spaceMapinfo;
            StartCoroutine(GetImage(mapData.spaceThumbnail));
            recommendText.text = mapData.spaceLike.ToString();

            Set = true;
        }
    }
    IEnumerator GetImage(string url)
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
            transform.GetChild(1).GetComponent<Image>().sprite = sprite;
        }
    }
}
