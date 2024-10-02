using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using SFB;
using System.Text;
using System.Net;

[System.Serializable]
public class Member
{
    public string memberEmail;
    public string memberPassword;
    public string memberName;
    public string memberPhone;
    public string memberMajor;
}
[System.Serializable]
public class MemberID
{
    public string memberEmail;
}
[System.Serializable]
public class MemberCardImage
{
    public byte[] file;
}
[System.Serializable]
public class AICardWhole
{
    public int status;
    public string message;
    public string data;
}


public class FileManager_H : MonoBehaviour
{
    public string singUpURL = "/auths/join";
    public string idCheckURL = "/auths/email";
    string cardURL = "/auths/card";
    public string nickNameURL = "/auths/nickname";

    public Userinfo info = new Userinfo();
    string path;
    public RawImage rawImage;

    public GameObject signUpImage;

    public InputField idInputfield;
    public Text[] OKText; //0 - id, 1- password
    public Text[] NoText;
    public Button findSameIdButton;

    public InputField passwordInputfield;
    public InputField passwordCheckInputfield;

    public InputField nameInputfield;

    public InputField phoneNumInputfield;
    public Button phoneNumCheckButton;
    public Button phoneCheckButtonImage;

    public Button closeImage;
    [SerializeField]
    bool[] singUpCheck = new bool[6] { false, false, false, false, false, false};
    public Button signUpButton;
    public Toggle signUpToggle;

    public GameObject CardUP;
    public Image SelectImage;
    public GameObject noText2;
    private void Start()
    {
        Debug.Log("F");
        idInputfield.onValueChanged.AddListener(OnIDValueChanged);
        passwordInputfield.onValueChanged.AddListener(OnPasswordValueChanged);
        passwordCheckInputfield.onValueChanged.AddListener(OnPasswordCheckValueChanged);
        nameInputfield.onValueChanged.AddListener(OnNameValueChanged);
        phoneNumInputfield.onValueChanged.AddListener(OnPhoneNumValueChanged);
    }

    public void XButton()
    {
        signUpImage.gameObject.SetActive(false);
        IdInput = false;
    }
    bool IdInput = false;
    private void Update()
    {
        #region ¾ÆÀÌµð ÆÄÆ®
        if (idInputfield.text.Length < 1 && IdInput == true)
        {
            NoText[0].gameObject.SetActive(true);
            noText2.SetActive(false);
            OKText[0].gameObject.SetActive(false);
        }
        else if(IdInput == true)
        {
            NoText[0].gameObject.SetActive(false);
        }
        #endregion
        #region °¡ÀÔ ÆÄÆ®
        if (signUpToggle.isOn)
        {
            singUpCheck[5] = true;
        }
        else
        {
            singUpCheck[5] = false;
        }
        for (int i=0;i<singUpCheck.Length;i++)
        {
            if(singUpCheck[i] == false)
            {
                signUpButton.interactable = false;
                break;
            }
            if (i == singUpCheck.Length - 1)
            {
                signUpButton.interactable = true;
            }
        }
        #endregion
    }
    #region ¾ÆÀÌµðÆÄÆ®
    void OnIDValueChanged(string id)
    {
        IdInput = true;
        findSameIdButton.interactable = id.Length > 0;
        singUpCheck[0] = false;
    }
    public void OnIDCheckButton()
    {
        if(idInputfield.text.Length > 0)
        {

            info.id = idInputfield.text;

            StartCoroutine(WebRequest_IDCheck());
            //TestFunc();
        }
    }
    public IEnumerator WebRequest_IDCheck()
    {
        MemberID id = new MemberID();
        id.memberEmail = idInputfield.text;
        List<IMultipartFormSection> data = new List<IMultipartFormSection>();
        data.Add(new MultipartFormDataSection("memberEmail", idInputfield.text));
        string mem = JsonUtility.ToJson(id);
        //"memberEmail":"123"
        //WWWForm form = new WWWForm();
        //form.AddField("memberEmail", 123.ToString());
        using (UnityWebRequest request = UnityWebRequest.Post(GameManager.instance.url + idCheckURL, data))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(mem);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            //request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            //Debug.Log(request.GetRequestHeader("Authorization")
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                noText2.SetActive(true);
                Debug.Log(request.error);

            }
            else
            {
                string result = request.downloadHandler.text;
                Debug.Log(result);
                OKText[0].gameObject.SetActive(true);
                singUpCheck[0] = true;
            }
            request.Dispose();
        }
    }

    void TestFunc()
    {
        MemberID id = new MemberID();
        id.memberEmail = idInputfield.text;
        string mem = JsonUtility.ToJson(id);
        Debug.Log(GameManager.instance.url + idCheckURL);
        Debug.Log(mem);
        
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(GameManager.instance.url + idCheckURL);
        req.ContentType = "application/json";
        req.Method = "POST";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(mem);
        req.ContentLength = jsonToSend.Length;
        Stream reqStream = req.GetRequestStream();
        reqStream.Write(jsonToSend, 0, jsonToSend.Length);
        reqStream.Close();
        HttpWebResponse res = (HttpWebResponse)req.GetResponse();
        StreamReader sr = new StreamReader(reqStream, Encoding.GetEncoding("UTF-8"));
        string msg = sr.ReadToEnd();
        sr.Close();
        res.Close();
        reqStream.Close();
    }
    #endregion
    #region ºñ¹Ð¹øÈ£ ÆÄÆ®
    string passwordChecker;
    void OnPasswordValueChanged(string password)
    {
        info.password = password;
        string str = @"[~!@\#$%^&*\()\=+|\\/:;?""<>']";
        Regex regex = new Regex(str);
        //passwordChecker = Regex.Replace(info.password,@"[ ^0-9a-zA-Z°¡-ÆR ]{1,10}", "", RegexOptions.Singleline);
        if(regex.IsMatch(password))
        {
            NoText[1].gameObject.SetActive(true);
        }
        else
        {
            NoText[1].gameObject.SetActive(false);
        }
    }
    void OnPasswordCheckValueChanged(string password)
    {
        if(password == info.password)
        {
            NoText[2].gameObject.SetActive(false);
            OKText[2].gameObject.SetActive(true);
            singUpCheck[1] = true;
        }
        else
        {
            NoText[2].gameObject.SetActive(true);
            OKText[2].gameObject.SetActive(false);
            singUpCheck[1] = false;
        }
    }
    #endregion
    #region ÀÌ¸§ ÆÄÆ®
    void OnNameValueChanged(string name)
    {
        info.name = name;
        singUpCheck[2] = true;
    }
    #endregion
    #region ÈÞ´ëÆù ¹øÈ£ ÆÄÆ®
    void OnPhoneNumValueChanged(string num)
    {
        phoneNumCheckButton.interactable = (num.Length == 11);
        if(OKText[3].gameObject.activeInHierarchy == false)
        {
            OKText[3].gameObject.SetActive(false);
            NoText[3].gameObject.SetActive(true);
        }


    }
    public void OnClickButtonImage()
    {
        phoneCheckButtonImage.gameObject.SetActive(false);
    }
    public void OnPhoneNumBTNClick()
    {
        info.phoneNumber = phoneNumInputfield.text;
        OKText[3].gameObject.SetActive(true);
        NoText[3].gameObject.SetActive(false);
        singUpCheck[3] = true;
        phoneCheckButtonImage.gameObject.SetActive(true);
    }
    #endregion
    #region ÇÐ°úÀü°ø ÀÎÁõ ÆÄÆ®
    public string pythonDirectory;
    public Text cardCheckText;
    public Button xCardButton;
    public void OnCardButton()
    {
        CardUP.SetActive(true);
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
    public void OpeinFileExplorer()
    {
        path = WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
        //path = EditorUtility.OpenFilePanel("Show all images(.jpg)", "", "jpg");
        StartCoroutine(GetTexture());
    }
    public void XCardButton()
    {
        CardUP.SetActive(false);
    }
    IEnumerator GetTexture()
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
            rawImage.texture = myTexture;
            singUpCheck[4] = true;
            SelectImage.gameObject.SetActive(true);
            byte[] tex = www.downloadHandler.data;
            byte[] textuerData = convertedTexture.EncodeToJPG();
            StartCoroutine(WebRequest_Card_AI(tex));
            //if (Directory.Exists(pythonDirectory) == false)
            //{
            //    Directory.CreateDirectory(pythonDirectory);
            //}
            //File.WriteAllBytes(pythonDirectory + "/cardImage.jpg", textuerData);
        }
    }
    IEnumerator WebRequest_Card_AI(byte[] path)
    {
        MemberCardImage data = new MemberCardImage();
        data.file = path;
        string jsonData = JsonUtility.ToJson(data);
        WWWForm wWWForm = new WWWForm();
        wWWForm.AddBinaryData("file", path);
        //wWWForm.AddField("file", jsonData, Encoding.UTF8);
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormFileSection("file", path, "id_card.jpg", "image/jpg"));
        Debug.Log(GameManager.instance.url + cardURL);
        using (UnityWebRequest request = UnityWebRequest.Post(GameManager.instance.url + cardURL, wWWForm))
        {
            Debug.Log("Ä«µå º¸³¿");
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                string result = request.downloadHandler.text;
                AICardWhole aICard = JsonUtility.FromJson<AICardWhole>(result);
                GameManager.instance.userinfo.departmentName = aICard.data;
                ProtoTest();
            }
            request.Dispose();
        }
    }
    IEnumerator WebRequest_Card(byte[] path)
    {
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormFileSection("file", path, "id_card.jpg", "image/jpg"));
        using (UnityWebRequest request = UnityWebRequest.Post(GameManager.instance.url + cardURL, form))
        {
            //byte[] b = Encoding.UTF8.GetBytes(jsonData);
            //request.uploadHandler = new UploadHandlerRaw(b);

            Debug.Log(cardURL);
            request.SetRequestHeader("Content-Type", "multipart/form-data");
            request.uploadHandler = new UploadHandlerRaw(path);
            //request.SetRequestHeader("Content-Type", "application/json");
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                string result = request.downloadHandler.text;
                Debug.Log(result);
                Invoke("ProtoTest", 2f);
            }
            request.Dispose();
        }
    }
    void ProtoTest() //ÇÁ·ÎÅä ¿ë ÀÓ½Ã ÇÔ¼ö
    {
        cardCheckText.text = GameManager.instance.userinfo.departmentName + "ÇÐ°ú·Î\nÀÎÁõµÇ¼Ì½À´Ï´Ù";
    }

    IEnumerator GetString()
    {
        FileInfo fileInfo = new FileInfo(pythonDirectory +"/cardData.txt");
        string value = "";
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (fileInfo.Exists)
            {
                StreamReader reader = new StreamReader(pythonDirectory + "/cardData.txt");
                value = reader.ReadToEnd();
                reader.Close();
                cardCheckText.text = value + "°ú·Î ÀÎÁõµÇ¼Ì½À´Ï´Ù";
                xCardButton.interactable = true;
                break;
            }
        }
    }
    #endregion
    #region °¡ÀÔ ÆÄÆ®
    public void OnSignButton()
    {
        StartCoroutine(WebRequest_SignUp());
        signUpImage.gameObject.SetActive(false);
    }
    #endregion
    

    public void OnJoin()
    {
        string jsonData = JsonUtility.ToJson(info, true);
    }
    public void OnBack()
    {
        SceneManager.LoadScene("LoginScene_L");
    }
    #region È¸¿ø°¡ÀÔ
    public IEnumerator WebRequest_SignUp()
    {
        Member form = new Member();
        form.memberEmail = idInputfield.text;
        form.memberMajor = GameManager.instance.userinfo.departmentName;
        form.memberName = nameInputfield.text;
        form.memberPassword = passwordInputfield.text;
        form.memberPhone = phoneNumInputfield.text;
        Debug.Log(form.memberMajor);
        string mem = JsonUtility.ToJson(form);
        using (UnityWebRequest request = UnityWebRequest.Post(GameManager.instance.url + singUpURL, mem))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(mem);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("Content-Type", "application/json");
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            string result = request.downloadHandler.text;
            request.Dispose();
        }
    }
    #endregion
}