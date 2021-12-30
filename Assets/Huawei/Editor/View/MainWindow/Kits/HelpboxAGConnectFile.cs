using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HmsPlugin
{
    internal class HelpboxAGConnectFile : IDrawer
    {
        private HelpBox.HelpBox _helpBox;

        public bool hasAGConnectFile;

        public string errorMessage = "Please import your agconnect-services.json file to StreamingAssets folder";

        public HelpboxAGConnectFile()
        {
            _helpBox = new HelpBox.HelpBox(errorMessage, UnityEditor.MessageType.Error);
            hasAGConnectFile = File.Exists(Application.streamingAssetsPath + "/agconnect-services.json");
        }

        public void Draw()
        {
            if (!hasAGConnectFile)
                _helpBox.Draw();
        }
    }
}
