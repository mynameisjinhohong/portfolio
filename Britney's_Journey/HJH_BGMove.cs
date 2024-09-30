using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HJH_BGMove : MonoBehaviour
{
    public static HJH_BGMove Instance;

    MeshRenderer mr;
    public Material mat;

    public bool isMove = true;

    public float speed = 0.2f;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mat = mr.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove == true)
        {
            speed = HJH_PlayerMove.instance.mapSpeed * 0.1f;
            mat.mainTextureOffset += Vector2.right * speed * Time.deltaTime;
        }
    }
}
