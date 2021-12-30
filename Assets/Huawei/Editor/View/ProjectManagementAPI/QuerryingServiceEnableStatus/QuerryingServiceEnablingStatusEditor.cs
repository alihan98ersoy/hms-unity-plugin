using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using HmsPlugin;
using HmsPlugin.Button;
using HmsPlugin.ConnectAPI;
using HmsPlugin.Label;
using System.IO;
public class QuerryingServiceEnablingStatusEditor : VerticalSequenceDrawer
{
    private string PackageName;
    public QuerryingServiceEnablingStatusEditor()
    {
        //GetObtainingTeamList(OnObtainingTeamListResponse);
        
    }
    private async void GetQueryingServiceEnablingStatus()
    {
            string accessToken = await HMSWebUtils.GetAccessTokenAsync();
            HMSWebRequestHelper.GetRequest("https://connect-api.cloud.huawei.com/api/cpms/project-service/v1/services/order-api?projectId=" + HMSEditorUtils.GetAGConnectConfig().client.project_id + "&appID=" + HMSEditorUtils.GetAGConnectConfig().client.app_id,
                new Dictionary<string, string>()
                {
                    {"oauth2Token", "Bearer " + accessToken}
                }, OnQueryingServiceEnablingStatusResponse);
    }
    private void OnQueryingServiceEnablingStatusResponse(UnityWebRequest response)
    {
        var responseJson = JsonUtility.FromJson<QueryingServiceEnablingStatusResJson>(response.downloadHandler.text);

        if (responseJson.ret.code == 0)
        {

        }
    }

    #region JsonStuff

    [Serializable]
    private class Ret
    {
        public int code;
        public string msg;
    }

    [Serializable]
    private class QueryingServiceEnablingStatusResJson
    {
        public string siteId;
        /*
           Site ID. The options are as follows:
            SG: Singapore site
            DE: Germany site
            RU: Russia site
            CN: China site
         */

        public List<ServiceOrderAndApiStatus> serviceInfoList;
        public Ret ret;
    }

    [Serializable]
    private class ServiceOrderAndApiStatus
    {
        public string serviceName;
        public int serviceStatus;
        /*
           Enabling status. The options are as follows:
            0: enabled
            1: disabled
            2: enabled again
            3: not requiring enabling in AppGallery Connect
         */

        public ServiceDisplayName serviceDisplayName;
        public string apiName;
        public int apiStatus;
        /*
         API enabling status.
            0: disabled
            1: enabled
         */
    }

    [Serializable]
    private class ServiceDisplayName
    {
        public string CN;
        public string EN;
    }

    #endregion
}
