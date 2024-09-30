using System;

public interface IAnalyticsProcessor
{
    public void SendEvents(CachedEventList cachedEvents, Action<bool, CachedEventList> sendEventsCallback);
}
