using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;


namespace Easy_AMM_Poses.src
{
    class Workspot
    {
        /// <summary>
        /// Build and populates the workspot JSON file using the users pose data.
        /// </summary>
        /// <param name="poseList">List of poses.</param>
        /// <param name="config">Configuration object.</param>
        /// <returns></returns>
        public static async Task<int> BuildWorkspotJson(List<Pose> poseList, Config config) {
            Debug.WriteLine("DEBUG: Building workspot JSON...");
            // Load core workspot file and deserialize to dynamic object.
            string pathToWorkspotJson = @"templates\workspot.json";
            dynamic jsonWorkspotObj = JsonConvert.DeserializeObject(File.ReadAllText(pathToWorkspotJson));

            // Load workspot list entry file. We'l use this as a template to add new entries to the workspot JSON file.
            string pathToWorkspotListEntry = @"templates\workspot_list_entry.json";

            // For each pose, add a new entry to the workspot JSON file
            // Start at id 6 because the workspot usually already has 5 entries.
            int currentId = 6;

            // Update the "rig" fields depending on the animations that the user has provided
            // Woman average
            if (config.animPathFemaleAvg != "")
            {
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["finalAnimsets"][2]["animations"]["cinematics"][0]["animSet"]["DepotPath"]["$value"] = config.animPathFemaleAvg;
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["finalAnimsets"][2]["loadingHandles"][0]["DepotPath"]["$value"] = config.animPathFemaleAvg;
            }

            // Male average
            if (config.animPathMaleAvg != "")
            {
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["finalAnimsets"][0]["animations"]["cinematics"][0]["animSet"]["DepotPath"]["$value"] = config.animPathMaleAvg;
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["finalAnimsets"][0]["loadingHandles"][0]["DepotPath"]["$value"] = config.animPathMaleAvg;
            }

            // Woman big
            if (config.animPathFemaleBig != "") {
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["finalAnimsets"][3]["animations"]["cinematics"][0]["animSet"]["DepotPath"]["$value"] = config.animPathFemaleBig;
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["finalAnimsets"][3]["loadingHandles"][0]["DepotPath"]["$value"] = config.animPathFemaleBig;
            }

            // Male big
            if (config.animPathMaleBig != "")
            {
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["finalAnimsets"][1]["animations"]["cinematics"][0]["animSet"]["DepotPath"]["$value"] = config.animPathMaleBig;
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["finalAnimsets"][1]["loadingHandles"][0]["DepotPath"]["$value"] = config.animPathMaleBig;
            }



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
            string pathToOutput = config.getProjectControllerDirectory() + @"workspot_output.json";


            File.WriteAllText(pathToOutput, output);
            config.pathToWorkspotJsonMFA = pathToOutput;
            Debug.WriteLine("DEBUG: Build complete...");
            return 1;
        }
    }
}
