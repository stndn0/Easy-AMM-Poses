namespace Easy_AMM_Poses.src
{
    /// <summary>
    /// New class instances of type Pose are created for each pose that is detected in the 
    /// animation files that the user provides.
    public class Pose
    {
        public string Name { get; set; }
        public string BodyType { get; set; }
    
        // Workspot slot/file that this pose belongs to. 1 = WS1, 2 = WS2
        public int Slot { get; set; }

        // Constructor that takes one parameter.
        public Pose(string name, string bodyType, int slot)
        {
            // "this" keyword references the current object (current instance of the class).
            this.Name = name;
            this.BodyType = bodyType;
            this.Slot = slot;

            //this.ExtraBodyTypes = new List<string>();
            //this.Slot.Add(slot);
        }
    }
}
