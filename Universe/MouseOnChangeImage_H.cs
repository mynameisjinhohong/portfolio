using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseOnChangeImage_H : MonoBehaviour
{
    Image myImage;
    public Sprite[] sprite;
    public bool ImOn;
    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<Image>();
    }
    public void OnMouse()
    {
        myImage.sprite = sprite[1];
        ImOn = true;
    }
    public void OffMouse()
    {
        myImage.sprite = sprite[0];
        ImOn = false;
    }
}
