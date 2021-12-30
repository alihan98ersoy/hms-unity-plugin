using HmsPlugin;
using HmsPlugin.Window;
using UnityEditor;
using UnityEngine;
public class HMSQuerryingServiceEnablingStatusWindow : HMSEditorWindow
{
    [MenuItem("Huawei/Connect API/Project Management API/Querrying Service Enabling Status")]
    public static void ShowWindow()
    {
        string PackageName = Application.identifier;
        HelpboxAGConnectFile helpboxAGConnectFile = new HelpboxAGConnectFile();
        if (helpboxAGConnectFile.hasAGConnectFile)
        {
            if (PackageName.Equals(HMSEditorUtils.GetAGConnectConfig().client.package_name))
            {
                GetWindow(typeof(HMSQuerryingServiceEnablingStatusWindow), false, "Querrying Service Enabling Status");
            }
            else
            {
                //Package name is different
                EditorUtility.DisplayDialog("Error", "Package name is different with your agconnect-services.json's package_name.", "OK");
            }
        }
        else 
        {
            //agconnect-services.json file is missing
            EditorUtility.DisplayDialog("Error", helpboxAGConnectFile.errorMessage, "OK");
        }

    }


    public override IDrawer CreateDrawer()
    {
        var tabBar = new TabBar();
        HMSProjectManamentAPITabFactory.QuerryingServiceEnablingStatusTab(tabBar);
        return tabBar;
    }
}
