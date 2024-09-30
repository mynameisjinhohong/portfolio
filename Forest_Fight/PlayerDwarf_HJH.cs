using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDwarf_HJH : PlayerMove_HJH //IPunObservable
{
    public GameObject skillEffect;

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //[현숙]
        //만약에 내것이라면 움직임
        if (photonView.IsMine)
        {
            if (Player == true)
            {
                if (state == State.Idle)
                {
                    if (keyboardMode == true)
                    {
                        KeyBoardMove();
                        can.SetActive(false);
                    }
                    else
                    {
                        JoyStickMove();
                        can.SetActive(true);
                    }
                    if (moveVec.x != 0)
                    {
                        ChangeState(State.Run);
                    }
                }
                else if (state == State.Run)
                {
                    if (keyboardMode == true)
                    {
                        KeyBoardMove();
                        can.SetActive(false);
                    }
                    else
                    {
                        JoyStickMove();
                        can.SetActive(true);
                    }
                    if (moveVec.x == 0)
                    {
                        ChangeState(State.Idle);
                    }
                }
                else if (state == State.Jump)
                {
                    if (keyboardMode == true)
                    {
                        KeyBoardMove();
                        can.SetActive(false);
                    }
                    else
                    {
                        JoyStickMove();
                        can.SetActive(true);
                    }
                    if (!cc.isGrounded)
                    {
                        moveVec.y += gravity * Time.deltaTime;
                        jumpCheckStart = true;

                    }
                    else
                    {
                        moveVec.y = 0;
                    }
                    if (jumpCheckStart == true && cc.isGrounded)
                    {
                        am.SetInteger("State", 0);
                        jumpCheckStart = false;
                        Invoke("JumpCountReturn", 1f);
                        state = State.Idle;
                    }
                }
                else if (state == State.Dash)
                {

                }
                else if (state == State.Attack)
                {

                }
                else if (state == State.Attacked)
                {
                    StartCoroutine(Stun(hp.Hp));
                }
                else if (state == State.JumpAttack)
                {
                    //moveVec.y = 0;
                }

            }
            else
            {
                if (state == State.Attacked)
                {
                    GameObject sm = PhotonNetwork.Instantiate("Smoke",transform.position,Quaternion.identity);
                    sm.transform.position = transform.position;
                    StartCoroutine(Stun(hp.Hp));
                }
                if (!cc.isGrounded)
                {
                    moveVec.y += gravity * Time.deltaTime;
                }
            }
            if (transform.position.z != 0)
            {
                cc.Move(new Vector3(0, 0, -transform.position.z));
            }
            else
            {
                moveVec.z = 0;
            }
            cc.Move(moveVec * Time.deltaTime);
        }
    }

    public override void Skill1()
    {
        am.SetInteger("State", 5);

    }
    public void SkillOver()
    {
        ChangeState(State.Idle);
    }
    public void SkillEffect()
    {
        //[현숙]
        if (photonView.IsMine)
        {
            photonView.RPC("RpcShowSkillEffect", RpcTarget.All);  
        }
        
        state = State.Attack;
    }
    void JumpCountReturn()
    {
        jumpCount = firstJumpCount;
    }
    public override void Dash()
    {
        PhotonNetwork.Instantiate("DashEffect", transform.position, Quaternion.identity);
        StartCoroutine(DashEffect());
    }
    IEnumerator DashEffect()
    {
        if (transform.rotation.y > 0)
        {
            cc.Move(new Vector3(dashRange, 0, 0));
        }
        else
        {
            cc.Move(new Vector3(-dashRange, 0, 0));
        }
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Weapon.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        Weapon.SetActive(true);
    }
    public override void Jump()
    {
        base.Jump();
    }
    public override void JumpAttack()
    {
        am.SetInteger("State", 3);
        Weapon.GetComponent<Weapon2_HJH>().Attack = true;
    }
    public void JumpAttackOver()
    {
        Weapon.GetComponent<Weapon2_HJH>().Attack = false;
    }
    public override void StopAttack()
    {
        am.SetInteger("State", 4);
        photonView.RPC("AtSet", RpcTarget.All, true);
        state = State.Attack;
        audio.clip = audioClips[0];
        audio.Play();
    }
    public void AttackOver()
    {
        ChangeState(State.Idle);
        photonView.RPC("AtSet", RpcTarget.All, false);
    }
    [PunRPC]
    void AtSet(bool set)
    {
        Debug.Log(Weapon.name);
        Weapon.GetComponent<Weapon2_HJH>().Attack = set;
    }
    [PunRPC]
    void RpcShowSkillEffect()
    {
        if (photonView.IsMine)
        {
            audio.clip = audioClips[1];
            audio.Play();

            GameObject skill = PhotonNetwork.Instantiate("Player2SkillEffect", gameObject.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            Destroy(skill, 1f);
            skill.GetComponent<Weapon2_HJH>().Attack = true;
        }
        
    }
}
