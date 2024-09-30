using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerMove_HJH : MonoBehaviourPun
{
    public GameObject smoke;
    public float jumpPower = 5;
    protected GameObject can;
    public float gravity = -9.8f;
    [SerializeField] 
    protected VariableJoystick joy;
    public float speed;
    public Vector3 moveVec;
    protected CharacterController cc;
    public int jumpCount = 2;
    protected int firstJumpCount;
    public bool keyboardMode = false;
    //점프했는지 체크하게 하기 위해
    protected bool jumpCheckStart = false;
    protected Animator am;
    public GameObject dashEffect;
    public bool Player = false;
    public float upDown = 0;
    protected PlayerHp_HJH hp;
    public AudioClip[] audioClips;
    protected AudioSource audio;

    #region [현숙] 변수
    //****레이어 충돌 변수
    int playerLayer1, playerLayer2, playerLayer3, playerLayer4, groundLayer;
    bool fallGround;
    #endregion

    #region [현숙] 충돌무시함수
    //****충돌무시(열림)
    protected void IgnoreLayerTrue()
    {
        if(this.gameObject.layer == LayerMask.NameToLayer("Player1"))
        {
            Physics.IgnoreLayerCollision(playerLayer1, groundLayer, true);
        }
        else if(this.gameObject.layer == LayerMask.NameToLayer("Player2"))
        {
            Physics.IgnoreLayerCollision(playerLayer2, groundLayer, true);
        }
        else if (this.gameObject.layer == LayerMask.NameToLayer("Player3"))
        {
            Physics.IgnoreLayerCollision(playerLayer3, groundLayer, true);
        }
        else if (this.gameObject.layer == LayerMask.NameToLayer("Player4"))
        {
            Physics.IgnoreLayerCollision(playerLayer4, groundLayer, true);
        }
    }
    #endregion

    #region [현숙] 충돌적용함수
    //****충돌적용(닫힘)
    protected void IgnoreLayerFalse()
    {
        if (this.gameObject.layer == LayerMask.NameToLayer("Player1"))
        {
            Physics.IgnoreLayerCollision(playerLayer1, groundLayer, false);
        }
        else if (this.gameObject.layer == LayerMask.NameToLayer("Player2"))
        {
            Physics.IgnoreLayerCollision(playerLayer2, groundLayer, false);
        }
        else if (this.gameObject.layer == LayerMask.NameToLayer("Player3"))
        {
            Physics.IgnoreLayerCollision(playerLayer3, groundLayer, false);
        }
        else if (this.gameObject.layer == LayerMask.NameToLayer("Player4"))
        {
            Physics.IgnoreLayerCollision(playerLayer4, groundLayer, false);
        }
    }
    #endregion

    #region [현숙] 착지면 떨어질 때 충돌처리
    //****착지면에 떨어지는 키를 눌렀을때 0.2초간 레이어 충돌이 무시된 후 다시 적용
    IEnumerator LayerOpenClose()
    {
        fallGround = true;
        IgnoreLayerTrue();
        yield return new WaitForSeconds(1f);
        IgnoreLayerFalse();
        fallGround = false;
    }
    #endregion

    #region [현숙] Ray변수
    //**** Ray변수
    private RaycastHit hit;
    private int layerMask;
    private int layerMask2;
    public float distance = 3;
    #endregion

    public enum State
    {
        Idle,
        Run,
        Jump,
        Dash,
        Attack,
        Attacked,
        JumpAttack,
    }
    public State state = State.Idle;
    protected GameObject Weapon = null;
        
    // Start is called before the first frame update
    private void Awake()
    {
        gameObject.transform.GetChild(0).GetComponent<Renderer>().sortingOrder = 50;
        if (!photonView.IsMine)
        {
            Outline outline = GetComponent<Outline>();
            outline.OutlineWidth = 0;
        }
        audio = GetComponent<AudioSource>();  
        hp = GetComponent<PlayerHp_HJH>();
        if (photonView.IsMine == true)
        {
            joy = GameObject.Find("Variable Joystick").GetComponent<VariableJoystick>();
        }
#if UNITY_EDITOR
        Debug.Log("??");
        keyboardMode = true;
#elif UNITY_ANDROID
        keyboardMode = false;
        GameObject.Find("Special").GetComponent<Button>().onClick.AddListener(SButton);
        GameObject.Find("Jump").GetComponent<Button>().onClick.AddListener(JButton);
        GameObject.Find("Attack").GetComponent<Button>().onClick.AddListener(AButton);
#elif UNITY_STANDALONE_WIN
        keyboardMode = true;
#endif
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.gameObject.name == weapon)
            {
                Weapon = child.gameObject;
            }
        }
        am = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        can = GameObject.Find("Controller Canvas");
        moveVec = Vector3.zero;
        //am.SetInteger("Jump",jumpCount);
        firstJumpCount = jumpCount;
        GameManager.instance.players[(photonView.ViewID / 1000) - 1] = gameObject;

#region [현숙] LayerMask지정
        //****LayerMask 지정
        playerLayer1 = LayerMask.NameToLayer("Player1");
        playerLayer2 = LayerMask.NameToLayer("Player2");
        playerLayer3 = LayerMask.NameToLayer("Player3");
        playerLayer4 = LayerMask.NameToLayer("Player4");
        groundLayer = LayerMask.NameToLayer("Ground");

        layerMask = 1 << 8;
        layerMask2 = 1 << 9;
#endregion
    }

    public virtual void Start()
    {
        if (photonView.IsMine)
        {
            Player = true;
        }
        else
        {
            Player = false;
        }
    }

    public void ChangeState(State s)
    {
        if (state == s) return;
        state = s;
        switch (s)
        {
            case State.Idle:
                Invoke("JumpCountReturn", 1f);
                moveVec.y = 0;
                am.SetInteger("State", 0);
                break;
            case State.Run:
                am.SetInteger("State", 1);
                break;
            case State.Jump:
                am.SetInteger("State", 2);
                break;
        }

    }

    // Update is called once per frame
    void Update()
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
                if (jumpCheckStart == true && cc.isGrounded)
                {
                    moveVec.y = 0;
                    am.SetInteger("State", 0);
                    jumpCheckStart = false;
                    Invoke("JumpCountReturn", 3f);
                    ChangeState(State.Idle);
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

        }
        else
        {
            if(state == State.Attacked)
            {
                Debug.Log("why");
                StartCoroutine(Stun(hp.Hp));
            }
            if (!cc.isGrounded)
            {
                moveVec.y += gravity * Time.deltaTime;
            }
        }

        cc.Move(moveVec * Time.deltaTime);

    }

    void JumpCountReturn()
    {
        jumpCount = firstJumpCount;
    }

    public IEnumerator Stun(int stunTime)
    {
        am.SetTrigger("Damage");
        yield return new WaitForSeconds((float)stunTime / 300);
        if (cc.isGrounded == true)
        {
            state = State.Jump;
        }
        else
        {
            ChangeState(State.Idle);
        }
    }

    protected void KeyBoardMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        upDown = z;
        moveVec.x = x * speed;
        if (moveVec.x < 0)
        {
            transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else if(moveVec.x == 0)
        {
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        if (!cc.isGrounded)
        {
            moveVec.y += gravity * Time.deltaTime;
           
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            JButton();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            SButton();
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            AButton();
        }

#region [현숙] 점프발판 / 벽점프 
            // 점프 발판
            // 아래로 레이를 쐈을 때 
            if (Physics.Raycast(this.transform.position, -this.transform.up, out hit, 10, layerMask) && !fallGround)
            {
                IgnoreLayerFalse();
            }

            //벽 점프
            if (Physics.Raycast(this.transform.position + new Vector3(0, 1.5f, 0), this.transform.forward, out hit, 0.7f, layerMask2) && moveVec.x != 0)
            {
                Debug.DrawRay(this.transform.position + new Vector3(0, 1.5f, 0), this.transform.forward, Color.green, 0.7f);
                if (moveVec.y < 0)
                {
                    moveVec = Vector3.zero;
                }

                jumpCount = 1;
            }

#endregion

    }

    protected void JoyStickMove()
    {
        float x = joy.Horizontal;
        float z = joy.Vertical;
        upDown = z;
        moveVec.x = x * speed;
        if (moveVec.x < 0)
        {
            transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else if (moveVec.x == 0)
        {
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        if (!cc.isGrounded)
        {
            moveVec.y += gravity * Time.deltaTime;
        }
        #region [현숙] 점프발판 / 벽점프 
        // 점프 발판
        // 아래로 레이를 쐈을 때 
        if (Physics.Raycast(this.transform.position, -this.transform.up, out hit, 10, layerMask) && !fallGround)
        {
            IgnoreLayerFalse();
        }

        //벽 점프
        if (Physics.Raycast(this.transform.position + new Vector3(0, 1.5f, 0), this.transform.forward, out hit, 0.7f, layerMask2) && moveVec.x != 0)
        {
            Debug.DrawRay(this.transform.position + new Vector3(0, 1.5f, 0), this.transform.forward, Color.green, 0.7f);
            if (moveVec.y < 0)
            {
                moveVec = Vector3.zero;
            }

            jumpCount = 1;
        }

        #endregion

    }

    public void JButton()
    {
        if(upDown < 0)
        {
            DownJump();
        }
        else
        {
            Jump();
        }

    }
    public virtual void DownJump()
    {
        moveVec.y = 0;
        StartCoroutine(LayerOpenClose());
    }


    public virtual void Jump()
    {

        //더블 점프 버그있음 왜그런지는 모르겠음

        // [현숙] 점프중이라면 True
        IgnoreLayerTrue();

        if (jumpCount > 0)
        {
            moveVec.y = jumpPower;
            ChangeState(State.Jump);
            jumpCount--;
        }
    }
    public void SButton()
    {
        if (state != State.Idle)
        {
            Dash();
        }
        else
        {
            Skill1();
        }
    }

    public void AButton()
    {
        if (state == State.Idle)
        {
            StopAttack();
        }
        else if (state == State.Run)
        {
            MoveAttack();
        }
        else if (state == State.Jump)
        {
            JumpAttack();
        }
    }

    public float dashRange = 2;
    public virtual void Dash()
    {

    }
    public string weapon;


    public virtual void Skill1()
    {

    }

    public virtual void StopAttack()
    {

    }

    public virtual void MoveAttack()
    {

    }

    public virtual void JumpAttack()
    {

    }


}
