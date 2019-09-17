using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCam : MonoBehaviour
{
    public GameObject playerObject;
    Transform tr, trPlayer;
    public float height;

    void Start()
    {
        tr = gameObject.transform;
        trPlayer = playerObject.transform;
        tr.position = new Vector3(trPlayer.position.x, height, trPlayer.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        tr.position = new Vector3(trPlayer.position.x, height, trPlayer.position.z);
    }
}
