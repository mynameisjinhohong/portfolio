using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class URLButton_H : MonoBehaviour
{
    Button me;
    public string url;
    public string name;
    void Start()
    {
        me = GetComponent<Button>();
        me.onClick.AddListener(GoURL);
    }

    void GoURL()
    {
        Application.OpenURL(url);
    }
    private void Update()
    {
        me.transform.GetChild(0).GetComponent<Text>().text = name;
    }
}
