namespace Easy_AMM_Poses.src
{
    /// <summary>
    /// New class instances of type Pose are created for each pose that is detected in the 
    /// animation files that the user provides.
    public class Pose
    {
        public string Name { get; set; }
        public string BodyType { get; set; }
    
        // Workspot slot. 1 = WS1, 2 = WS2
        // Does not need to be a list (this is a holdover from an old implementation where poses could have multiple slots)
        // Refactor to single int value eventually but for now it does not cause issues..
        public List<int> Slot = new List<int>();

        // Constructor that takes one parameter.
        public Pose(string name, string bodyType, int slot)
        {
            // "this" keyword references the current object (current instance of the class).
            this.Name = name;
            this.BodyType = bodyType;
            //this.ExtraBodyTypes = new List<string>();
            this.Slot.Add(slot);
        }
    }
}
