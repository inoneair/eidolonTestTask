using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private AnalyticsService analyticsService;

    [SerializeField]
    private float _cooldownBeforeGenerateEvent;

    private float secondsToCoolDown;

    private int id = 0;

    // Update is called once per frame
    void Update()
    {
        secondsToCoolDown -= Time.deltaTime;

        if (secondsToCoolDown <= 0)
        {
            secondsToCoolDown = _cooldownBeforeGenerateEvent;

            analyticsService.TrackEvent("update", id.ToString());
            ++id;
        }
        
    }
}
