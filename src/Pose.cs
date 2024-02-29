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

        //public string Female = "WA";
        //public string Male = "MA";

        // Constructor that takes one parameter.
        public Pose(string name, string bodyType)
        {
            // "this" keyword references the current object (current instance of the class).
            this.Name = name;
            this.BodyType = bodyType;
        }
    }


}
