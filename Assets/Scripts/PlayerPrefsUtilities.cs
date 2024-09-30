using UnityEngine;
using Newtonsoft.Json;

public static class PlayerPrefsUtilities
{
    public static string GetString(string key, string defaultValue) =>
        PlayerPrefs.GetString(key, defaultValue);    

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
        Save();
    }

    public static void Save() =>
        PlayerPrefs.Save();    

    public static void SetString(string key, string value, bool isSaveImmediately = false)
    {
        PlayerPrefs.SetString(key, value);

        if (isSaveImmediately)
            Save();
    }

    public static T GetObjectValue<T>(string key, T defaultValue = null) where T : class
    {
        T result = defaultValue;
        var savedObjectValue = GetString(key, string.Empty);

        if (!string.IsNullOrEmpty(savedObjectValue))        
            result = JsonConvert.DeserializeObject<T>(savedObjectValue);

        return result;
    }

    public static void SetObjectValue<T>(string key, T value, bool saveImmediately = false)
            where T : class
    {
        var objectValue = (value == null) ? (string.Empty) : (JsonConvert.SerializeObject(value));

        SetString(key, objectValue, saveImmediately);
    }
}
