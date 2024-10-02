using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPresetEditor_H : MonoBehaviour
{
    public string mapPresetName;
    // Start is called before the first frame update
    private void Awake()
    {
        SpaceInfo.spaceURL = mapPresetName;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpaceInfo.spaceURL = mapPresetName; 
    }
}
