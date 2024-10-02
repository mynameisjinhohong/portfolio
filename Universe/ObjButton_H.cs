using Dummiesman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjButton_H : MonoBehaviour
{
    Button button;
    //해당 버튼의 오브젝트가 3D일 경우, 불러올 오브젝트의 이름을 _3DObjectName 에 저장하기
    public string _3DObjectName;
    public Texture2D _3DObjectTexture;
    public enum Type
    {
        _2D,
        _3D
    }
    public Type type;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CanDrawObject);
    }
    void CanDrawObject()
    {
        if (type == Type._2D)
        {
            MapEditor_L.instance.objectType = MapEditor_L.ObjectType.Image;
            MapEditor_L.instance.objImage = GetComponent<Image>().sprite.texture;
        }
        else
        {
            
            MapEditor_L.instance.loadedObject = new OBJLoader().Load(Application.persistentDataPath + "/" + _3DObjectName + ".obj");
            MapEditor_L.instance.loadedObject.SetActive(false);
            MapEditor_L.instance.placementType = MapEditor_L.PlacementType._3DObject;
            MapEditor_L.instance.currStamp3DObjName = _3DObjectName;
            MapEditor_L.instance.currStamp3DTexture = _3DObjectTexture;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}