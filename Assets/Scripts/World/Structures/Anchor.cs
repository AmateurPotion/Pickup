namespace Pickup.World.Structures
{
    public class Anchor : Structure
    {
        public int range;
        
        public Anchor(string name, int range) : base(name)
        {
            this.range = range;
        }
    }
}