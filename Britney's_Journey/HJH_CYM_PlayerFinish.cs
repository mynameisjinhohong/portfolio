using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HJH_CYM_PlayerFinish : MonoBehaviour
{
    public float curTime = 0;
    public bool call = false;

    public GameObject finishText;

    CapsuleCollider2D ccd;

    // Start is called before the first frame update
    void Start()
    {
        //ccd = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (call == true)
        {
            //ccd.enabled = false;

            curTime += Time.deltaTime;
            if (curTime > 1 && curTime < 3)
            {
                finishText.SetActive(true);
                //transform.position = Vector3.Lerp(transform.position, new Vector3(-6.48f, 2.36f, 0), Time.deltaTime * 3);
            }

            if (curTime > 3 && curTime < 7)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(12f, 2.36f, -0), Time.deltaTime * 5);
            }

            if (curTime > 7)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                curTime = 0;
            }


        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FinishLine")
        {
            HJH_BGMove.Instance.isMove = false;
            call = true;
            GameObject[] g = GameObject.FindGameObjectsWithTag("enemy");
            for (int i = 0; i < g.Length; i++)
            {
                Destroy(g[i].gameObject);
            }
            HJH_RockManager.gg = true;
        }
    }
}
