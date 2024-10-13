using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 100, 0);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
