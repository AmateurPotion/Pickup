using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Pickup.FileSystem
{
    // ReSharper disable once InconsistentNaming
    public sealed class FSManager
    {
        public readonly string root;
        
        public FSManager()
        {
            root = Application.isEditor ? Application.dataPath : "";
        }

        public void Init()
        {
            foreach (var dir in new[]
                     {
                         new DirectoryInfo(Path.Combine(root, "saves", "worlds")),
                         new DirectoryInfo(Path.Combine(root, "saves", "characters"))
                     })
            {
                if(!dir.Exists) dir.Create();
            }
        }
    }
}