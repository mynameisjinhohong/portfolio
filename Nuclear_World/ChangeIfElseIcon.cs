using MG_BlocksEngine2.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeIfElseIcon : MonoBehaviour
{
    public Image image;
    public BE2_Dropdown dropdown;
    public Sprite spriteLeft;
    public Sprite spriteRight;
    public Sprite spriteFoward;
    public Sprite spriteBack;

    void OnEnable()
    {
        dropdown = BE2_Dropdown.GetBE2ComponentInChildren(transform);
        dropdown.onValueChanged.AddListener(delegate
        {
            SetIcon();
        });
    }

    void OnDisable()
    {
        dropdown.onValueChanged.RemoveAllListeners();
    }

    void SetIcon()
    {
        string value = dropdown.GetSelectedOptionText();
        if (value == "Left")
        {
            image.sprite = spriteLeft;
        }
        else if (value == "Right")
        {
            image.sprite = spriteRight;
        }
        else if(value == "Front")
        {
            image.sprite = spriteFoward;
        }
        else if(value == "Back")
        {
            image.sprite = spriteBack;
        }
    }
}
