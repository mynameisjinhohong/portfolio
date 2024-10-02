using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressFPortal_H : MonoBehaviour
{
    public enum PortalNum
    {
        Portal1,
        Portal2
    }

    public PortalNum portalNum;

    CharacterMove_H player;
    public Transform movePortal;
    public SpriteRenderer sr;

    public List<Sprite> portal1Sprites; //0=default, 1=light
    public List<Sprite> portal2Sprites; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(onPlayer == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                player.gameObject.transform.position = movePortal.position;
                player = null;
                onPlayer = false;
            }
        }
    }
    bool onPlayer;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            CharacterMove_H character = collision.GetComponent<CharacterMove_H>();
            player = character;
            character.pressFKey.SetActive(true);
            character.objInfo = transform.parent.GetComponent<ObjectInfo_H>();
            onPlayer = true;

            if(portalNum == PortalNum.Portal1)
            {
                sr.sprite = portal1Sprites[1];
            }
            else if(portalNum == PortalNum.Portal2)
            {
                sr.sprite = portal2Sprites[1];
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CharacterMove_H character = collision.GetComponent<CharacterMove_H>();
            player = null;
            character.pressFKey.SetActive(false);
            onPlayer = false;

            if (portalNum == PortalNum.Portal1)
            {
                sr.sprite = portal1Sprites[0];
            }
            else if (portalNum == PortalNum.Portal2)
            {
                sr.sprite = portal2Sprites[0];
            }
        }
    }
}
