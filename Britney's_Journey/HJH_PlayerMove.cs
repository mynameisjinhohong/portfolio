using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HJH_PlayerMove : MonoBehaviour
{
    public static HJH_PlayerMove instance;

    public AudioClip[] ac;

    AudioSource al;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public enum State
    {
        Running,
        Jump,
        Jumping,
        Slid,
        Sliding,
        Stun,
        Stuning,
    }
    public State state; 
    public float jumpPower = 1f;
    Animator anim;
    Rigidbody2D rd;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        al = Camera.main.GetComponent<AudioSource>();
    }

    public void Jump()
    {
        if (state == State.Running)
        {
            anim.SetTrigger("Jump");
            state = State.Jump;
        }
    }
    bool jumping = false;
    public float mapSpeed;
    public float slidingTime;
    public float stunTime;
    public float slidingSlow = 5f;
    float currentTime;
    void GoJumpingTrue()
    {
        jumping = true;
    }
    void Update()
    {
        #region 게임메니저에서 데이터 받아와서 상태 변환
        if(UDPReceive.instance.dt.updown == 1)
        {
            Jump();
        }
        else if(UDPReceive.instance.dt.updown == -1)
        {
            Slid();
        }
        #endregion
        anim.SetFloat("AnimationSpeed",mapSpeed);
        if (state == State.Jump)
        {
            rd.AddForce(Vector2.up * jumpPower);
            state = State.Jumping;
            Invoke("GoJumpingTrue", 0.1f);
            mapSpeed = 1.5f;
        }
        else if(state == State.Jumping)
        {

        }
        else if(state == State.Running)
        {
            if (UDPReceive.instance.dtList.dataList.Count > 1)
            {
                gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                mapSpeed = UDPReceive.instance.CalculateDegree() * 0.5f;
            }
        }
        else if(state == State.Slid)
        {   
            gameObject.transform.eulerAngles = new Vector3(0, 0, 90);
            gameObject.transform.position = new Vector3(-6.48000002f, -3.28999996f, 0);
            gameObject.transform.GetChild(0).transform.eulerAngles = new Vector3(0, 0, 0);
            anim.SetTrigger("Slide");
            state = State.Sliding;
        }
        else if(state == State.Sliding)
        {
            if(mapSpeed > 1.5)
            {
                mapSpeed -= Time.deltaTime * slidingSlow;
            }
            else
            {
                mapSpeed = 1.5f;
            }
            
            if(mapSpeed < 0)
            {
                mapSpeed = 0;
            }
            if (UDPReceive.instance.dt.updown == 0 || UDPReceive.instance.dt.updown == 1)
            {
                state = State.Running;
                gameObject.transform.position = new Vector3(-6.48000002f, -2.3599999f, 0);
                gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                gameObject.transform.GetChild(0).transform.eulerAngles = new Vector3(0, 0, 0);
                anim.SetTrigger("SlideOver");
            }
        }
        else if(state == State.Stun)
        {
            anim.SetTrigger("Stun");
            StopAllCoroutines();
            StartCoroutine(stun());
            state = State.Stuning;
        }
        else if(state == State.Stuning)
        {
            currentTime += Time.deltaTime;
            if (currentTime > stunTime)
            {
                state = State.Running;
                currentTime = 0;
            }
        }
        
    }

    public void Slid()
    {
        if (state == State.Running)
        {
            state = State.Slid;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(state == State.Jumping && jumping == true && collision.gameObject.tag == "ground")
        {
            state = State.Running;
            jumping = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (collision.gameObject.tag == "enemy")
        {
            jumping = false;
            mapSpeed = 0;
            state = State.Stun;
            al.clip = ac[UnityEngine.Random.Range(0, 2)];
            al.Play();
        }
    }
    public float stunSpeed = 2f;
    IEnumerator stun()
    {
        
        float time = 0;
        float stunTime = 0.05f;
        while (true)
        {
            time += Time.deltaTime;
            //gameObject.transform.position += Vector3.left * stunSpeed * Time.deltaTime;
            mapSpeed = stunSpeed * -1;
            if(time > stunTime)
            {
                mapSpeed = 0;
                yield break;
            }

            yield return null;
        }
    }
}
