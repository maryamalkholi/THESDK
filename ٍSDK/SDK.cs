using System;
using System.Collections;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class SDK : IFetchDataFromAPI
{
    public string data;
    public Action<string> OnDataReceived; 

    public void FetchDataFromAPI(string url)
    {
        CoroutineRunner.instance.StartCoroutine(GetApiRequest(url));
    }

    private IEnumerator GetApiRequest(string url)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + unityWebRequest.error);
            }
            else
            {
                string json = unityWebRequest.downloadHandler.text;
                DataConverter value = JsonConvert.DeserializeObject<DataConverter>(json);
                data = value.Value;
                Debug.Log("Fetched: " + data);

                OnDataReceived?.Invoke(data); 
            }
        }
    }

    public class DataConverter
    {
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
