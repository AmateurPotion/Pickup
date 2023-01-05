using System;

namespace Pickup.FileSystem
{
    public class SaveData
    {
        public readonly Guid guid;
        
        public SaveData()
        {
            guid = Guid.NewGuid();
        }

    }
}