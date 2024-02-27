using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy_AMM_Poses.src
{
    class Workspot
    {
        /// <summary>
        /// Build and populates the workspot JSON file using the users pose data.
        /// </summary>
        public static void BuildWorkspotJson() {
            string pathToWorkspotJson = @"C:\Users\stndn\Documents\pwa_workspot.workspot.json";
            dynamic jsonWorkspotObj = JsonConvert.DeserializeObject(File.ReadAllText(pathToWorkspotJson));
            jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["rootEntry"]["Data"]["list"].Add("test");

            string output = JsonConvert.SerializeObject(jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["rootEntry"]["Data"]["list"], Formatting.Indented);
            Debug.WriteLine(output);


        }
    }
}
