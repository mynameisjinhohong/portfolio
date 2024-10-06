using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum Player_State // �÷��̾��� ���� �޸���������, ������������
{
    Run,
    Rolling,
    IceFloor,
}

public class Player_HJH : MonoBehaviour
{
    #region Variable
    Rigidbody2D rigid;
    AudioSource audio;
    SoundManager_HJH soundManager;
    Camera cam;

    public InGame_UI_shj ui;
    public LineRenderer predictLine;
    public GameObject gameOverPanel;
    public GameObject gameClearPanel;
    public GameObject selectPanel;
    [Range(0.0f, 10.0f)]
    public float camera_distance;

    [Range(0.0f, 1.0f)]
    public float timeSlowSpeed;
    public bool timeSlowOnOff;

    [Range(0.0f, 15.0f)]
    public float speed; //�ִ�ӵ�
    [Range(0.0f, 15.0f)]
    public float velocity; //���ӵ�

    [Header("���� ����")]
    [Range(0.0f, 100.0f)]
    public float jump_up_power; //���� ������

    //[Range(0.0f, 15.0f)]
    //public float jump_right_power; //���������� ������
    [Range(0.0f, 2.0f)]
    public float maxJumpPower; //���� ������ �������� �ӵ�

    [Range(0.0f, 2.0f)]
    public float minJumpPower; //���� ������ �������� �ӵ�

    [Range(0.0f, 10.0f)]
    public float charge_speed; //���� ������ �������� �ӵ�
    [Header("������ ����")]
    [Range(0.0f, 10.0f)]
    public float rolling_time; //������ �ð�
    [Range(0.0f, 15.0f)]
    public float rolling_Speed; //������ �ӵ�
    [Range(0.0f, 50.0f)]
    public float rolling_MoveSpeed;
    bool rollStart = false;


    [Range(0.0f, 5.0f)] //������ ũ��
    public float rolling_size;
    float default_size;

    [SerializeField]
    bool jumping = false; //���������� �ƴ��� Ȯ��
    bool floorCheck = true; //���� �ϰ� ��񵿾� �ٴ� üũ ���ϰ�
    bool start = false;
    public float jump_charge = 0.0f; //������ ����
    public GameObject charge_img;
    //public Image charge_img; //���� ������
    //int jump_cnt = 0; //����Ƚ�� 2�������� ���

    public Animator playerAnimator;
    [Header("�浹 ����")]
    //public GameObject hp_List;
    //public int maxHP = 10;
    public BoxCollider2D boxCol;

    int Hp;
    public int hp
    {
        get
        {
            return Hp;
        }
        set
        {
            if (!invincible)
            {
                if (Hp > value)
                {
                    nuckBackBool = true;
                }
                Hp = value;
                if(Hp > GameManager_shj.Getinstance.Save_data.max_hp)
                {
                    Hp = GameManager_shj.Getinstance.Save_data.max_hp;
                }
                if (Hp < 1)
                {
                    if (SceneManager.GetActiveScene().name.Contains("Hidden"))
                    {
                        hiddenGameOverPanel.SetActive(true);
                    }
                    else if (SceneManager.GetActiveScene().name.Contains("Challenge"))
                    {
                        challengGameOverPanel.transform.GetChild(0).GetComponent<Text>().text = "���� �ð� : " + timer.text.text + " ��";
                        challengGameOverPanel.SetActive(true);
                    }
                    else
                    {
                        gameOverPanel.SetActive(true);

                    }
                    Time.timeScale = 0f;
                    soundManager.LifeZeroSoundPlay();
                }
            }
        }
    }
    [Header("�˹�� �����ð� ����")]
    [Range(0.0f, 10.0f)]
    public float invincibleTime = 1f;
    [Range(0.0f, 1000.0f)]
    public float nuckBackPower = 1f;
    public int blinkCount;
    [Range(0.0f, 1f)]
    public float blinkSpeed;
    bool invincible = false;

    [Header("ī�޶� �ӵ�")]
    [Range(0.0f, 10.0f)]
    public float cameraSpeed;//ī�޶� ���ǵ�

    public float stuckCheckTime;
    bool stuckRespawn;
    //public Map_shj map; //���� �ӵ� ������ ����


    public SpriteRenderer playerSprite;

    Player_State player_State;
    public Player_State state { get { return player_State; } set { player_State = value; } }

    [Header("�ӽ÷� ���� �͵�")]
    public bool isFloor = false;
    public float gravity = -9.81f;
    public bool jumpBool = false;
    bool nuckBackBool = false;
    bool nuckBackDuring = false;
    public bool upCrushCheck = false; //���� �ε����� ��
    public int ancientMax; //����� ���� ����
    public int juksunMax;
    Coroutine nuckBackCoroutine;
    #endregion
    float test = 0.0f;
    bool iceFloorRunning = false;
    public float nomalSpeed = 0f;
    public bool gameClear = false;
    float slowTime = 0f;
    Vector2 colSize;
    Vector2 colOffset;
    [Header("����,ç����")]
    public GameObject challengGameOverPanel;
    public GameObject hiddenGameOverPanel;
    public GameObject hiddenGameClearPanel;
    public TImer_shj timer;
    Vector3 currentPos;
    public void Start()
    {
        nomalSpeed = speed;
        colOffset = boxCol.offset;
        colSize = boxCol.size;
        cam = Camera.main;
        soundManager = GetComponentInChildren<SoundManager_HJH>();
        audio = GetComponent<AudioSource>();
        playerAnimator.SetFloat("RollSpeed", rolling_Speed);
        rigid = GetComponent<Rigidbody2D>();
        player_State = Player_State.Run;
        default_size = transform.localScale.x;
        charge_img.SetActive(false);
        currentPos = transform.position;
    }
    private void OnEnable()
    {
        if (!start) start = true;
        else
        {
            invincible = true;
            StartCoroutine(Invincible());
            StartCoroutine(Blink());
        }
    }

    private void Update()
    {

        //Debug.Log(transform.localPosition);
        //���� �ڵ� �ּ����� ����
        #region test
        //if (player_State == Player_State.Rolling)
        //{
        //    if (transform.position.y < 0)
        //    {
        //        rigid.velocity = new Vector2(rigid.velocity.x, 0);
        //    }
        //}
        //if (jumping)
        //{
        //    rigid.AddForce(Vector2.down);
        //}
        //if (floorCheck)
        //{
        //    test = 0.0f; ;
        //    RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, Vector2.down, transform.localScale.y, LayerMask.GetMask("ground")); //�ٴ� �˻� �ؼ� ������ �� �ְ�
        //    Debug.DrawRay(gameObject.transform.position, Vector2.down * hit.distance, Color.red);
        //    if (hit.collider != null)
        //    {
        //        isFloor = true;
        //        jumping = false;
        //        if (!jumping && !invincible)
        //        {
        //            if(player_State == Player_State.Rolling)
        //            {
        //                moveVec += (Vector3)Vector2.right * rolling_MoveSpeed;
        //                //rigid.velocity = Vector2.right * speed + Vector2.right * rolling_MoveSpeed;
        //                //transform.position += (Vector3)Vector2.right * rolling_MoveSpeed * Time.deltaTime;
        //            }
        //            else
        //            {
        //                moveVec += (Vector3)Vector2.right * speed;
        //                //rigid.velocity = Vector2.right * speed; //���������� ���� ���� �ӵ� �����ϰ�
        //                //transform.position += (Vector3)Vector2.right * speed * Time.deltaTime;
        //            }
        //            if (!Input.GetMouseButton(0))
        //            {
        //                predictLine.positionCount = 0; // �޸� ���� ���� �� �ȱ׷�����
        //            }
        //        }
        //    }

        //}
        if(Mathf.Abs((currentPos -transform.position).x) < 0.01)
        {
            if(!stuckRespawn && !gameClear)
            {
                stuckRespawn = true;
                StartCoroutine(StuckCheck(currentPos));
            }
        }
        currentPos = transform.position;
        if (transform.position.y < -6)
        {
            //GameOver();
            if (hp <= 2) GameOver();
            else
                Respawn(true);
        }
        //RaycastHit2D hit = Physics2D.Raycast(rayPoint[0].position, Vector2.up, transform.localScale.x / 2, LayerMask.GetMask("ground"));
        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position + new Vector3(0, boxCol.size.y * transform.localScale.y * 0.5f), new Vector2(boxCol.size.x * transform.localScale.x, 1f), 0, Vector2.up, 0.1f, LayerMask.GetMask("ground"));//��
        for(int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider != null)
            {
                hit[i].transform.gameObject.GetComponent<Floor_HJH>().Crash(gameObject);
                upCrushCheck = true;
                StartCoroutine(UpCrushOff());
            }
        }

        hit = Physics2D.BoxCastAll(transform.position + new Vector3(boxCol.size.x * transform.localScale.x * 0.5f, 0), new Vector2(1f, boxCol.size.y * transform.localScale.y), 0, Vector2.right, 0.1f, LayerMask.GetMask("ground")); //��
        for(int i = 0; i< hit.Length; i++)
        {
            if (hit[i].collider != null)
            {
                hit[i].transform.gameObject.GetComponent<Floor_HJH>().Crash(gameObject);
            }

        }

        //if (hp_List.transform.childCount > 0)
        //{
        //    if (Hp != 0)
        //    {
        //        for (int i = 0; i < 10; i++)
        //        {
        //            if (i < hp)
        //            {
        //                hp_List.transform.GetChild(i).gameObject.SetActive(true);
        //            }
        //            else
        //            {
        //                hp_List.transform.GetChild(i).gameObject.SetActive(false);
        //            }
        //        }
        //    }
        //    else if (Hp == 0)
        //    {
        //        for (int i = 0; i < hp_List.transform.childCount; i++)
        //        {
        //            hp_List.transform.GetChild(i).gameObject.SetActive(false);
        //        }
        //    }

        //}
        //if (hp < 1)
        //{

        //}
        #endregion
        if (!gameClear)
        {
            Camera.main.transform.position = new Vector3((transform.position + new Vector3(camera_distance, 0, 0)).x, 2, -10);//�÷��̾����� ���缭 ī�޶� ��ġ
        }
        else
        {
            Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
            if (viewPos.x > 1 && viewPos.z > 0)
            {
                //if (ui.Select_chk) selectPanel.SetActive(true);
                if (ui.Ending)
                {
                    ui.Ending_Check();
                    //InGameUI, Ending_Check() �Լ��� �����̵����׽��ϴ�
                    //int num = ui.Scene_num < 30 ? (ui.Scene_num / 5) - 2 : ui.Scene_num / 2;

                    //switch (num)
                    //{
                    //    case 1:
                    //        ui.Load_Story("ending1");
                    //        break;
                    //    case 2:
                    //        ui.Load_Story("ending2");
                    //        break;
                    //    case 3:
                    //        ui.Load_Story("ending3");
                    //        break;
                    //    case 15:
                    //        ui.Load_Story("hidden1");
                    //        break;
                    //    case 16:
                    //        ui.Load_Story("hidden2");
                    //        break;
                    //}
                    //GameManager_shj.Getinstance.Save_data.ending[num] = true;
                    //ui.Data_Reset();
                    //ui.Data_Save();
                    gameObject.GetComponent<Player_HJH>().enabled = false;
                }
                else
                {
                    if (SceneManager.GetActiveScene().name.Contains("Hidden"))
                    {
                        hiddenGameClearPanel.SetActive(true);
                    }
                    else
                    {
                        gameClearPanel.SetActive(true);
                    }

                }
                gameOverPanel.SetActive(false);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        //#if UNITY_EDITOR
        if (!jumping && Input.GetMouseButton(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject() && Time.deltaTime > 0f)
            {
                jump_charge = jump_charge <= maxJumpPower ? jump_charge + Time.deltaTime * charge_speed : maxJumpPower; //��¡�ϸ� �������� �������ϴ�
                                                                                                                        //charge_img.enabled = true; //uiȰ��ȭ
                charge_img.SetActive(true); // �����̴��� ����
                                            //Debug.Log(jump_charge/(maxJumpPower -minJumpPower) - 1);
                                            //charge_img.fillAmount = (jump_charge - minJumpPower) / ((jump_charge - minJumpPower) + (maxJumpPower - jump_charge))/*Time.deltaTime*//*jump_charge*/; //�����Ǿ���
                charge_img.GetComponent<Slider>().value = (jump_charge - minJumpPower) / ((jump_charge - minJumpPower) + (maxJumpPower - jump_charge));
                if (charge_img.GetComponent<Slider>().value >= 1.0f)
                    charge_img.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                //if (timeSlowOnOff)
                //{
                //    Time.timeScale = timeSlowSpeed;
                //}
                if (jump_charge < minJumpPower)
                {
                    jump_charge = minJumpPower;
                }
                if (jump_charge > maxJumpPower)
                {
                    jump_charge = maxJumpPower;
                }
            }

        }
        else if (!jumping && Input.GetMouseButtonUp(0))
        {
            //test = true;
            jumpBool = true;

            //Time.timeScale = 1f;
        }


        //if(jumping && test < 1.0f)
        //{
        //    float angle = 75 * Mathf.Deg2Rad;
        //    Vector3 direction = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        //    transform.position += (direction - transform.position) * 10 * Time.deltaTime;
        //    test += Time.deltaTime;
        //}

        //#elif UNITY_ANDROID
        //        if (Input.touchCount > 0 && !jumping)
        //        {
        //            if (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)
        //            {
        //                charge_img.enabled = true; //uiȰ��ȭ
        //                jump_charge = jump_charge <= 1.0f ? jump_charge + Time.deltaTime * charge_speed : 1.0f; //��¡�ϸ� �������� �������ϴ�
        //                charge_img.fillAmount = jump_charge;
        //            }
        //            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
        //                Jump();
        //        }
        //#endif
        if(rigid.velocity.x < speed)
        {
            slowTime += Time.deltaTime;
            if(slowTime > 2f)
            {
                NuckBackAddForce();
                slowTime = 0f;
            }
        }
        else
        {
            slowTime = 0;
        }
        if(state == Player_State.IceFloor && !iceFloorRunning)
        {
            StartCoroutine(IceFloorCheck());
        }
        else if(state == Player_State.Run && speed != nomalSpeed && nuckBackCoroutine == null)
        {
            nuckBackCoroutine = StartCoroutine(NuckBackAddForce());
        }
    }
    IEnumerator IceFloorCheck() //�ؿ� ���� �ٴ��� �ִ���
    {
        bool noIce = false;
        while (true)
        {
            iceFloorRunning = true;
            RaycastHit2D[] hit = Physics2D.RaycastAll (gameObject.transform.position - new Vector3(0, boxCol.size.y * transform.localScale.y * 0.5f - boxCol.offset.y), Vector2.down, 50f, LayerMask.GetMask("ground"));
            if(hit != null)
            {
                for(int i =0; i<hit.Length; i++)
                {
                    if (hit[i].collider.name.Contains("iceFloor"))
                    {
                        noIce = false; 
                        break ;
                    }
                    if(i == hit.Length - 1)
                    {
                        noIce = true;
                    }
                }

            }
            if (noIce)
            {
                iceFloorRunning = false;
                speed = nomalSpeed;
                state = Player_State.Run;
                break;
            }
            yield return null;
        }
    }
    private void FixedUpdate()
    {
        RunRoll();
        Jump();
        NukBack();

    }
    public void GameOver()
    {
        if(SceneManager.GetActiveScene().name.Contains("Hidden"))
        {
            hiddenGameOverPanel.SetActive(true);
        }
        else if (SceneManager.GetActiveScene().name.Contains("Challenge"))
        {
            challengGameOverPanel.transform.GetChild(0).GetComponent<Text>().text = "���� �ð� : " + timer.text.text + " ��";
            challengGameOverPanel.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(true);

        }
        Time.timeScale = 0f;
    }
    IEnumerator UpCrushOff()
    {
        yield return new WaitForSeconds(0.5f);
        upCrushCheck = false;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        #region ���� �ڵ�
        //RaycastHit2D hit = Physics2D.Raycast(rayPoint[0].position, Vector2.up, transform.localScale.x / 2, LayerMask.GetMask("ground"));
        //if (hit.collider != null)
        //{
        //    Gizmos.DrawRay(rayPoint[0].position, Vector2.up * hit.distance);
        //}
        //else
        //{
        //    Gizmos.DrawRay(rayPoint[0].position, Vector2.up * transform.localScale.x / 2);
        //}
        //hit = Physics2D.Raycast(rayPoint[1].position, Vector2.up, transform.localScale.x / 2, LayerMask.GetMask("ground"));
        //if (hit.collider != null)
        //{
        //    Gizmos.DrawRay(rayPoint[1].position, Vector2.up * hit.distance);
        //}
        //else
        //{
        //    Gizmos.DrawRay(rayPoint[1].position, Vector2.up * transform.localScale.x / 2);
        //}
        //hit = Physics2D.Raycast(rayPoint[2].position, Vector2.right, transform.localScale.x / 2, LayerMask.GetMask("ground"));
        //if (hit.collider != null)
        //{
        //    Gizmos.DrawRay(rayPoint[2].position, Vector2.right * hit.distance);
        //}
        //else
        //{
        //    Gizmos.DrawRay(rayPoint[2].position, Vector2.right * transform.localScale.x / 2);
        //}
        //hit = Physics2D.Raycast(rayPoint[3].position, Vector2.right, transform.localScale.x / 2, LayerMask.GetMask("ground"));
        //if (hit.collider != null)
        //{
        //    Gizmos.DrawRay(rayPoint[3].position, Vector2.right * hit.distance);
        //}
        //else
        //{
        //    Gizmos.DrawRay(rayPoint[3].position, Vector2.right * transform.localScale.x / 2);
        //}
        //hit = Physics2D.Raycast(rayPoint[4].position, Vector2.right, transform.localScale.x / 2, LayerMask.GetMask("ground"));
        //if (hit.collider != null)
        //{
        //    Gizmos.DrawRay(rayPoint[4].position, Vector2.right * hit.distance);
        //}
        //else
        //{
        //    Gizmos.DrawRay(rayPoint[4].position, Vector2.right * transform.localScale.x / 2);
        //}
        #endregion
        //���� ��� ��
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + new Vector3(0,boxCol.size.y * transform.localScale.y*0.5f + boxCol.offset.y), new Vector2(boxCol.size.x * transform.localScale.x, 1f), 0, Vector2.up,0.1f, LayerMask.GetMask("ground"));
        if(hit.collider != null)
        {
            Gizmos.DrawRay(transform.position + new Vector3(0, boxCol.size.y * transform.localScale.y*0.5f + boxCol.offset.y), Vector2.up * hit.distance);
            Gizmos.DrawWireCube(transform.position + new Vector3(0, boxCol.size.y * transform.localScale.y * 0.5f + boxCol.offset.y) + (Vector3)Vector2.up * hit.distance, new Vector2(boxCol.size.x * transform.localScale.x, 1f));
        }
        else
        {
            Gizmos.DrawRay(transform.position + new Vector3(0, boxCol.size.y * transform.localScale.y * 0.5f + boxCol.offset.y), Vector2.up * 0.1f);
        }
        //������ ��� ��
        hit = Physics2D.BoxCast(transform.position + new Vector3(boxCol.size.x * transform.localScale.x * 0.5f + boxCol.offset.x, 0), new Vector2(1f, boxCol.size.y * transform.localScale.y), 0, Vector2.right, 0.1f, LayerMask.GetMask("ground"));
        if (hit.collider != null)
        {
            Gizmos.DrawRay(transform.position + new Vector3(boxCol.size.x * transform.localScale.x * 0.5f + boxCol.offset.x, 0), Vector2.right * hit.distance);
            Gizmos.DrawWireCube(transform.position + new Vector3(boxCol.size.x * transform.localScale.x * 0.5f + boxCol.offset.x, 0) + (Vector3)Vector2.right * hit.distance, new Vector2(1f, boxCol.size.y * transform.localScale.y));
        }
        else
        {
            Gizmos.DrawRay(transform.position + new Vector3(boxCol.size.x * transform.localScale.x * 0.5f + boxCol.offset.x, 0), Vector2.right * 0.1f);
        }
        Gizmos.DrawRay(gameObject.transform.position- new Vector3(0, boxCol.size.y * transform.localScale.y * 0.5f - boxCol.offset.y), Vector2.down * 0.1f);
    }
    //private void FixedUpdate()
    //{
    //    if(test)
    //    {
    //        Jump();
    //        test = false;
    //    }mathf
    //}
    public void RunRoll()
    {
        //rigid.AddForce(Vector2.right * velocity);
        if (player_State == Player_State.Rolling && !rollStart) //������
        {
            StartCoroutine(Rolling());
            rollStart = true;
        }
        if (player_State == Player_State.Rolling)
        {
            nuckBackDuring = false;
        }
        if (!nuckBackDuring)
        {
            if (player_State == Player_State.Rolling)
            {
                transform.localScale = new Vector3(rolling_size, rolling_size, 0.0f);
                rigid.velocity = Vector2.right * rolling_MoveSpeed + new Vector2(0, rigid.velocity.y);
                //vecSpeed.x = rolling_MoveSpeed;
            }
            else
            {
                transform.localScale = new Vector3(default_size, default_size, 0.0f);
                rigid.velocity = Vector2.right * speed + new Vector2(0, rigid.velocity.y);
                //vecSpeed.x = speed;
            }
        }
        if (floorCheck)
        {
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position - new Vector3(0, boxCol.size.y * transform.localScale.y * 0.5f - boxCol.offset.y), Vector2.down ,0.1f, LayerMask.GetMask("ground"));
            if (hit.collider == null)
            {
                rigid.AddForce(Vector2.down * gravity);
            }
            if (!isFloor)
            {
                //velocity.y = gravity;
            }
            if (!upCrushCheck)
            {
                if (hit.collider != null)
                {
                    isFloor = true;
                    if (jumping)
                    {
                        playerAnimator.SetTrigger("JumpEnd");
                    }
                    jumping = false;
                }
            }
        }


    }

    public void Jump() //����
    {
        if (!jumpBool)
        {
            return;
        }
        audio.Play();
        jumpBool = false;
        floorCheck = false;
        isFloor = false;
        StartCoroutine(FloorCheck());
        playerAnimator.SetTrigger("Jump"); //���� �ִϸ��̼�
        jumping = true; //������
        //StartCoroutine(HeightTest());
        //Debug.Log(jump_up_power * jump_charge);
        if(nuckBackCoroutine != null)
        {
            nuckBackDuring = false;
            StopCoroutine(nuckBackCoroutine);
            rigid.velocity = new Vector2(speed, 0);
        }
        rigid.AddForce((Vector2.up * jump_up_power) * jump_charge, ForceMode2D.Impulse);
        //velocity.y = jump_up_power *jump_charge;
        //charge_img.enabled = false; //ui��Ȱ��ȭ
        charge_img.SetActive(false);
        jump_charge = 0.0f;
        //charge_img.fillAmount = 0.0f; //�߰���
        charge_img.GetComponent<Slider>().value = 0.0f;
        charge_img.GetComponentInChildren<TextMeshProUGUI>().enabled = false;

        #region �����ڵ�
        //Debug.Log((Vector2.up * jump_up_power) * 50 * jump_charge);
        //rigid.AddForce((Vector2.up * jump_up_power) * 0.75f * jump_charge,ForceMode2D.Impulse);
        //rigid.velocity = (Vector2.right * jump_right_power + Vector2.up * jump_up_power) * jump_charge;
        //rigid.AddForce(Vector2.up * jump_up_power * jump_charge, ForceMode2D.Impulse); //��¡�� ��ŭ ����
        //if(jump_cnt != 2) //2�� ���� ����
        //{
        //    jump_cnt++;
        //    rigid.velocity = Vector2.zero;
        //    rigid.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
        //}
        #endregion
    }
    IEnumerator HeightTest()
    {
        Vector3 trans = transform.position;
        Vector3 trans2 = trans;
        while (true)
        {
            trans = transform.position;
            yield return null;
            if (trans2.y > trans.y)
            {
                //Debug.Log(trans2.y);
                break;
            }
            else
            {
                trans2 = trans;
            }
        }
    }

    IEnumerator FloorCheck()
    {
        yield return new WaitForSeconds(0.1f);
        floorCheck = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.layer == 8)
        //{

        //}//jumping = false;
        //else if (collision.gameObject.layer == 9)
        //{
        //    Rock_HJH rock;
        //    if (collision.gameObject.TryGetComponent<Rock_HJH>(out rock))
        //    {
        //        rock.RockTouch();
        //    }
        //    collision.boxCol.enabled = false;

        //    //collision.gameObject.SetActive(false);
        //    if(player_State == Player_State.Run)
        //        hp--;
        //}
    }

    void PredictLine(Vector2 startPos, Vector2 vel)  //������ ����
    {
        int step = 120;
        float deltaTime = Time.fixedDeltaTime;
        Vector2 gravity = (Vector2)Physics.gravity + Vector2.down;
        Vector2 position = startPos;
        Vector2 velocity = vel;
        predictLine.positionCount = 120;
        for (int i = 0; i < step; i++)
        {
            position += velocity * deltaTime + gravity * deltaTime * deltaTime;
            velocity += gravity * deltaTime;
            predictLine.SetPosition(i, position);
            Collider2D colls = Physics2D.OverlapCircle(position, 0.5f);
            if (colls != null && colls.transform.name != "Player")
            {
                predictLine.positionCount = i; //�������� �ٸ� ��ü�� �浹�� ���̻� �׸��� �ʰ�
                break;
            }

        }
    }
    public void NukBack()
    {
        if (!nuckBackBool)
        {
            return;
        }
        nuckBackBool = false;
        invincible = true;
        rigid.velocity = Vector2.zero;
        rigid.AddForce((Vector2.left * nuckBackPower), ForceMode2D.Force);
        nuckBackDuring = true;
        nuckBackCoroutine = StartCoroutine(NuckBackAddForce());
        StartCoroutine(Invincible());
        StartCoroutine(Blink());
    }


    IEnumerator NuckBackAddForce()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            rigid.AddForce(Vector2.right * velocity);
            if (rigid.velocity.x > speed)
            {
                rigid.velocity = Vector2.right * speed;
                nuckBackCoroutine = null;
                nuckBackDuring = false;
                break;
            }
        }
    }
    IEnumerator Invincible() //�������� ���� �ð� �� ���ִ� �ڷ�ƾ
    {
        yield return new WaitForSeconds(invincibleTime);
        invincible = false;
    }
    IEnumerator Blink() //���ΰ� �����̰�
    {
        int count = 0;
        gameObject.SetActive(true);

        while (count < blinkCount)
        {
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0);
            yield return new WaitForSeconds(blinkSpeed);
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
            yield return new WaitForSeconds(blinkSpeed);
            count += 2;
        }
    }
    IEnumerator Rolling() //������
    {
        //boxCol.isTrigger = true; //�ӽ� �ּ�
        playerAnimator.SetTrigger("Roll"); //������ �ִϸ��̼� �۵�
        boxCol.offset = Vector2.zero;
        boxCol.size = new Vector2(1.7f, 1.7f);
        float currentTime = 0;
        while (true)
        {
            if(ui.countdown < 1)
            {
                currentTime += Time.deltaTime;

            }
            if(currentTime > rolling_time)
            {
                break;
            }
            yield return null;
        }
        //yield return new WaitForSeconds(rolling_time); //�����ð����� ����������
        boxCol.offset = colOffset;
        boxCol.size = colSize;
        player_State = Player_State.Run; //�޸��� ���·� ����
        //boxCol.isTrigger = false; //�ӽ� �ּ�
        playerAnimator.SetTrigger("RollEnd");
        rollStart = false;
        //playerAnimator.SetBool("Rolling", false);
    }
    IEnumerator StuckCheck(Vector3 nowPos)
    {
        float current = 0;
        while(true)
        {
            current += Time.deltaTime;
            if(Mathf.Abs((nowPos-currentPos).x)>0.01)
            {
                stuckRespawn = false;
                break;
            }
            if(current > stuckCheckTime)
            {
                stuckRespawn = false;
                Respawn(false);
                break;
            }
            yield return null;
        }
    }

    public void Respawn(bool damage)
    {
        float default_x = 1.0f;

        charge_img.SetActive(false);
        charge_img.GetComponent<Slider>().value = 1.0f;

        RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.localPosition.x + default_x, 5, 0), Vector3.down, 30.0f);

        while (hit.collider == null || hit.collider.gameObject.layer != 8)
        {
            default_x += 3.0f;
            hit = Physics2D.Raycast(new Vector3(transform.localPosition.x + default_x, 5, 0), Vector3.down, 30.0f);
        }


        //if(GameObject.Find("EndingPoint").transform.position.x - cam.transform.position.x > 14) ī�޶� ���׷����� �ּ�
        //    cam.transform.position = new Vector3((transform.position + new Vector3(camera_distance, 0, 0)).x, 2, -10);//�÷��̾����� ���缭 ī�޶� ��ġ

        //Camera.main.transform.position =  new Vector3(GameObject.Find("EndingPoint").transform.position.x - 13.9f,2,-10);
        if (hp - 2 > 0 && damage) Hp -= 2;

        if (ui.respawn) gameOverPanel.SetActive(false);

        nuckBackDuring = false;
        if(nuckBackCoroutine != null)
        {
            StopCoroutine(nuckBackCoroutine);
        }
        ui.Start();
        rigid.velocity = Vector2.zero;
        transform.localPosition = hit.point + (Vector2)Vector3.up * 1.5f;
    }
}
