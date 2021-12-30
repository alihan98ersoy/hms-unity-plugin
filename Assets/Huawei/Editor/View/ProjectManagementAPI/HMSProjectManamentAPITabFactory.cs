using HmsPlugin;
using HmsPlugin.ConnectAPI.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class HMSProjectManamentAPITabFactory
{
    public static TabView ObtainingAppBriefInfoTab(TabBar tabBar)
    {
        var tab = new TabView("Obtaining App Brief Info Tab");
        tabBar.AddTab(tab);
        tab.AddDrawer(new ObtainingAppBriefInfoEditor());
        return tab;
    }
    public static TabView QuerryingServiceEnablingStatusTab(TabBar tabBar)
    {
        var tab = new TabView("Querrying Service Enabling Status");
        tabBar.AddTab(tab);
        tab.AddDrawer(new QuerryingServiceEnablingStatusEditor());
        return tab;
    }
}

