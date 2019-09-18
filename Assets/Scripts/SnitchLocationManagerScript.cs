using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using CommonScript;
using Enum;

public struct LocationInfo
{
    public double time;
    public Vector3 position;
    public LocationInfo(double time, Vector3 position) {
        this.time = time;
        this.position = position;
    }
}

public class SnitchLocationManagerScript : MonoBehaviour
{
    [SerializeField]
    private int playId=1;
    [SerializeField]
    public string serverDomain="http://133.11.59.240:8000";

    private Dictionary<int, SnitchMovementScript> snitchMovementScriptDict;

    private PeriodicRoutine periodicRoutine;

    // Start is called before the first frame update
    void Start()
    {
        snitchMovementScriptDict = new Dictionary<int, SnitchMovementScript>();
        foreach(GameObject snitch in GameObject.FindGameObjectsWithTag("Snitch")) {
            SnitchMovementScript script = snitch.GetComponent<SnitchMovementScript>();
            snitchMovementScriptDict.Add(script.snitchId, script);
        }
        periodicRoutine = new PeriodicRoutine(1.2f);
        periodicRoutine.setRoutine(updateLocationInfos);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(periodicRoutine.run());
    }

    IEnumerator updateLocationInfos()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverDomain + "/observe/?play_id=" + playId);
        yield return request.SendWebRequest();

        if (request.isNetworkError) {
            yield break;
        }
        JsonData response = JsonMapper.ToObject(request.downloadHandler.text);
        if (!response.IsObject) {
            yield break;
        }
        if ((int) ResponseStatus.OK != (int) response["status"]) {
            yield break;
        }
        foreach(JsonData ballInfo in response["data"]) {
            SnitchMovementScript script = snitchMovementScriptDict[(int) ballInfo["ball_id"]];
            List<LocationInfo> preInfos = script.locationInfos;
            List<LocationInfo> list = new List<LocationInfo>();
            double lastestTime = 0;
            if (preInfos != null && preInfos.Count > 0) {
                list = preInfos;
                lastestTime = list[list.Count - 1].time;
            }
            if (list.Count > 200) continue;
            foreach (JsonData info in ballInfo["data"]) {
                double timestamp = double.Parse(info["timestamp"].ToString());
                if (lastestTime > timestamp) continue;

                float x =  float.Parse(info["location"]["x"].ToString());
                float y =  float.Parse(info["location"]["y"].ToString());
                float z =  float.Parse(info["location"]["z"].ToString());
                Vector3 position = new Vector3(x, y, z);
                LocationInfo loc = new LocationInfo(timestamp, position);
                list.Add(loc);
            }
            script.locationInfos = list;
        }
    }
}
