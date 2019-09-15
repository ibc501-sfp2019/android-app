using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class ReadQRCode : MonoBehaviour
{
    public Text UIText;
    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        JsonData jsonData = JsonMapper.ToObject(GameManager.instance.readJson);
        //set camera position
        float x = float.Parse(jsonData["location"]["x"].ToString());
        float y = float.Parse(jsonData["location"]["y"].ToString());
        float z = float.Parse(jsonData["location"]["z"].ToString());
        camera.transform.position = new Vector3(x, y, z);
        //set camera rotation
        x = float.Parse(jsonData["rotation"]["x"].ToString());
        y = float.Parse(jsonData["rotation"]["y"].ToString());
        z = float.Parse(jsonData["rotation"]["z"].ToString());
        camera.transform.rotation = Quaternion.Euler(x, y, z);

        UIText.text = GameManager.instance.readJson;
    }
}
