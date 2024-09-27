using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCall_HJH : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        if (StageMenu_lyd.instance.whatScene == 1 && StageMenu_lyd.instance.whatCountry == BTNType.Korea )
        {

            Camera camera;
            camera = Camera.main;
            GameObject g = GameObject.Find("Pioneer_HJH(Clone)");

            g.transform.position = new Vector3(441.672943f, 10, 375);

            GameObject u = GameObject.Find("ArmyUnit_HJH(Clone)");

            u.transform.position = new Vector3(458.993439f, 10, 375);
            camera.transform.position = new Vector3(450, 57, 328);
            camera.transform.eulerAngles= new Vector3(45f, 0, 0);
        }
        else if (StageMenu_lyd.instance.whatScene == 1 && StageMenu_lyd.instance.whatCountry == BTNType.China)
        {
            Camera camera;
            camera = Camera.main;
            GameObject g = GameObject.Find("Pioneer_HJH(Clone)");


            g.transform.position = new Vector3(190.525574f, 10, 780);

            GameObject u = GameObject.Find("ArmyUnit_HJH(Clone)");

            u.transform.position = new Vector3(173.205078f, 10, 780);

            camera.transform.position = new Vector3(184.5f, 57, 734.299988f);
            camera.transform.eulerAngles = new Vector3(45f, 0, 0);
        }
        else if (StageMenu_lyd.instance.whatScene == 1 && StageMenu_lyd.instance.whatCountry == BTNType.Japan)
        {
            Camera camera;
            camera = Camera.main;
            GameObject g = GameObject.Find("Pioneer_HJH(Clone)");

            g.transform.position = new Vector3(1021.90991f, 10, 450);

            GameObject u = GameObject.Find("ArmyUnit_HJH(Clone)");

            u.transform.position = new Vector3(1039.23047f, 10, 450);

            camera.transform.position = new Vector3(1028, 57, 387);
            camera.transform.eulerAngles = new Vector3(45f, 0, 0);
        }
        else if(StageMenu_lyd.instance.whatScene == 2)
        {
            Camera camera;
            camera = Camera.main;
            camera.transform.position = new Vector3(504.333344f, 838, 492);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}