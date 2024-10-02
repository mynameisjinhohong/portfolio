using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMajorInfo_H : MonoBehaviour
{
    public Text majorText;
    public Text concentrationText;
    public Text keywordText;
    public Text specialityText;
    public Text hopepathText;
    bool setDone = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (setDone == false)
        {
            SetInfo();
            if (concentrationText.text.Length > 0)
            {
                setDone = true;
            }
        }
    }

    private void SetInfo()
    {
        majorText.text = GameManager.instance.userinfo.departmentName;
        concentrationText.text = GameManager.instance.userinfo.majorConcentration;
        keywordText.text = GameManager.instance.userinfo.majorKeyword;
        specialityText.text = GameManager.instance.userinfo.majorSpecialty;
        hopepathText.text = GameManager.instance.userinfo.majorHopePath;
    }
}
