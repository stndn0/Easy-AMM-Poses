namespace Easy_AMM_Poses.src
{
    /// <summary>
    /// New class instances of type Pose are created for each pose that is detected in the 
    /// animation files that the user provides.
    public class Pose
    {
        public string Name { get; set; }
        public string BodyType { get; set; }

        // By default, users load poses into slot 1 (main four animation slots in the UI).
        // However, if the user clicks the optional arrow button to add more animation files,
        // then poses from these optional files are loaded into slot 2.
        // We need a way to determine which poses to load into which workspot.
        // Poses with slot 1 are for the first workspot, and slot 2 is for the second workspot.
        public int Slot { get; set; }

        // A pose name might belong to more than one body type.
        // E.g., both WA and MA have a pose with the same name ("Pose 1").
        public List<string> ExtraBodyTypes { get; set; }

        // Constructor that takes one parameter.
        public Pose(string name, string bodyType, int slot)
        {
            // "this" keyword references the current object (current instance of the class).
            this.Name = name;
            this.BodyType = bodyType;
            this.ExtraBodyTypes = new List<string>();
            this.Slot = slot;
        }
    }
}
