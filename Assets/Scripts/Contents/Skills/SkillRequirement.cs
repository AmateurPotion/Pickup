using System;

namespace Pickup.Contents.Skills
{
    [Serializable]
    public class SkillRequirement
    {
        public readonly Func<bool> checker;
        
        public SkillRequirement(Func<bool> checker)
        {
            this.checker = checker;
        }
    }
}