﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

internal class HMSWebRequestHelper
{
    private static HMSWebRequestHelper _instance;
    public static HMSWebRequestHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new HMSWebRequestHelper();
            }
            if (persistedObj == null || behaviour == null)
            {
                _instance.CheckPersistentcy();
            }

            return _instance;
        }
    }

    private static GameObject persistedObj;
    private static HMSWebRequestBehaviour behaviour;

    public void CheckPersistentcy()
    {
        var objs = GameObject.FindObjectsOfType<HMSWebRequestBehaviour>();
        if (objs != null && objs.Count() > 0)
        {
            foreach (var item in objs)
            {
                UnityEngine.Object.DestroyImmediate(item.gameObject);
            }
        }
        persistedObj = new GameObject("[HMSWebRequestHelper]");
        behaviour = persistedObj.AddComponent<HMSWebRequestBehaviour>();
    }

    internal void PostRequest(string url, string bodyJsonString, Action<UnityWebRequest> callback)
    {
        behaviour.Post(url, bodyJsonString, callback);
    }

    internal async Task<UnityWebRequest> PostRequest(string url, string bodyJsonString)
    {
        return await behaviour.PostAsync(url, bodyJsonString);
    }

    internal void PostRequest(string url, string bodyJsonString, Dictionary<string, string> requestHeaders, Action<UnityWebRequest> callback)
    {
        behaviour.Post(url, bodyJsonString, requestHeaders, callback);
    }

    internal void GetRequest(string url, Dictionary<string, string> requestHeaders, Action<UnityWebRequest> callback)
    {
        behaviour.Get(url, requestHeaders, callback);
    }

    internal void PutRequest(string url, string bodyJsonString, Dictionary<string, string> requestHeaders, Action<UnityWebRequest> callback)
    {
        behaviour.Put(url, bodyJsonString, requestHeaders, callback);
    }

    internal void GetFile(string url, string path, Action<bool> result = null)
    {
        behaviour.GetFile(url, path, result);
    }
}

public class HMSWebRequestBehaviour : MonoBehaviour
{
    public void Post(string url, string bodyJsonString, Action<UnityWebRequest> callback)
    {
        StartCoroutine(PostCoroutine(url, bodyJsonString, callback));
    }

    public void Get(string url, Dictionary<string, string> requestHeaders, Action<UnityWebRequest> callback)
    {
        StartCoroutine(GetCoroutine(url, requestHeaders, callback));
    }

    public void Post(string url, string bodyJsonString, Dictionary<string, string> requestHeaders, Action<UnityWebRequest> callback)
    {
        StartCoroutine(PostCoroutine(url, bodyJsonString, requestHeaders, callback));
    }

    public void Put(string url, string bodyJsonString, Dictionary<string, string> requestHeaders, Action<UnityWebRequest> callback)
    {
        StartCoroutine(PutCoroutine(url, bodyJsonString, requestHeaders, callback));
    }

    public void GetFile(string url, string path, Action<bool> result = null)
    {
        StartCoroutine(GetFileCoroutine(url, path, result));
    }

    public async Task<UnityWebRequest> PostAsync(string url, string bodyJsonString)
    {
        return await PostAsync(url, bodyJsonString, null);
    }

    public async Task<UnityWebRequest> PostAsync(string url, string bodyJsonString, Dictionary<string, string> requestHeaders)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        if (requestHeaders != null)
        {
            foreach (var item in requestHeaders)
            {
                request.SetRequestHeader(item.Key, item.Value);
            }
        }
        var asyncOp = request.SendWebRequest();
        while (true)
        {
            if (asyncOp.progress == 1)
                break;
        }
        return request;
    }

    public async Task<UnityWebRequest> PutAsync(string url, string bodyJsonString, Dictionary<string, string> requestHeaders)
    {
        UnityWebRequest request = new UnityWebRequest(url, "PUT");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        if (requestHeaders != null)
        {
            foreach (var item in requestHeaders)
            {
                request.SetRequestHeader(item.Key, item.Value);
            }
        }
        var asyncOp = request.SendWebRequest();
        while (true)
        {
            if (asyncOp.progress == 1)
                break;
        }
        return request;
    }

    private IEnumerator PostCoroutine(string url, string bodyJsonString, Action<UnityWebRequest> callback)
    {
        yield return PostCoroutine(url, bodyJsonString, null, callback);
    }

    private IEnumerator PostCoroutine(string url, string bodyJsonString, Dictionary<string, string> requestHeaders, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            if (requestHeaders != null)
            {
                foreach (var item in requestHeaders)
                {
                    request.SetRequestHeader(item.Key, item.Value);
                }
            }
            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
         var requestError =
            request.result == UnityWebRequest.Result.ProtocolError ||
            request.result == UnityWebRequest.Result.ConnectionError;
#else
            bool requestError =
               request.isNetworkError ||
               request.isHttpError;
#endif

            if (requestError)
            {
                if (request.error == null)
                {
                    Debug.LogError("HMSWebRequestHelper encountered an unknown error");
                }
                else
                {
                    Debug.LogError("HMSWebRequestHelper encountered an error: " + request.error);
                }
                yield break;
            }

            callback(request);
        }
    }

    private IEnumerator PutCoroutine(string url, string bodyJsonString, Dictionary<string, string> requestHeaders, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "PUT"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            if (requestHeaders != null)
            {
                foreach (var item in requestHeaders)
                {
                    request.SetRequestHeader(item.Key, item.Value);
                }
            }
            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
         var requestError =
            request.result == UnityWebRequest.Result.ProtocolError ||
            request.result == UnityWebRequest.Result.ConnectionError;
#else
            bool requestError =
               request.isNetworkError ||
               request.isHttpError;
#endif

            if (requestError)
            {
                if (request.error == null)
                {
                    Debug.LogError("HMSWebRequestHelper encountered an unknown error");
                }
                else
                {
                    Debug.LogError("HMSWebRequestHelper encountered an error: " + request.error);
                }
                yield break;
            }

            callback(request);
        }
    }

    private IEnumerator GetCoroutine(string url, Dictionary<string, string> requestHeaders, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "GET"))
        {
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            if (requestHeaders != null)
            {
                foreach (var item in requestHeaders)
                {
                    request.SetRequestHeader(item.Key, item.Value);
                }
            }
            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
         var requestError =
            request.result == UnityWebRequest.Result.ProtocolError ||
            request.result == UnityWebRequest.Result.ConnectionError;
#else
            bool requestError =
               request.isNetworkError ||
               request.isHttpError;
#endif

            if (requestError)
            {
                if (request.error == null)
                {
                    Debug.LogError("HMSWebRequestHelper encountered an unknown error");
                }
                else
                {
                    Debug.LogError("HMSWebRequestHelper encountered an error: " + request.error);
                }
                yield break;
            }

            callback(request);
        }
    }

    private IEnumerator GetFileCoroutine(string url, string path, Action<bool> result = null)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "GET"))
        {
            request.downloadHandler = new DownloadHandlerFile(path, true);
            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
         var requestError =
            request.result == UnityWebRequest.Result.ProtocolError ||
            request.result == UnityWebRequest.Result.ConnectionError;
#else
            bool requestError =
               request.isNetworkError ||
               request.isHttpError;
#endif

            if (requestError)
            {
                if (request.error == null)
                {
                    Debug.LogError("HMSWebRequestHelper encountered an unknown error");
                }
                else
                {
                    Debug.LogError("HMSWebRequestHelper encountered an error: " + request.error);
                }
                result?.Invoke(false);
                yield break;
            }
            result?.Invoke(true);
        }
    }
}
