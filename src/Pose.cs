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

        // Constructor that takes one parameter.
        public Pose(string name)
        {
            // "this" keyword references the current object (current instance of the class).
            this.Name = name;
        }
    }


}
