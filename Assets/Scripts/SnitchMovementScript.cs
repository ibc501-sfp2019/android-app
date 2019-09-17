using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CommonScript;

public class SnitchMovementScript : MonoBehaviour
{
    [SerializeField]
    public int snitchId;

    [SerializeField]
    public int size=0;
    public List<LocationInfo> locationInfos;

    private LocationInfo currentLocationInfo;

    private float eps = 0.05f;

    private PeriodicRoutine periodicRoutine;

    IEnumerator arrangeLocationInfos()
    {
        if (locationInfos == null || locationInfos.Count < 1) yield break;
        // Debug.Log("start");
        Double now = (DateTime.UtcNow - new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalSeconds;
        foreach (LocationInfo location in locationInfos)
        {
            if (location.time < now) {
                locationInfos.Remove(location);
                yield return null;
            }
            if ((location.time - now) < eps) {
                currentLocationInfo = location;
            }
            size = locationInfos.Count;
            yield break;
        }
        // Debug.Log("end");
    }

    private void move()
    {
        if ((object) currentLocationInfo == null) return;
        transform.position = currentLocationInfo.position;
    }

    void Start()
    {
        periodicRoutine = new PeriodicRoutine(0.05f);
        periodicRoutine.setRoutine(arrangeLocationInfos);
    }

    void FixedUpdate()
    {
        move();
    }

    void Update() {
        StartCoroutine(periodicRoutine.run());
    }
}
