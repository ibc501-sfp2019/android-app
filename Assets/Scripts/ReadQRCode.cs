using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadQRCode : MonoBehaviour
{
    public Text UIText;

    // Start is called before the first frame update
    void Start()
    {
        UIText.text = GameManager.instance.readJson;
    }
}
