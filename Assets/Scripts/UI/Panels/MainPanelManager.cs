namespace Pickup.UI.Panels
{
    public class MainPanelManager : PanelManager
    {
        public Panel setting
        {
            get => panelDic["setting"];
            set => panelDic["setting"] = value;
        }
        
        public MainPanelManager() : base()
        {
        }
    }
}