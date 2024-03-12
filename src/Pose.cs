using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy_AMM_Poses.src
{
    /// <summary>
    /// Store the pose data from animation JSON file.
    /// </summary>
    public class Pose
    {
        public string Name { get; set; }
        public string BodyType { get; set; }

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
