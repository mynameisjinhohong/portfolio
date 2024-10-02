using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FavoriteInfoButton_H : MonoBehaviour
{
    public bool IsMyFavorite = false;
    public Sprite[] sprites;
    Button myButton;
    Image myImage;
    int myidx;
    private void Start()
    {
        myImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        myidx = int.Parse(gameObject.name.Replace("Button (Legacy) (", "").Replace(")", ""));
    }
    // Start is called before the first frame update
    public void OnClickFavoriteButton()
    {
        IsMyFavorite = !IsMyFavorite;
        if(IsMyFavorite == true)
        {
            myImage.sprite = sprites[1];

        }
        else
        {
            myImage.sprite = sprites[0];
        }
    }
}
