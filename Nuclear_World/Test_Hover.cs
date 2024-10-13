using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test_Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // public Animator Character_anim;
    public GameObject hover;
    [TextArea]
    public string hoverText;

    void Start()
    {
        hover = GameObject.Find("Hover");
        //Character_anim.SetBool("Button_Size", false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover.SetActive(true);
        hover.GetComponent<RectTransform>().position = new Vector3(this.transform.position.x, this.transform.position.y + 150, this.transform.position.z);
        hover.GetComponentInChildren<TMP_Text>().text = hoverText;
        string[] strings = hoverText.Split("\n");
        int textLength = hoverText.Split("\n")[0].Length;
        for(int i =0; i< strings.Length; i++)
        {
            if (textLength < strings[i].Length)
            {
                textLength = strings[i].Length;
            }
        }
        hover.GetComponent<RectTransform>().sizeDelta = new Vector2(textLength * 30, 200);
        // Character_anim.SetBool("Button_Size", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover.SetActive(false);
        //Character_anim.SetBool("Button_Size", false);


    }
}
