using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



public class FindSimilarImage_H : MonoBehaviour
{
    public RawImage myImage;
    public RawImage findImage;
    public Button findButton;
    public GameObject bg1;
    public GameObject bg2;
    string artWorkURL = "/auths/art";
    byte[] myWork;
    // Start is called before the first frame update
    void Start()
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
    public void OpenFileExplorer()
    {
        string path = WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
        //path = EditorUtility.OpenFilePanel("Show all images(.jpg)", "", "jpg");
        StartCoroutine(GetTexture(path));
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
            myImage.texture = myTexture;
            findButton.interactable = true;
            myWork = www.downloadHandler.data;
            //StartCoroutine(WebRequest_Card(tex));
        }
    }
    IEnumerator WebRequest_ArtWork(byte[] path)
    {
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormFileSection("file", path, "id_card.jpg", "image/jpg"));
        using (UnityWebRequest request = UnityWebRequest.Post(GameManager.instance.url + artWorkURL, form))
        {
            //byte[] b = Encoding.UTF8.GetBytes(jsonData);
            //request.uploadHandler = new UploadHandlerRaw(b);
            //request.SetRequestHeader("Content-Type", "multipart/form-data");
            //request.uploadHandler = new UploadHandlerRaw(path);
            //request.SetRequestHeader("Content-Type", "application/json");
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("작품 추천받기 성공!");
                MapUpdate result = JsonUtility.FromJson<MapUpdate>(request.downloadHandler.text);
                StartCoroutine(GetImage(result.data));
                Debug.Log(result.data);
            }
            request.Dispose();
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
            findImage.texture = ((DownloadHandlerTexture)ww.downloadHandler).texture;
        }
    }
    public void OnclickUploadButton()
    {
        OpenFileExplorer();
    }
    public void OnclickFindButton()
    {
        StartCoroutine(WebRequest_ArtWork(myWork));
    }

    public void RollBack()
    {
        myImage.texture = null;
        findImage.texture = null;
        findButton.interactable = false;
        bg2.SetActive(false);
        bg1.SetActive(true);
        myWork = null;
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
