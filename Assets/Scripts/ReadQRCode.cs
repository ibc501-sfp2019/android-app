using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.SpatialTracking;

public class ReadQRCode : TrackedPoseDriver
{
    // Start is called before the first frame update
    void Start()
    {
        JsonData jsonData = JsonMapper.ToObject(GameManager.instance.readJson);
        //set playerObject position
        float x = float.Parse(jsonData["location"]["x"].ToString());
        float y = float.Parse(jsonData["location"]["y"].ToString());
        float z = float.Parse(jsonData["location"]["z"].ToString());
        transform.position = new Vector3(x, y, z);
        //set playerObject rotation
        x = float.Parse(jsonData["rotation"]["x"].ToString());
        y = float.Parse(jsonData["rotation"]["y"].ToString());
        z = float.Parse(jsonData["rotation"]["z"].ToString());
        transform.rotation = Quaternion.Euler(x, y, z);
    }
}
