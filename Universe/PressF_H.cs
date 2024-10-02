using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressF_H : MonoBehaviour
{
    ObjectInfo objinfo;
    public SpriteRenderer sr;
    public Material defaultMat;
    public Material outlineMat;

    // Start is called before the first frame update
    void Start()
    {
        sr = transform.parent.GetComponent<SpriteRenderer>();
        objinfo = gameObject.transform.parent.GetComponent<ObjectInfo_H>().objectInfo;

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void SetOutline(bool outline)
    {
        if (outline)
        {
            sr.material = outlineMat;
        }
        else
        {
            sr.material = defaultMat;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(objinfo == null)
        {
            return;
        }
        if(objinfo.interactionType == ObjectInfo.InteractionType.touch)
        {
            return;
        }
        if(collision.tag == "Player" && objinfo.interactionType == ObjectInfo.InteractionType.pressF)
        {
            CharacterMove_H character = collision.GetComponent<CharacterMove_H>();
            character.pressFKey.SetActive(true);
            print("here");
            UpdateOutline(true);
            character.objInfo = transform.parent.GetComponent<ObjectInfo_H>();
            character.inObj = true;
        }
        if(objinfo.objName.Length > 0)
        {
            transform.GetChild(0).GetComponent<TextMesh>().text = objinfo.objName;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (objinfo.interactionType == ObjectInfo.InteractionType.touch)
        {
            return;
        }
        if (collision.tag == "Player" && objinfo.interactionType == ObjectInfo.InteractionType.pressF)
        {
            CharacterMove_H character = collision.GetComponent<CharacterMove_H>();
            UpdateOutline(false);

            character.pressFKey.SetActive(false);
            character.inObj = false;
        }
        if (objinfo.objName.Length > 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        sr.GetPropertyBlock(mpb);
        print("Update Outline sr name : "+sr.gameObject.name);
        mpb.SetFloat("_OutlineEnabled", outline ? 1f : 0f);
        sr.SetPropertyBlock(mpb);
    }
}
