using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAvatar_H : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField]
    int whatAnimNow;
    public int dir;
    public CharacterMove_H characterController;
    public Skins skins;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {


    }
    private void LateUpdate()
    {
        dir = characterController.dir;
        whatAnimNow = characterController.whatAnimNow;
        if (dir == 0)
        {
            if (whatAnimNow == 1)
            {
                spriteRenderer.sprite = skins.backSprites[0];
            }
            else if (whatAnimNow == 2)
            {
                spriteRenderer.sprite = skins.backSprites[1];
            }
            else if (whatAnimNow == 3)
            {
                spriteRenderer.sprite = skins.backSprites[2];
            }
            else if (whatAnimNow == 4)
            {
                spriteRenderer.sprite = skins.backSprites[3];
            }
        }
        else if (dir == 1)
        {
            if (whatAnimNow == 1)
            {
                spriteRenderer.sprite = skins.rightSprites[0];
            }
            else if (whatAnimNow == 2)
            {
                spriteRenderer.sprite = skins.rightSprites[1];
            }
            else if (whatAnimNow == 3)
            {
                spriteRenderer.sprite = skins.rightSprites[2];
            }
            else if (whatAnimNow == 4)
            {
                spriteRenderer.sprite = skins.rightSprites[3];
            }
        }
        else if (dir == 2)
        {
            if (whatAnimNow == 1)
            {
                spriteRenderer.sprite = skins.forwardSprites[0];
            }
            else if (whatAnimNow == 2)
            {
                spriteRenderer.sprite = skins.forwardSprites[1];
            }
            else if (whatAnimNow == 3)
            {
                spriteRenderer.sprite = skins.forwardSprites[2];
            }
            else if (whatAnimNow == 4)
            {
                spriteRenderer.sprite = skins.forwardSprites[3];
            }
        }
        else if (dir == 3)
        {
            if (whatAnimNow == 1)
            {
                spriteRenderer.sprite = skins.leftSprites[0];
            }
            else if (whatAnimNow == 2)
            {
                spriteRenderer.sprite = skins.leftSprites[1];
            }
            else if (whatAnimNow == 3)
            {
                spriteRenderer.sprite = skins.leftSprites[2];
            }
            else if (whatAnimNow == 4)
            {
                spriteRenderer.sprite = skins.leftSprites[3];
            }
        }
    }
}
