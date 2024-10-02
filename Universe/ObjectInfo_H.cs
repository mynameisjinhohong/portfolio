using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ObjectInfo_H : MonoBehaviour
{
    MapEditor_L mapEditor;
    public ObjectInfo objectInfo;
    public float textureSize = 0.007f;
    public BoxCollider2D bc;
    public BoxCollider2D thisBc;
    public GameObject btnSetting;
    GameObject speechBubble;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MapEditorScene_H")
            mapEditor = GameObject.Find("MapEditor").GetComponent<MapEditor_L>();
        if (mapEditor)
        {
            if (mapEditor.placementType == MapEditor_L.PlacementType.UpperObject || mapEditor.placementType == MapEditor_L.PlacementType.Object)
            {
                btnSetting.SetActive(true);
            }
        }

    }
    bool setTexture = false;
    // Update is called once per frame
    void Update()
    {
/*        if(mapEditor.placementType==MapEditor_L.PlacementType.UpperObject || mapEditor.placementType == MapEditor_L.PlacementType.Object)
        {
            btnSetting.SetActive(true);
        }
        else
        {
            btnSetting.SetActive(false);
        }*/

        if(objectInfo.upperObj == true)
        {
            GetComponent<SortingGroup>().sortingLayerName = "UpperObject";
        }
        else if(objectInfo.upperObj == false)
        {
            GetComponent<SortingGroup>().sortingLayerName = "Object";
        }
        if(objectInfo.objSkill == ObjectInfo.ObjectSkill.talkingObj)
        {
            speechBubble = transform.Find("SpeechCanvas").GetChild(0).gameObject;
        }
        if(objectInfo.image.Length > 0 && setTexture == false)
        {
            setTexture = true;

            

            #region 주석처리 원래 잘 되던것
            Texture2D texture = new Texture2D(objectInfo.objWidth,objectInfo.objHeight);
            //Texture2D texture = new Texture2D((int)spriteX, (int)spriteY);

            texture.LoadImage(objectInfo.image);
            Rect rect = new Rect(0, 0, texture.width, texture.height);

            GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width * textureSize, texture.height * textureSize);
            btnSetting.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x * objectInfo.objectSize.x*0.5f,
               GetComponent<RectTransform>().sizeDelta.y * objectInfo.objectSize.y * 0.5f);
            btnSetting.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
            
            /*btnSetting.GetComponent<RectTransform>().position = new Vector3(GetComponent<RectTransform>().position.x - (GetComponent<RectTransform>().sizeDelta.x / 2),
                GetComponent<RectTransform>().position.y + (GetComponent<RectTransform>().sizeDelta.y / 2), GetComponent<RectTransform>().position.z);*/
            bc.size = new Vector2(texture.width * textureSize, texture.height * textureSize);
            thisBc.size = new Vector2(texture.width * textureSize, texture.height * textureSize);
            GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
            #endregion
            float spriteX = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            float spriteY = GetComponent<SpriteRenderer>().sprite.bounds.size.y;
            print("sprite size : " + spriteX.ToString() + " , " + spriteY.ToString());

            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * objectInfo.objectSize.x,
                gameObject.transform.localScale.y * objectInfo.objectSize.y,
                gameObject.transform.localScale.z);

            //GetComponent<MeshRenderer>().material.SetTexture("_MainTex", (Texture)texture);
        }
    }

    public void ChangeObjectSize(int x, int y)
    {
        gameObject.transform.localScale = new Vector3(x*0.5f,
                y*0.5f,
                gameObject.transform.localScale.z);

        btnSetting.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x * objectInfo.objectSize.x * 0.5f,
               GetComponent<RectTransform>().sizeDelta.y * objectInfo.objectSize.y * 0.5f);
    }

    public void OnClickBtnObjSetting()
    {
        mapEditor.currObjectInfo = EventSystem.current.currentSelectedGameObject.transform.parent.parent.GetChild(0).GetComponent<ObjectInfo_H>().objectInfo;
        mapEditor.currObjectInfo_H = EventSystem.current.currentSelectedGameObject.transform.parent.parent.GetChild(0).GetComponent<ObjectInfo_H>();
        mapEditor.panelObjectSetting.SetActive(true);
    }

    public void OnPlayerCall()
    {
        if(objectInfo.objSkill == ObjectInfo.ObjectSkill.urlObj)
        {
            UrlObj();
        }
        else if (objectInfo.objSkill == ObjectInfo.ObjectSkill.changeObj)
        {
            ChangeObj();
        }
        else if(objectInfo.objSkill == ObjectInfo.ObjectSkill.talkingObj)
        {
            SpeechObj();
        }
    }
    void SpeechObj()
    {
        speechBubble.SetActive(true);
        speechBubble.transform.GetChild(0).GetComponent<Text>().text = objectInfo.talkingSkill;
    }
    void ChangeObj()
    {
        Texture2D objTexture = new Texture2D(objectInfo.objWidth,objectInfo.objHeight);
        objTexture.LoadImage(objectInfo.imageSkill);

        Rect rect = new Rect(0, 0, objTexture.width, objTexture.height);

        GetComponent<RectTransform>().sizeDelta = new Vector2(objTexture.width * textureSize, objTexture.height * textureSize);

        GetComponent<SpriteRenderer>().sprite = Sprite.Create(objTexture, rect, new Vector2(0.5f, 0.5f));

        //gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", objTexture);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(objectInfo.interactionType == ObjectInfo.InteractionType.touch)
        {
            OnPlayerCall();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (objectInfo.interactionType == ObjectInfo.InteractionType.touch)
            {
                OnPlayerCall();
            }
        }
    }
    void UrlObj()
    {
        Application.OpenURL(objectInfo.urlSkill);
    }
    
}
