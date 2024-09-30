using System.Collections.Generic;
using UnityEngine;


public class AnalyticsService : MonoBehaviour, IAnalyticsSevice
{
    [SerializeField]
    private string _serverUrl;

    [SerializeField]
    private float _sendCooldownDuration;

    private const string AnalyticsEventCache = "analytics_event_cache";
    private List<CachedEvent> _waitingToSendCachedEvents;
    private IAnalyticsProcessor _processor;

    private float _secondsToTheEndOfCooldown = 0;

    private void Start()
    {
        _waitingToSendCachedEvents = PlayerPrefsUtilities.GetObjectValue(AnalyticsEventCache, new List<CachedEvent>());
        _processor = new AnalyticsProcessor(this, _serverUrl);
    }

    private void Update()
    {
        _secondsToTheEndOfCooldown -= Time.deltaTime;
        if(_secondsToTheEndOfCooldown <= 0)
        {
            _secondsToTheEndOfCooldown = _sendCooldownDuration;
                        
            _processor.SendEvents(new CachedEventList() { events = new List<CachedEvent>(_waitingToSendCachedEvents) }, SendEventsHandler);
            _waitingToSendCachedEvents.Clear();
        }
    }

    public void TrackEvent(string type, string data) =>    
        CacheEvent(type, data);    

    private void CacheEvent(string type, string data)
    {
        _waitingToSendCachedEvents.Add(new CachedEvent()
        {
            type = type,
            data = data
        });

        PlayerPrefsUtilities.SetObjectValue(AnalyticsEventCache, _waitingToSendCachedEvents);
    }

    private void SendEventsHandler(bool isSendingSuccessful, CachedEventList eventList)
    {
        if(!isSendingSuccessful)        
            _waitingToSendCachedEvents.AddRange(eventList.events);        

        SyncCacheStorage(_waitingToSendCachedEvents, AnalyticsEventCache);
    }

    private void SyncCacheStorage(List<CachedEvent> eventList, string key)
    {
        if (eventList.Count > 0)        
            PlayerPrefsUtilities.SetObjectValue(key, eventList);        
        else        
            PlayerPrefsUtilities.DeleteKey(key);        
    }
}
