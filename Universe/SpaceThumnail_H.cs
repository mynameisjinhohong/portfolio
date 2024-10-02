using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SpaceThumnail_H : MonoBehaviour
{
    public RawImage thumNail;
    string thumNailURL = "/spaces/thumbnails/upload";
    //string thumNailURL2 = "/spaces/thumbnails/upload2";
    // Start is called before the first frame update
    void Start()
    {
        GetImage(GameManager.instance.mapData.spaceThumbnail);
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
            thumNail.texture = ((DownloadHandlerTexture)ww.downloadHandler).texture;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
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
    public void SetThumNail()
    {
        StartCoroutine(GetTexture(WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false))));
    }
    IEnumerator GetTexture(string path)
    {
        //xCardButton.interactable = false;
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
            thumNail.texture = convertedTexture;
            byte[] a = convertedTexture.EncodeToJPG();
            StartCoroutine(WebRequest_ThumNail1(a));
            //StartCoroutine(WebRequest_ThumNail2(a));
            //if (Directory.Exists(pythonDirectory) == false)
            //{
            //    Directory.CreateDirectory(pythonDirectory);
            //}
            //File.WriteAllBytes(pythonDirectory + "/cardImage.jpg", textuerData);
        }
    }
    IEnumerator WebRequest_ThumNail1(byte[] path)
    {
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormFileSection("ThumNail", path, "ThumNailImg.png", "image/png"));
        WWWForm wwform = new WWWForm();
        wwform.AddBinaryData("file", path);
        using (UnityWebRequest request = UnityWebRequest.Post(GameManager.instance.url + thumNailURL, wwform))
        {
            Debug.Log(GameManager.instance.url + thumNailURL);
            request.SetRequestHeader("spaceCode", GameManager.instance.mapData.spaceCode.ToString());
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("썸네일 수정 성공!!");
            }
            string result = request.downloadHandler.text;
            Debug.Log(result);
            request.Dispose();
        }
    }
    //IEnumerator WebRequest_ThumNail2(byte[] path)
    //{
    //    List<IMultipartFormSection> form = new List<IMultipartFormSection>();
    //    form.Add(new MultipartFormFileSection("ThumNail", path, "ThumNailImg.png", "image/png"));
    //    WWWForm wwform = new WWWForm();
    //    wwform.AddBinaryData("file", path);
    //    Debug.Log(path.Length);
    //    using (UnityWebRequest request = UnityWebRequest.Post(GameManager.instance.url + thumNailURL2+"/"+GameManager.instance.mapPut.spaceCode, form))
    //    {
    //        //request.SetRequestHeader("spaceCode", GameManager.instance.mapPut.spaceCode.ToString());
    //        //Debug.Log(request.GetRequestHeader("Authorization"));
    //        yield return request.SendWebRequest();
    //        if (request.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(request.error);
    //        }
    //        string result = request.downloadHandler.text;
    //        Debug.Log(result);
    //        request.Dispose();
    //    }
    //}
}
