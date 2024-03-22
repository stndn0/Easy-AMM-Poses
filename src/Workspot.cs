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
        /// <param name="animSlot">Slot number for provided animations. Optional animations are loaded in slot 2. As two workspots can be created, the slot number determines the workspot number.</param>
        /// <returns></returns>
        public static async Task<int> BuildWorkspotJson(List<Pose> poseList, Config config, string femAnimPath, string manAnimPath, string femBigAnimPath, string manBigAnimPath, int animSlot) {
            if (femAnimPath == "" && manAnimPath == "" && femBigAnimPath == "" && manBigAnimPath == "")
            {
                Debug.WriteLine("DEBUG: Error. All animation paths are empty. Cannot build workspot!...");
                Debug.WriteLine("DEBUG: Cannot build workspot on slot: " + animSlot);
                return -1;
            }

            Debug.WriteLine("\n\nDEBUG: Building workspot JSON...");
            Debug.WriteLine("Female anim path: " + femAnimPath);
            Debug.WriteLine("Male anim path: " + manAnimPath);
            Debug.WriteLine("Female big anim path: " + femBigAnimPath);
            Debug.WriteLine("Man big anim path: " + manBigAnimPath);


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
            if (femAnimPath != "")
            {
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["finalAnimsets"][2]["animations"]["cinematics"][0]["animSet"]["DepotPath"]["$value"] = femAnimPath;
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["finalAnimsets"][2]["loadingHandles"][0]["DepotPath"]["$value"] = femAnimPath;
            }

            // Male average
            if (manAnimPath != "")
            {
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["finalAnimsets"][0]["animations"]["cinematics"][0]["animSet"]["DepotPath"]["$value"] = manAnimPath;
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["finalAnimsets"][0]["loadingHandles"][0]["DepotPath"]["$value"] = manAnimPath;
            }

            // Woman big
            if (femBigAnimPath != "") {
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["finalAnimsets"][3]["animations"]["cinematics"][0]["animSet"]["DepotPath"]["$value"] = femBigAnimPath;
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["finalAnimsets"][3]["loadingHandles"][0]["DepotPath"]["$value"] = femBigAnimPath;
            }

            // Male big
            if (manBigAnimPath != "")
            {
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["finalAnimsets"][1]["animations"]["cinematics"][0]["animSet"]["DepotPath"]["$value"] = manBigAnimPath;
                jsonWorkspotObj["Data"]["RootChunk"]["workspotTree"]["Data"]["finalAnimsets"][1]["loadingHandles"][0]["DepotPath"]["$value"] = manBigAnimPath;
            }



            foreach (Pose pose in poseList)
            {

                // Add a new pose entries to the workspot template json.
                Debug.WriteLine("POSE SLOT: " + pose.Slot + "ANIM SLOT: " + animSlot);

                // If the pose belongs to the current animation slot for the workspot then it is relevant to us.
                if (pose.Slot == animSlot)
                {
                    // Deserialize entry template in order to write to it.
                    dynamic jsonWorkspotListEntryObj = JsonConvert.DeserializeObject(File.ReadAllText(pathToWorkspotListEntry));
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


            }

            // Serialize and write updated data to the workspot JSON file.
            string output = JsonConvert.SerializeObject(jsonWorkspotObj, Formatting.Indented);
            string pathToOutput = "";

            if (animSlot == 1)
            {
                pathToOutput = config.getProjectControllerDirectory() + @"workspot_output.json";
                File.WriteAllText(pathToOutput, output);
                config.pathToWorkspotJson1 = pathToOutput;
                Debug.WriteLine("DEBUG: Build workspot1 complete...");
            }
            else if (animSlot == 2)
            {
                pathToOutput = config.getProjectControllerDirectory() + @"workspot_output02.json";
                File.WriteAllText(pathToOutput, output);
                config.pathToWorkspotJson2 = pathToOutput;
                Debug.WriteLine("DEBUG: Build workspot2 complete...");
            }

            return 1;
        }
    }
}
