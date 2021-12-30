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

namespace HmsPlugin.ConnectAPI.ProjectManagement
{
    public class ObtainingAppBriefInfoEditor : VerticalSequenceDrawer
    {
        private AppType appTypeList;
        private string PackageName;
        private string teamID = "";
        private string appID = "";

        public ObtainingAppBriefInfoEditor()
        {
            //GetObtainingTeamList(OnObtainingTeamListResponse);
            PackageName = Application.identifier;
            //TODO opening project settings with SettingsService.OpenProjectSettings("Project/Player"); needed to be test on other versions of unity
            AddDrawer(new HorizontalLine());
            AddDrawer(new HorizontalSequenceDrawer(new Label.Label("PackageName:"), new Spacer(), new Clickable(new Label.Label(PackageName).SetBold(true), () => { SettingsService.OpenProjectSettings("Project/Player"); }), new Space(10)));
            OnObtainingTeamListResponse(new ObtainingTeamListResJson() { teams = new List<Team> { new Team() { countryCode = "1", id = "1", name = "1", siteId = 1, userType = 1 } }, ret = new Ret() { code = 0, msg = "" } });
        }

        private async void GetObtainingAppBriefInfo()
        {
            OnObtainingInfoResponse(new ObtainingAppInfoResJson() {appInfos = new List<AppBriefInfo> { new AppBriefInfo() { appID = "AppID", appName = "appName", appType = "1", packageName = "packageName", packageType = 1, projectId = "projectId", teamId = "teamId", prodSerialNo = 11 } }, ret = new Ret() { code = 0, msg = "" }, notExistIDs = "" });
           /* if (teamID != "")
            {
                string accessToken = await HMSWebUtils.GetAccessTokenAsync();
                HMSWebRequestHelper.GetRequest("https://connect-api.cloud.huawei.com/api/cds/app-distirbution/v1/all/app-brief-info/list?packageNames=" + PackageName,
                    new Dictionary<string, string>()
                    {
                    {"oauth2Token", "Bearer " + accessToken},
                    {"userID", teamID}
                    }, OnObtainingInfoResponse);
            }
            else
            {
                Debug.LogWarning("[HMS ConnectAPI] GetObtainingAppBriefInfo teamID is empty");
            }*/
        }

        private async void GetObtainingTeamList(Action<UnityWebRequest> callback) //TODO carry this method to public methods script
        {
            string accessToken = await HMSWebUtils.GetAccessTokenAsync();
            HMSWebRequestHelper.GetRequest("https://connect-api.cloud.huawei.com/api/ups/user-permission-service/v1/user-team-list",
                new Dictionary<string, string>()
                {
                    {"oauth2Token", "Bearer " + accessToken }
                }, callback);
        }

        private void OnObtainingTeamListResponse(ObtainingTeamListResJson a/*UnityWebRequest response*/)
        {
            var responseJson = a;//JsonUtility.FromJson<ObtainingTeamListResJson>(response.downloadHandler.text);

            if (responseJson.ret.code == 0)
            {
                if (responseJson.teams.Count > 0)
                {
                    teamID = responseJson.teams[0].id;
                    AddDrawer(new HmsPlugin.Space(10));
                    AddDrawer(new HmsPlugin.Space(10));
                    AddDrawer(new HorizontalSequenceDrawer(new Spacer(), new Button.Button("Obtain App Brief Info", GetObtainingAppBriefInfo).SetWidth(200), new Spacer()));
                    AddDrawer(new HorizontalLine());
                }
            }
        }

        private void OnObtainingInfoResponse(ObtainingAppInfoResJson a)//UnityWebRequest response)
        {
            //We are obtaining appBrief info with PackageName so response should have one or zero item in appInfos list.

            var responseJson = a;// JsonUtility.FromJson<ObtainingAppInfoResJson>(response.downloadHandler.text);

            if (responseJson.ret.code == 0)
            {
                Debug.Log($"[HMS ConnectAPI] ObtainingingAppInfo success. appInfos.size:{responseJson.appInfos.Count}");
                if (responseJson.appInfos.Count > 0)
                {
                    appID = responseJson.appInfos[0].appID;
                    AddDrawer(new HmsPlugin.Space(10));
                    AddDrawer(new HorizontalSequenceDrawer(new Label.Label("App ID:"), new Spacer(), new Label.Label(appID), new Space(10)));
                    AddDrawer(new HmsPlugin.Space(10));
                    AddDrawer(new HorizontalSequenceDrawer(new Label.Label("App Name:"), new Spacer(), new Label.Label(responseJson.appInfos[0].appName), new Space(10)));
                    AddDrawer(new HmsPlugin.Space(10));
                    AddDrawer(new HorizontalSequenceDrawer(new Label.Label("App Type:"), new Spacer(), new Label.Label(convertAppTypeInttoString(responseJson.appInfos[0].appType)), new Space(10)));
                    AddDrawer(new HmsPlugin.Space(10));
                    AddDrawer(new HorizontalSequenceDrawer(new Spacer(), new Button.Button("Obtain The Configuration File", GetObtainingConfigurationFile).SetWidth(200), new Spacer()));
                    AddDrawer(new HorizontalLine());
                }
                else
                {
                    Debug.LogWarning($"[HMS ConnectAPI] ObtainingingAppInfo appInfos is empty.");
                }
            }
            else
            {
                Debug.LogError($"[HMS ConnectAPI] ObtainingingAppInfo failed. Error Code: {responseJson.ret.code}, Error Message: {responseJson.ret.msg}.");
            }
        }
        private async void GetObtainingConfigurationFile()
        {
            /*
            //TODO in documents in table no headher but example there is and querry is different in example test link and headher
            if (appID != "")
            {
                string accessToken = await HMSWebUtils.GetAccessTokenAsync();
                HMSWebRequestHelper.GetRequest("https://connect-api.cloud.huawei.com/api/cpms/project-management-service/v1/config-file/" + appID,
                    new Dictionary<string, string>()
                    {
                    {"oauth2Token", "Bearer " + accessToken},
                    {"teamId", teamID}
                    }, OnObtainingConfigurationFileResponse);
            }
            else
            {
                Debug.LogWarning("[HMS ConnectAPI] GetObtainingConfigurationFile appID is empty");
            }*/
            OnObtainingConfigurationFileResponse(new ObtainingConfigurationFileResJson() { content = @"{
	""agcgw"":{
		""backurl"":""connect-dre.dbankcloud.cn"",
		""url"":""connect-dre.hispace.hicloud.com"",
		""websocketbackurl"":""connect-ws-dre.hispace.dbankcloud.cn"",
		""websocketurl"":""connect-ws-dre.hispace.dbankcloud.com""
	},
	""agcgw_all"":{
		""CN"":""connect-drcn.hispace.hicloud.com"",
		""CN_back"":""connect-drcn.dbankcloud.cn"",
		""DE"":""connect-dre.hispace.hicloud.com"",
		""DE_back"":""connect-dre.dbankcloud.cn"",
		""RU"":""connect-drru.hispace.hicloud.com"",
		""RU_back"":""connect-drru.dbankcloud.cn"",
		""SG"":""connect-dra.hispace.hicloud.com"",
		""SG_back"":""connect-dra.dbankcloud.cn""
	},
	""client"":{
		""cp_id"":""5190090000027631217"",
		""product_id"":""736430079245826044"",
		""client_id"":""660843898943325440"",
		""client_secret"":""15EC27CFA4DF1FC894835A8A9A25DAEC33B4168B46BCF3AE1227C677B86FDBE5"",
		""project_id"":""736430079245826044"",
		""app_id"":""104478135"",
		""api_key"":""CgB6e3x9JLiMjfCtUrE5ZOF87RwF0FMwuDA2+s3HscwlRqHm/iYroqHC4h1JDvY2OQgqECmYc7mQyiSoucwqJzGz"",
		""package_name"":""com.hms.codelab.tictactwo""
	},
	""oauth_client"":{
		""client_id"":""104478135"",
		""client_type"":1
	},
	""app_info"":{
		""app_id"":""104478135"",
		""package_name"":""com.hms.codelab.tictactwo""
	},
	""service"":{
		""analytics"":{
			""collector_url"":""datacollector-dre.dt.hicloud.com,datacollector-dre.dt.dbankcloud.cn"",
			""resource_id"":""p1"",
			""channel_id"":""""
		},
		""search"":{
			""url"":""https://search-dre.cloud.huawei.com""
		},
		""cloudstorage"":{
			""storage_url"":""https://ops-dre.agcstorage.link""
		},
		""ml"":{
			""mlservice_url"":""ml-api-dre.ai.dbankcloud.com,ml-api-dre.ai.dbankcloud.cn""
		}
	},
	""region"":""DE"",
	""configuration_version"":""3.0"",
	""appInfos"":[
		{
			""package_name"":""com.hms.codelab.tictactwo"",
			""client"":{
				""app_id"":""104478135""
			},
			""app_info"":{
				""package_name"":""com.hms.codelab.tictactwo"",
				""app_id"":""104478135""
			},
			""oauth_client"":{
				""client_type"":1,
				""client_id"":""104478135""
			}
		}
	]
}", ret = new Ret() { code = 0, msg = "" } });
        }
        private void OnObtainingConfigurationFileResponse(ObtainingConfigurationFileResJson a)//UnityWebRequest response)
        {
            var responseJson = a;// JsonUtility.FromJson<ObtainingConfigurationFileResJson>(response.downloadHandler.text);

            if (responseJson.ret.code == 0)
            {
                HMSEditorUtils.CreateAGConnectConfig(responseJson.content);
            }
        }
        private string convertAppTypeInttoString(string typeKey)
        {
            if (appTypeList == null) 
            {
                InitializeAppType();
            }

            foreach (var type in appTypeList.types)
            {
                if (type.Key == typeKey)
                {
                    return type.Value;
                }
            }
            return "";
        }

        #region JsonStuff

        [Serializable]
        private class Ret
        {
            public int code;
            public string msg;
        }

        

        [Serializable]
        private class ObtainingAppInfoResJson
        {
            public List<AppBriefInfo> appInfos;
            public string notExistIDs;
            public Ret ret;
        }

        [Serializable]
        private class ObtainingTeamListResJson
        {
            public List<Team> teams;
            public Ret ret;
        }

        [Serializable]
        private class ObtainingConfigurationFileResJson
        {
            public string fileName;
            public string content;
            public Ret ret;
        }


        [Serializable]
        private class Team
        {
            public string name;
            public string id;
            public string countryCode;

            public int userType;
            /*
             * Team account type.
             1: individual developer
             2: company developer
            */

            public int siteId;
            /*
                Site where the team owner account is located.
                1: China site
                5: Singapore site
                7: Germany site
                8: Russia site
            */
        }

        [Serializable]
        private class AppBriefInfo
        {
            public int prodSerialNo;
            public string appID;
            public string appName;
            public string projectId;
            public string appType;
            public string packageName;
            public string teamId;
            public int packageType;
            public List<DeviceTypeInfo> deviceTypes;
        }

        [Serializable]
        private class DeviceTypeInfo
        {
            public int deviceType;
            /*
               Device type.
                4: mobile phone
                6: VR gadget
                7: watch
                8: Vision
                9: router
                10: head unit
             */

            public string appAdapters;
            /*
               Compatible device type. Use commas (,) to separate multiple device types.
                1: Honor Cube (router)
                4: mobile phone
                5: tablet
                6: VR gadget
                7: watch (Android Wear)
                8: Vision
                9: router
                11: children's watch
                12: smart watch
                13: speaker with a screen
             */
        }


        private enum PackageType
        {
            none,
            APK,
            RPK,
            WPK,
            EXE
        }

        private class AppType
        {
            public List<KeyValuePair<string, string>> types;
        }

        private void InitializeAppType()
        {
            appTypeList = new AppType();

            appTypeList.types = new List<KeyValuePair<string, string>>();
            appTypeList.types.Add(new KeyValuePair<string, string>("1", "Mobile App"));
            appTypeList.types.Add(new KeyValuePair<string, string>("2", "Vision App"));
            appTypeList.types.Add(new KeyValuePair<string, string>("4", "Router App"));
            appTypeList.types.Add(new KeyValuePair<string, string>("8", "VR gadget App"));
            appTypeList.types.Add(new KeyValuePair<string, string>("9", "Watch App"));
            appTypeList.types.Add(new KeyValuePair<string, string>("19", "Quick App"));
            appTypeList.types.Add(new KeyValuePair<string, string>("23", "Vision Quick App"));
            appTypeList.types.Add(new KeyValuePair<string, string>("27", "Head Unit Native App"));
            appTypeList.types.Add(new KeyValuePair<string, string>("28", "Head Unit Quick App"));
            appTypeList.types.Add(new KeyValuePair<string, string>("29", "Sports Watch App"));
            appTypeList.types.Add(new KeyValuePair<string, string>("30", "Cloud game"));
            appTypeList.types.Add(new KeyValuePair<string, string>("9998", "iOS App"));
            appTypeList.types.Add(new KeyValuePair<string, string>("9999", "Web App"));
        }
        #endregion


    }
}
