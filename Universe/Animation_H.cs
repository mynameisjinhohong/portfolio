using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_H : MonoBehaviour
{
    public int what; //무슨 프리셋을 사용할지.
    public int dir; //up = 0; right = 1; down = 2; left = 3;
    public int skinNr;
    public CharacterMove_H characterController;
    public Skins[] skins;
    [SerializeField]
    int whatAnimNow;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (gameObject.name.Contains("Head"))
        {
            what = characterController.avatarSet.head;
        }
        else if (gameObject.name.Contains("Body"))
        {
            what = characterController.avatarSet.body;
        }
        else if (gameObject.name.Contains("Face"))
        {
            what = characterController.avatarSet.face;
        }
        spriteRenderer.sprite = skins[what].forwardSprites[0];
    }
    bool set = false;
    // Update is called once per frame
    void Update()
    {
        if(characterController.itemSet == true && set == false)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (gameObject.name.Contains("Head"))
            {
                what = characterController.avatarSet.head;
            }
            else if (gameObject.name.Contains("Body"))
            {
                what = characterController.avatarSet.body;
            }
            else if (gameObject.name.Contains("Face"))
            {
                what = characterController.avatarSet.face;
            }
            spriteRenderer.sprite = skins[what].forwardSprites[0];
            set = true;
        }

    }
    private void LateUpdate()
    {
        dir = characterController.dir;
        whatAnimNow = characterController.whatAnimNow;
        if (dir == 0)
        {
            if (whatAnimNow == 1)
            {
                spriteRenderer.sprite = skins[what].backSprites[0];
            }
            else if(whatAnimNow == 2)
            {
                spriteRenderer.sprite = skins[what].backSprites[1];
            }
            else if (whatAnimNow == 3)
            {
                spriteRenderer.sprite = skins[what].backSprites[2];
            }
            else if (whatAnimNow == 4)
            {
                spriteRenderer.sprite = skins[what].backSprites[3];
            }
        }
        else if (dir == 1)
        {
            if (whatAnimNow == 1)
            {
                spriteRenderer.sprite = skins[what].rightSprites[0];
            }
            else if (whatAnimNow == 2)
            {
                spriteRenderer.sprite = skins[what].rightSprites[1];
            }
            else if (whatAnimNow == 3)
            {
                spriteRenderer.sprite = skins[what].rightSprites[2];
            }
            else if (whatAnimNow == 4)
            {
                spriteRenderer.sprite = skins[what].rightSprites[3];
            }
        }
        else if (dir == 2)
        {
            if (whatAnimNow == 1)
            {
                spriteRenderer.sprite = skins[what].forwardSprites[0];
            }
            else if (whatAnimNow == 2)
            {
                spriteRenderer.sprite = skins[what].forwardSprites[1];
            }
            else if (whatAnimNow == 3)
            {
                spriteRenderer.sprite = skins[what].forwardSprites[2];
            }
            else if (whatAnimNow == 4)
            {
                spriteRenderer.sprite = skins[what].forwardSprites[3];
            }
        }
        else if (dir == 3)
        {
            if (whatAnimNow == 1)
            {
                spriteRenderer.sprite = skins[what].leftSprites[0];
            }
            else if (whatAnimNow == 2)
            {
                spriteRenderer.sprite = skins[what].leftSprites[1];
            }
            else if (whatAnimNow == 3)
            {
                spriteRenderer.sprite = skins[what].leftSprites[2];
            }
            else if (whatAnimNow == 4)
            {
                spriteRenderer.sprite = skins[what].leftSprites[3];
            }
        }
    }

}

[System.Serializable]
public struct Skins
{
    public Sprite[] backSprites;
    public Sprite[] forwardSprites;
    public Sprite[] leftSprites;
    public Sprite[] rightSprites;
}