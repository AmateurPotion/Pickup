using System.Collections.Generic;
using UnityEngine;

namespace Pickup.UI
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