using System;

namespace Pickup.Scenes.LobbyScene
{
    [Serializable]
    public class FieldConstructData
    {
        public string name;
        public bool create;
        public DateTime createTime;
        
        public FieldConstructData(string name)
        {
            this.name = name;
        }

        public void Save()
        {
            
        }

        public static bool Load(string path, out FieldConstructData data)
        {
            data = null;
            
            return false;
        }
    }
}