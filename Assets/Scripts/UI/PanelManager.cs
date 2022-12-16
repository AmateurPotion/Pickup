using System.Collections.Generic;

namespace Pickup.UI
{
    public class PanelManager
    {
        public readonly Dictionary<string, Panel> panelDic;
        public Panel setting { get; internal set; }
        public PanelManager()
        {
            panelDic = new Dictionary<string, Panel>();
        }
    }
}