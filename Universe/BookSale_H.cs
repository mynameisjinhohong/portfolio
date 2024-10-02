using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookSale_H : MonoBehaviour
{
    public GameObject booksaleObj;
    public SpriteRenderer sr;
    public List<Sprite> drSprites;
    bool CalenderOn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CalenderOn == true && Input.GetKeyDown(KeyCode.F))
        {
           booksaleObj.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CharacterMove_H character = collision.GetComponent<CharacterMove_H>();
            character.pressFKey.SetActive(true);
            sr.sprite = drSprites[1];
            CalenderOn = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CharacterMove_H character = collision.GetComponent<CharacterMove_H>();
            character.pressFKey.SetActive(false);
            sr.sprite = drSprites[0];
            CalenderOn = false;
        }

    }
}
