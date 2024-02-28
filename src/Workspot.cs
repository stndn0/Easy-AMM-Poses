using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Easy_AMM_Poses.src
{
    class Workspot
    {
        /// <summary>
        /// Build and populates the workspot JSON file using the users pose data.
        /// 
        /// TODO: Change absolute paths to relative paths.
        /// </summary>
        /// <param name="poses">
        /// </param>
        /// 
        public static void BuildWorkspotJson(List<Pose> poseList) {
            // Load core workspot file and deserialize to dynamic object.
            string pathToWorkspotJson = @"C:\Home\Development\repos\csharp\Easy AMM Poses\templates\workspot.json";
            dynamic jsonWorkspotObj = JsonConvert.DeserializeObject(File.ReadAllText(pathToWorkspotJson));

            // Load workspot list entry file. We'l use this as a template to add new entries to the workspot JSON file.
            string pathToWorkspotListEntry = @"C:\Home\Development\repos\csharp\Easy AMM Poses\templates\workspot_list_entry.json";

            // For each pose, add a new entry to the workspot JSON file
            // Start at id 6 because the workspot usually already has 5 entries.
            int currentId = 6;

            foreach (Pose pose in poseList)
            {
                // Deserialize entry template in order to write to it.
                dynamic jsonWorkspotListEntryObj = JsonConvert.DeserializeObject(File.ReadAllText(pathToWorkspotListEntry));

                // Add a new pose entries to the workspot template json.
                jsonWorkspotListEntryObj["Data"]["idleAnim"]["$value"] = pose.Name;
                jsonWorkspotListEntryObj["Data"]["list"][0]["Data"]["animName"]["$value"] = pose.Name;

                // Update id fields. Incremented the nested list field.
                jsonWorkspotListEntryObj["HandleId"] = currentId.ToString();
                jsonWorkspotListEntryObj["Data"]["list"][0]["HandleId"] = (currentId + 1).ToString();

                jsonWorkspotListEntryObj["Data"]["id"]["id"] = currentId;
                jsonWorkspotListEntryObj["Data"]["list"][0]["Data"]["id"]["id"] = currentId + 1;

                // Increment counter for the next iteration.
                currentId += 2;

                // Merge template with the main workspot json.
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["rootEntry"]["Data"]["list"].Add(jsonWorkspotListEntryObj);
            }

            // Serialize and write updated data to the workspot JSON file.
            string output = JsonConvert.SerializeObject(jsonWorkspotObj, Formatting.Indented);
            string pathToOutput = @"C:\Home\Development\repos\csharp\Easy AMM Poses\templates\workspot_output.json";
            File.WriteAllText(pathToOutput, output);


        }
    }
}
