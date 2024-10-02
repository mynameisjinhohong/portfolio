using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Photon.Pun;

[System.Serializable]
public class UserPortfolioWhole
{
    public int status;
    public string message;
    public List<UserPortfolioClass> data;

    public UserPortfolioWhole()
    {
        data = new List<UserPortfolioClass>();
    }
}

[System.Serializable]
public class UserPortfolioClass
{
    public int portfolioCode;
    public int memberCode;
    public int portfolioIndex;
    public string portfolioName;
    public string portfolioLink;
}

[System.Serializable]
public class UserBasicInfo
{
    public string memberPassword;
    public string memberPhone;
    public string memberNickName;
}
[System.Serializable]
public class URLInfo
{
    public int portfolioIndex;
    public string portfolioName;
    public string portfolioLink;
}

public class ItemInfo
{
    public int itemHairCode;
    public int itemFaceCode;
    public int itemClothCode;
}
[System.Serializable]
public class UserMajorInfo
{
    public string majorConcentration;
    public string majorKeyword;
    public string majorSpecialty;
    public string majorHopePath;
}
public class NickName
{
    public string memberNickName;
}
public class MyInfoManager_H : MonoBehaviourPun
{
    public string nickNameURL = "/auths/nickname";
    public string withDrawURL = "/members/delete";
    public string saveBasicInfoURL = "/members/update";
    public string saveMajorInfoURL = "/members/major/update";
    public string setItemURL = "/members/item/update";
    public string setPortfolioURL = "/portfolios/update";
    public string getPortfolioURL = "/portfolios";

    public GameObject characterEditorImgae;
    public GameObject myFavoriteImgae;
    public GameObject portfolioLink;
    public GameObject portfolioPop;
    public Sprite favoriteClick;
    public List<FavoriteInfoButton_H> favoriteInfoButtons;
    public Button[] urlButtons;
    public InputField[] urlInputFields;
    public InputField[] urlNameFields;
    public URLButton_H[] urls;

    [Header("회원 기본 정보(전화번호,닉네임,비밀번호 수정)")]
    public InputField withDrawInputField;
    public Button withDrawCheckButton;
    public GameObject withDrawParent;
    public GameObject[] withDrawImages;
    public GameObject[] canUsecantUse;
    public InputField phoneNumInputField;
    public InputField nickNameInputField;
    public InputField passWardInputField;
    public Text nickName;
    public Text phoneNum;

    [Header("회원 전공 정보(세부전공,관심키워드,Specialty,희망분야)")]
    public InputField concentrationInputField;
    public InputField keywordInputField;
    public InputField specialtyInputField;
    public InputField hopepathInputField;

    public Text majorText;
    public Text concentrationText;
    public Text keywordText;
    public Text specialtyText;
    public Text hopepathText;
    public Text capText;
    public Text cherryText;
    public Text nameText;
    public Text idText;
    public List<URLButton_H> portfolioList;

    public Button saveButton;
    // Start is called before the first frame update
    void Start()
    {
        for(int i =0; i < myFavoriteImgae.transform.childCount; i++)
        {
            favoriteInfoButtons.Add(myFavoriteImgae.transform.GetChild(0).GetComponent<FavoriteInfoButton_H>());
        }
        withDrawInputField.onValueChanged.AddListener(WithDrawInputFieldValueChanged);
        majorText.text = GameManager.instance.userinfo.departmentName;
        concentrationText.text = GameManager.instance.userinfo.majorConcentration;
        keywordText.text = GameManager.instance.userinfo.majorKeyword;
        specialtyText.text = GameManager.instance.userinfo.majorSpecialty;
        hopepathText.text = GameManager.instance.userinfo.majorHopePath;
        capText.text = GameManager.instance.userinfo.memberCap.ToString();
        cherryText.text = GameManager.instance.userinfo.memberCherry.ToString();
        nameText.text = GameManager.instance.userinfo.name;
        idText.text = GameManager.instance.userinfo.id;
        for(int i =0; i<5; i++)
        {
            urls[i].name = GameManager.instance.userinfo.URLName[i];
            urls[i].url = GameManager.instance.userinfo.URLs[i];
        }
        StartCoroutine(WebRequestGetPortfolio(GameManager.instance.userinfo.token));
    }
    private void Update()
    {
        saveButton.interactable = !canUsecantUse[1].activeInHierarchy;
    }

    public IEnumerator WebRequestGetPortfolio(string userToken)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(GameManager.instance.url + getPortfolioURL))
        {
            //request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", userToken);
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("포트폴리오 받아오기 : "+System.Text.Encoding.Default.GetString(request.downloadHandler.data));
                UserPortfolioWhole userPortfolioWhole = JsonUtility.FromJson<UserPortfolioWhole>(System.Text.Encoding.Default.GetString(request.downloadHandler.data));
                //print("This User Avatar is " + System.Text.Encoding.Default.GetString(request.downloadHandler.data));
                for(int i = 0; i < userPortfolioWhole.data.Count; i++)
                {
                    print("포폴 링크 : "+userPortfolioWhole.data[i].portfolioLink);
                    switch (userPortfolioWhole.data[i].portfolioIndex)
                    {
                        case 0:
                            portfolioList[0].url = userPortfolioWhole.data[i].portfolioLink;
                            portfolioList[0].name = userPortfolioWhole.data[i].portfolioName;
                            break;
                        case 1:
                            portfolioList[1].url = userPortfolioWhole.data[i].portfolioLink;
                            portfolioList[1].name = userPortfolioWhole.data[i].portfolioName;
                            break;
                        case 2:
                            portfolioList[2].url = userPortfolioWhole.data[i].portfolioLink;
                            portfolioList[2].name = userPortfolioWhole.data[i].portfolioName;
                            break;
                        case 3:
                            portfolioList[3].url = userPortfolioWhole.data[i].portfolioLink;
                            portfolioList[3].name = userPortfolioWhole.data[i].portfolioName;
                            break;
                        case 4:
                            portfolioList[4].url = userPortfolioWhole.data[i].portfolioLink;
                            portfolioList[4].name = userPortfolioWhole.data[i].portfolioName;
                            break;

                    }
                }
            }
            request.Dispose();
        }
    }
    void WithDrawInputFieldValueChanged(string s)
    {
        if(s == GameManager.instance.userinfo.password)
        {
            withDrawCheckButton.interactable = true;
        }
    }
    public void EditAvatar()
    {
        characterEditorImgae.SetActive(true);
    }
    public void XButton()
    {
        characterEditorImgae.SetActive(false);
    }
    public void SaveButton()
    {
        characterEditorImgae.SetActive(false);
        StartCoroutine(WebRequest_ItemSave());
    }
    public IEnumerator WebRequest_ItemSave()
    {
        ItemInfo itemInfo = new ItemInfo();
        itemInfo.itemHairCode = GameManager.instance.userinfo.avatarSet.head;
        itemInfo.itemFaceCode = GameManager.instance.userinfo.avatarSet.face;
        itemInfo.itemClothCode = GameManager.instance.userinfo.avatarSet.body;
        string info = JsonUtility.ToJson(itemInfo);
        using (UnityWebRequest request = UnityWebRequest.Put(GameManager.instance.url + setItemURL,info))
        {
            request.SetRequestHeader("Authorization", GameManager.instance.userinfo.token);
            request.SetRequestHeader("Content-Type", "application/json");
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("아이템 됨!");
            }
            request.Dispose();
        }
    }
    public void LogOut()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("LoginScene_L");
    }

    public void MyFavoriteDown()
    {
        myFavoriteImgae.SetActive(true);
    }
    public void MyFavoriteUP()
    {
        myFavoriteImgae.SetActive(false);
    }
    public void SaveURLButton()
    {
        for(int i =0; i < 5; i++)
        {
            urls[i].url = urlInputFields[i].text;
            urls[i].name = urlNameFields[i].text;
            if(urls[i].url.Length >0 && urls[i].name.Length > 0)
            {
                StartCoroutine(WebRequest_URLSave(i,urls[i].name,urls[i].url));
            }
            GameManager.instance.userinfo.URLName[i] = urls[i].name;
            GameManager.instance.userinfo.URLs[i] = urls[i].url;
        }
        portfolioLink.SetActive(false);
    }
    public IEnumerator WebRequest_URLSave(int idx,string name,string link)
    {
        Debug.Log(idx +"      " + name +"         " +link);
        URLInfo urlInfo = new URLInfo();
        urlInfo.portfolioIndex = idx;
        urlInfo.portfolioName = name;
        urlInfo.portfolioLink = link;
        string info = JsonUtility.ToJson(urlInfo);
        using (UnityWebRequest request = UnityWebRequest.Put(GameManager.instance.url + setPortfolioURL,info))
        {
            request.SetRequestHeader("Authorization", GameManager.instance.userinfo.token);
            request.SetRequestHeader("Content-Type", "application/json");
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("포트폴리오 됨!");
                StartCoroutine(WebRequestGetPortfolio(GameManager.instance.userinfo.token));

            }
            request.Dispose();
        }
    }
    public IEnumerator WebRequest_URLPost(int idx)
    {
        URLInfo urlInfo = new URLInfo();
        urlInfo.portfolioIndex = idx;
        urlInfo.portfolioLink = "";
        urlInfo.portfolioName = "";
        string info = JsonUtility.ToJson(urlInfo);
        using (UnityWebRequest request = UnityWebRequest.Post(GameManager.instance.url + setItemURL, info))
        {
            request.SetRequestHeader("Authorization", GameManager.instance.userinfo.token);
            request.SetRequestHeader("Content-Type", "application/json");
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.data);
            }
            request.Dispose();
        }
    }
    public void URLButton()
    {
        portfolioLink.SetActive(true);
    }
    public void PopButton()
    {
        portfolioPop.SetActive(true);
    }
    public void XButtonURL()
    {
        portfolioLink.SetActive(false);
    }
    public void XButtonPop()
    {
        portfolioPop.SetActive(false);
    }
    #region 회원탈퇴
    public void RollBackWithDrawButton()
    {
        for(int i =0; i< withDrawImages.Length; i++)
        {
            withDrawImages[i].SetActive(false);
        }
        withDrawInputField.text = "";
        withDrawCheckButton.interactable = false;
        withDrawImages[0].SetActive(true);
        withDrawParent.SetActive(false);
    }

    public void WithDraw()
    {
        StartCoroutine(WebRequest_WithDraw());
    }
    public IEnumerator WebRequest_WithDraw()
    {

        using (UnityWebRequest request = UnityWebRequest.Delete(GameManager.instance.url + withDrawURL))
        {
            request.SetRequestHeader("Authorization",GameManager.instance.userinfo.token );
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                //string result = request.downloadHandler.text;
                SceneManager.LoadScene("LoginScene_L");
            }
            request.Dispose();
        }
    }
    #endregion
    #region 닉네임 중복 체크
    public void NickNameCheckButton()
    {
        StartCoroutine(WebRequest_NickNameCheck());
    }
    public IEnumerator WebRequest_NickNameCheck()
    {
        NickName nickName = new NickName();
        nickName.memberNickName = nickNameInputField.text;
        List<IMultipartFormSection> data = new List<IMultipartFormSection>();
        data.Add(new MultipartFormDataSection("memberNickName", nickNameInputField.text));
        Debug.Log(GameManager.instance.url + nickNameURL);
        string form = JsonUtility.ToJson(nickName);
        using (UnityWebRequest request = UnityWebRequest.Post(GameManager.instance.url + nickNameURL, form))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(form);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                canUsecantUse[1].SetActive(true);
                canUsecantUse[0].SetActive(false);
                Debug.Log(request.error);
            }
            else
            {
                canUsecantUse[1].SetActive(false);
                canUsecantUse[0].SetActive(true);
            }
            Debug.Log(request.result);
            request.Dispose();
        }
    }
    #endregion
    public void SaveInfoButton()
    {
        StartCoroutine(WebRequest_InfoSave());
        StartCoroutine(WebRequest_MajorUpdate());
    }
    #region 닉네임,전화번호,비밀번호 저장
    public IEnumerator WebRequest_InfoSave()
    {
        UserBasicInfo forms = new UserBasicInfo();
        if(nickNameInputField.text.Length > 0)
        {
            forms.memberNickName = nickNameInputField.text;
        }
        else
        {
            forms.memberNickName = GameManager.instance.userinfo.nickname;
        }
        if (passWardInputField.text.Length > 0)
        {
            forms.memberPassword = passWardInputField.text;
        }
        else
        {
            forms.memberPassword = GameManager.instance.userinfo.password;
        }
        if (phoneNumInputField.text.Length > 0)
        {
            forms.memberPhone = phoneNumInputField.text;
        }
        else
        {
            forms.memberPhone = GameManager.instance.userinfo.phoneNumber;
        }
        string form = JsonUtility.ToJson(forms);
        Debug.Log(GameManager.instance.url + saveBasicInfoURL);
        Debug.Log(form);
        using (UnityWebRequest request = UnityWebRequest.Put(GameManager.instance.url + saveBasicInfoURL, form))
        {
            request.SetRequestHeader("Authorization", GameManager.instance.userinfo.token);
            request.SetRequestHeader("Content-Type", "application/json");
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                canUsecantUse[1].SetActive(false);
                canUsecantUse[0].SetActive(false);
                nickName.text = forms.memberNickName;
                phoneNum.text = forms.memberPhone;
                GameManager.instance.userinfo.password = forms.memberPassword;
                GameManager.instance.userinfo.nickname = forms.memberNickName;
                PhotonNetwork.NickName = forms.memberNickName;
                GameManager.instance.userinfo.phoneNumber = forms.memberPhone;
            }
            request.Dispose();
        }
    }
    #endregion
    public IEnumerator WebRequest_MajorUpdate()
    {
        UserMajorInfo forms = new UserMajorInfo();
        if (concentrationInputField.text.Length > 0)
        {
            forms.majorConcentration = concentrationInputField.text;
        }
        else
        {
            forms.majorConcentration = GameManager.instance.userinfo.majorConcentration;
        }
        if (hopepathInputField.text.Length > 0)
        {
            forms.majorHopePath = hopepathInputField.text;
        }
        else
        {
            forms.majorHopePath = GameManager.instance.userinfo.majorHopePath;
        }
        if (keywordInputField.text.Length > 0)
        {
            forms.majorKeyword = keywordInputField.text;
        }
        else
        {
            forms.majorKeyword = GameManager.instance.userinfo.majorKeyword;
        }
        if (specialtyInputField.text.Length >0)
        {
            forms.majorSpecialty = specialtyInputField.text;
        }
        else
        {
            forms.majorSpecialty = GameManager.instance.userinfo.majorSpecialty;
        }
        string form = JsonUtility.ToJson(forms);
        using (UnityWebRequest request = UnityWebRequest.Put(GameManager.instance.url + saveMajorInfoURL, form))
        {
            request.SetRequestHeader("Authorization", GameManager.instance.userinfo.token);
            request.SetRequestHeader("Content-Type", "application/json");
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Success");
                GameManager.instance.userinfo.majorConcentration = concentrationInputField.text;
                concentrationText.text = GameManager.instance.userinfo.majorConcentration;
                GameManager.instance.userinfo.majorHopePath = forms.majorHopePath;
                hopepathText.text = forms.majorHopePath;
                GameManager.instance.userinfo.majorKeyword = forms.majorKeyword;
                keywordText.text = forms.majorKeyword;
                GameManager.instance.userinfo.majorSpecialty = forms.majorSpecialty;
                specialtyText.text = forms.majorSpecialty;
            }
            request.Dispose();
        }
    }

}
