using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SendFavorite
{
    public int spaceCode;
}
[System.Serializable]
public class PureWholeRequestIntData
{
    public string status;
    public string message;
    public int data;
}
[System.Serializable]
public class PureWholeRequestStringData
{
    public string status;
    public string message;
    public string data;
}
[System.Serializable]
public class MyFavoriteListWhole
{
    public string status;
    public string message;
    public List<MyFavorite> data;
}
[System.Serializable]
public class MyLikeListWhole
{
    public string status;
    public string message;
    public List<MyLike> data;
}
[System.Serializable]
public class MyLike
{
    public int likeCode;
    public int spaceCode;
    public int memberCode;
}
[System.Serializable]
public class MyFavorite
{
    public int bookmarkCode;
    public int spaceCode;
    public int memberCode;
}

public class SpaceInfoBG_H : MonoBehaviour
{
    string SetFavoriteURL = "/bookmarks/insert";
    string GetFavoriteURL = "/bookmarks";
    string DeleteFavoriteURL = "/bookmarks/delete/";

    string SetLikeURL = "/likes/insert";
    string GetLikeURL = "/likes";
    string DeleteLikeURL = "/likes/delete/";

    public Text RoomNameText;
    public Text RecommendText;
    public Text RoomInstruductionText;
    public Text RoomKeyWord1Text;
    public Text RoomKeyWord2Text;
    public Text RoomKeyWord3Text;

    public Image starImage;

    public Sprite grayStar;
    public Sprite yellowStar;

    int favoriteCode;
    int likeCode;

    bool Ilike = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WebRequest_GetFavoriteURL());
        StartCoroutine(WebRequest_GetLikeURL());
        RoomNameText.text = GameManager.instance.mapData.spaceName;
        RecommendText.text = GameManager.instance.mapData.spaceLike.ToString();
        RoomInstruductionText.text = GameManager.instance.mapData.spaceIntro;
        RoomKeyWord1Text.text = "# " + GameManager.instance.mapData.spaceTag1;
        RoomKeyWord2Text.text = "# " + GameManager.instance.mapData.spaceTag2;
        RoomKeyWord3Text.text = "# " + GameManager.instance.mapData.spaceTag3;
    }
    public IEnumerator WebRequest_PostFavoriteURL()
    {
        SendFavorite sendFavorite = new SendFavorite();
        sendFavorite.spaceCode = GameManager.instance.mapData.spaceCode;
        string form = JsonUtility.ToJson(sendFavorite);
        using (UnityWebRequest request = UnityWebRequest.Post(GameManager.instance.url + SetFavoriteURL, form))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(form);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("Authorization", GameManager.instance.userinfo.token);
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("즐겨찾기 추가 성공!");
                PureWholeRequestIntData result = JsonUtility.FromJson<PureWholeRequestIntData>(System.Text.Encoding.Default.GetString(request.downloadHandler.data));
                favoriteCode = result.data;
                starImage.sprite = yellowStar;
            }
            request.Dispose();
        }
    }
    public IEnumerator WebRequest_GetFavoriteURL()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(GameManager.instance.url + GetFavoriteURL))
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
                MyFavoriteListWhole myFavoriteList = JsonUtility.FromJson<MyFavoriteListWhole>(System.Text.Encoding.Default.GetString(request.downloadHandler.data));
                for (int i =0; i < myFavoriteList.data.Count; i++)
                {
                    if(myFavoriteList.data[i].spaceCode == GameManager.instance.mapData.spaceCode)
                    {
                        favoriteCode = myFavoriteList.data[i].bookmarkCode;
                        starImage.sprite = yellowStar;
                    }
                }
                Debug.Log("즐겨찾기 조회 성공!");
            }
            request.Dispose();
        }
    }
    public IEnumerator WebRequest_DeleteFavoriteURL()
    {
        using (UnityWebRequest request = UnityWebRequest.Delete(GameManager.instance.url + DeleteFavoriteURL + favoriteCode))
        {
            //request.SetRequestHeader("Authorization", GameManager.instance.userinfo.token);
            //request.SetRequestHeader("Content-Type","multipart/form-data");
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("즐겨찾기 삭제 성공!");
                starImage.sprite = grayStar;
                
            }
            request.Dispose();
        }
    }
    public IEnumerator WebRequest_PostLikeURL()
    {
        SendFavorite sendFavorite = new SendFavorite();
        sendFavorite.spaceCode = GameManager.instance.mapData.spaceCode;
        string form = JsonUtility.ToJson(sendFavorite);
        using (UnityWebRequest request = UnityWebRequest.Post(GameManager.instance.url + SetLikeURL, form))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(form);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("Authorization", GameManager.instance.userinfo.token);
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("좋아요 추가 성공!");
                PureWholeRequestIntData result = JsonUtility.FromJson<PureWholeRequestIntData>(System.Text.Encoding.Default.GetString(request.downloadHandler.data));
                likeCode = result.data;
                RecommendText.text = (int.Parse(RecommendText.text) + 1).ToString();
                Ilike = true;
            }
            request.Dispose();
        }
    }
    public IEnumerator WebRequest_GetLikeURL()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(GameManager.instance.url + GetLikeURL))
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
                MyLikeListWhole myFavoriteList = JsonUtility.FromJson<MyLikeListWhole>(System.Text.Encoding.Default.GetString(request.downloadHandler.data));
                for (int i = 0; i < myFavoriteList.data.Count; i++)
                {
                    if (myFavoriteList.data[i].spaceCode == GameManager.instance.mapData.spaceCode)
                    {
                        likeCode = myFavoriteList.data[i].likeCode;
                        Ilike = true;
                    }
                }
                Debug.Log("즐겨찾기 조회 성공!");
            }
            request.Dispose();
        }
    }
    public IEnumerator WebRequest_DeleteLikeURL()
    {
        using (UnityWebRequest request = UnityWebRequest.Delete(GameManager.instance.url + DeleteLikeURL + likeCode))
        {
            //request.SetRequestHeader("Authorization", GameManager.instance.userinfo.token);
            //request.SetRequestHeader("Content-Type","multipart/form-data");
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("좋아요 삭제 성공!");
                Ilike = false;
                RecommendText.text = (int.Parse(RecommendText.text) - 1).ToString();
            }
            request.Dispose();
        }
    }
    public void StarButton()
    {
        if(starImage.sprite == grayStar)
        {
            StartCoroutine(WebRequest_PostFavoriteURL());
        }
        else if(starImage.sprite == yellowStar)
        {
            StartCoroutine(WebRequest_DeleteFavoriteURL());
        }
    }
    public void RecommendButton()
    {
        if(Ilike == false)
        {
            StartCoroutine(WebRequest_PostLikeURL());
        }
        else
        {
            StartCoroutine(WebRequest_DeleteLikeURL());
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}