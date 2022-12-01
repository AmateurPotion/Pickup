using System;
using UnityEngine;

namespace Pickup.Contents
{
    [Serializable]
    public sealed class ContentManager
    {
        public static ContentManager instance { get; } = new ();
        // Skill
        // test

        private ContentManager()
        {
            
        }
    }
}