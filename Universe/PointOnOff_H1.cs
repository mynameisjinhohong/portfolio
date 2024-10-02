using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PointOnOff_H1 : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler
{
    public Room_H1 room_H;
    GameObject mouseImage;
    private void Start()
    {
        mouseImage = GameObject.Find("PanelCanvas").transform.GetChild(3).gameObject;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseImage.SetActive(true);
        Debug.Log(room_H.roomInfoText);
        mouseImage.GetComponent<FollowMouseUI_H>().introString = room_H.roomInfoText;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseImage.SetActive(false);
    }
}
