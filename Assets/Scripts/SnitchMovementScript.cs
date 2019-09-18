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

    private double startTimestamp;

    private double getUnixTimeTamp()
    {
        return startTimestamp + Time.time;
    }

    IEnumerator arrangeLocationInfos()
    {
        if (locationInfos == null || locationInfos.Count < 1) yield break;
        var enumerator = locationInfos.GetEnumerator();
        int idx=0;
        while (enumerator.MoveNext()) {
            idx++;
            LocationInfo location = enumerator.Current;
            double diff = location.time - getUnixTimeTamp();
            if (0 < diff && diff < eps) {
                currentLocationInfo = location;
                locationInfos.RemoveRange(0, idx);
                size = locationInfos.Count;
                yield break;
            }
        }
    }

    private void move()
    {
        if ((object) currentLocationInfo == null) return;
        transform.position = currentLocationInfo.position;
    }

    void Start()
    {
        startTimestamp = (DateTime.UtcNow - new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalSeconds;
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
