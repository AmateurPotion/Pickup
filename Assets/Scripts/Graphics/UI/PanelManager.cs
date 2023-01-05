using System.Collections.Generic;
using Pickup.Graphics.UI.Panels;
using UnityEngine;

namespace Pickup.Graphics.UI
{
    public class PanelManager
    {
        public Canvas canvas;
        public readonly Dictionary<string, Panel> panelDic;
        public PanelManager()
        {
            panelDic = new Dictionary<string, Panel>();
        }
    }
}