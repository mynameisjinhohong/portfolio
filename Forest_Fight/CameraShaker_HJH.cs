using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker_HJH : MonoBehaviour
{
    CameraMove3D_LHS cm;
    Camera mainCamera;
    Vector3 cameraPos;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        cm = mainCamera.GetComponent<CameraMove3D_LHS>();
    }
    public void Shake(float power,float time)
    {
        cameraPos = mainCamera.transform.position;
        StartCoroutine(CameraShake(time, power));
    }
    IEnumerator CameraShake(float duration, float magnitude)
    {
        float timer = 0;
        while (timer <= duration)
        {
            mainCamera.transform.localPosition = Random.insideUnitSphere * magnitude + cameraPos;
            timer += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.localPosition = cameraPos;
    }
}