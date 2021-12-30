using HmsPlugin;
using HmsPlugin.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

public class HMSObtainingAppBriefInfoWindow : HMSEditorWindow
{
    [MenuItem("Huawei/Connect API/Project Management API/Obtaining App Brief Info")]
    public static void ShowWindow()
    {
        GetWindow(typeof(HMSObtainingAppBriefInfoWindow), false, "Obtaining App Brief Info");
    }


    public override IDrawer CreateDrawer()
    {
        var tabBar = new TabBar();
        HMSProjectManamentAPITabFactory.ObtainingAppBriefInfoTab(tabBar);
        return tabBar;
    }
}
