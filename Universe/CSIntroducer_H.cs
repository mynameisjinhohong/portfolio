using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSIntroducer_H : MonoBehaviour
{
    public float txtRemainSpeed = 1f;
    public float txtNextWait = 5f;
    public Text txt;
    public SpeechBubble_H bubble;
    public string[] whatSay;
    bool istalking = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && istalking == false)
        {
            Userinfo user = collision.GetComponent<CharacterMove_H>().user;
            bubble.speechTime = whatSay.Length * txtRemainSpeed + txtRemainSpeed;
            bubble.gameObject.SetActive(true);
            //txt.text = "�ȳ��ϼ���, " + user.name + "��";
            StartCoroutine(Say());

        }
    }
    IEnumerator Say()
    {
        Debug.Log("ä�� ����");
        istalking = true;
        for (int i =0; i< whatSay.Length; i++)
        {
            bubble.gameObject.SetActive(true);
            txt.text = whatSay[i];
            if(i == whatSay.Length - 1)
            {
                istalking = false;
            }
            yield return new WaitForSeconds(txtRemainSpeed);
            bubble.gameObject.SetActive(false);
            yield return new WaitForSeconds(txtNextWait);
        }
    }
}
