using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FaceChatCanvas_H : MonoBehaviour
{
    public Transform faceChatContent;
    public Transform middlePannel;
    public GameObject middleObject;
    // Start is called before the first frame update
    void Start()
    {
        middlePannel = GameObject.Find("MiddlePannel").transform;
        faceChatContent = GameObject.Find("FaceChatContent").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (middlePannel.childCount > 0)
        {
            middleObject = middlePannel.GetChild(0).gameObject;
        }
        else
        {
            middleObject = null;
        }
        //if (Input.GetMouseButton(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    RaycastHit hitInfo;

        //    if (Physics.Raycast(ray, out hitInfo))
        //    {
        //        Debug.Log(hitInfo.transform.name);
        //        if(hitInfo.transform.gameObject == gameObject)
        //        {

        //            ClickSurface();
        //        }
        //    }
        //}
    }

    public void ClickSurface()
    {
        if(middleObject == gameObject)
        {
            middleObject.transform.localScale = new Vector3(1, 1, 1);
            middleObject.transform.parent = faceChatContent;
        }
        else if(middleObject == null)
        {
            gameObject.transform.parent = middlePannel;
            middleObject = gameObject;
            middleObject.transform.localPosition = Vector3.zero;
            middleObject.transform.localScale = new Vector3(6, 6, 1);
        }
        else
        {
            middleObject.transform.localScale = new Vector3(1, 1, 1);
            middleObject.transform.parent = faceChatContent;
            middleObject = gameObject;
            gameObject.transform.parent = middlePannel;
            middleObject.transform.localPosition = Vector3.zero;
            middleObject.transform.localScale = new Vector3(6, 6, 1);
        }
    }

}
