using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressFURL_H : MonoBehaviour
{
    CharacterMove_H player;
    //public Transform movePortal;
    public string url;
    public SpriteRenderer sr;

    public List<Sprite> urlPortalSprites;

    bool onPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (onPlayer == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Application.OpenURL(url);
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            CharacterMove_H character = collision.GetComponent<CharacterMove_H>();
            player = character;
            character.pressFKey.SetActive(true);
            character.objInfo = transform.parent.GetComponent<ObjectInfo_H>();
            onPlayer = true;

            sr.sprite = urlPortalSprites[1];
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

            sr.sprite = urlPortalSprites[0];

        }
    }
}
