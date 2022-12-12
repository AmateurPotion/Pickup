using System;
using Pickup.Players;

namespace Pickup.Contents.Skills
{
    [Serializable]
    public class Skill
    {
        public string name;
        public SkillRequirement[] requirements;
        private Action<Skill> seq;
        
        public Skill(string name, Action<Skill> sequence, params SkillRequirement[] requirements)
        {
            this.name = name;
            seq = sequence;
            this.requirements = requirements;
        }

        public void Use(Player player)
        {
            
        }
    }
}