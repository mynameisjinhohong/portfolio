using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove_HJH : MonoBehaviour
{
    public float zoomSpeed = 10;
    public float cameraSpeed = 10f;
    private Camera mainCamera;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();

    }

    void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;
        if (distance != 0)
        {
            if (mainCamera.fieldOfView <= 150)
            {
                mainCamera.fieldOfView += distance;
                if (mainCamera.fieldOfView >= 150)
                {
                    mainCamera.fieldOfView = 150;
                }
            }
        }
    }
    void Move()
    {
        // 마우스 위치에 따라 맵이동을 하고 싶다
        // 1. 마우스 위치 정보를 불러온다
        Vector3 mPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        // 카메라 시점을 일정 범위 내로 제한하고 싶다
        // 1. 움직일 수 있는 카메라 시점의 x축을 -15 ~ 29 로 제한하고 싶다
        // 2. 움직일 수 있는 카메라 시점의 z축을 -19 ~ 19 로 제한하고 싶다
        float x = Mathf.Clamp(transform.position.x, 80 , 1080);
        float z = Mathf.Clamp(transform.position.z, 40, 965);
        transform.position = new Vector3(x, transform.position.y, z);


        // 2. 만약 mousePosition의 x값이 0이하이고 카메라 위치의 z값이 19를 넘지 않는 다면 (왼쪽) 카메라 위치의 z값을 더해주고
        if (mousePosition.x <= 0)
        {
            mainCamera.transform.position -= cameraSpeed * Vector3.right * Time.deltaTime;
        }
        // 2. 만약 mousePosition의 x값이 1이상이라면(오른쪽) 카메라 위치의 z값을 빼주고
        if (mousePosition.x >= 1)
        {
            mainCamera.transform.position += cameraSpeed * Vector3.right * Time.deltaTime;
        }
        // 2. 만약 mousePosition의 y값이 1이상이라면(위쪽) 카메라 위치의 x값을 더해주고
        if (mousePosition.y >= 0.9)
        {
            mainCamera.transform.position += cameraSpeed * Vector3.forward * Time.deltaTime;
        }
        // 2. 만약 mousePosition의 y값이 0이하라면(아래쪽) 카메라 위치의 x값을 빼준다
        if (mousePosition.y <= 0)
        {
            mainCamera.transform.position -= cameraSpeed * Vector3.forward * Time.deltaTime;
        }
        // 만약 space키를 누르면 카메라 시점을 원래대로 옮기고 싶다
        // 1. 만약 space키를 누르면 메인 화면으로

        //if (Input.GetButtonDown("Jump"))
        //{
        //    mainCamera.transform.position = new Vector3(-9.5f, 22.5f, 0.99f);
        //}
        //// 2. 만약 마우스 휠 or 왼쪽 shift키를 누르면 조합식으로
        //if (Input.GetButtonDown("Fire3"))
        //{
        //    mainCamera.transform.position = new Vector3(58.73f, 22.5f, 14.11f);
        //}

    }

    // Update is called once per frame
    void Update()
    {

        Zoom();
        Move();

    }
}