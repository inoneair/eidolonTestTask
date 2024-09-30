using System;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class AnalyticsProcessor : IAnalyticsProcessor
{
    private MonoBehaviour _coroutineStarter;
    private readonly string _serverUrl;

    public AnalyticsProcessor(MonoBehaviour coroutineStarter, string url)
    {
        _coroutineStarter = coroutineStarter;
        _serverUrl = url;
    }

    public void SendEvents(CachedEventList cachedEvents, Action<bool, CachedEventList> webRequestHandler)
    {
        _coroutineStarter.StartCoroutine(SendEventsCoroutine(cachedEvents, webRequestHandler));
    }

    private IEnumerator SendEventsCoroutine(CachedEventList cachedEvents, Action<bool, CachedEventList> webRequestHandler)
    {
        var eventsAsJson = JsonConvert.SerializeObject(cachedEvents);
        var request = CreateJsonPostRequest(_serverUrl, eventsAsJson);

        yield return request.SendWebRequest();

        var isSendingSuccessful = request.result == UnityWebRequest.Result.Success;

        webRequestHandler?.Invoke(isSendingSuccessful, cachedEvents);
    }

    private UnityWebRequest CreateJsonPostRequest(string url, string json)
    {
        var request = new UnityWebRequest(url, "POST");
        var bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }
}
