using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;


namespace Easy_AMM_Poses.src
{
    class Entity
    {
        // TODO
        public static async Task<int> buildEntityJson(Config config)
        {
            Debug.WriteLine("DEBUG: Building entity JSON");

            // Load entity template and deserialize to dynamic object.
            string pathToEntityJsonTemplate = @"templates\entity_template.json";
            dynamic jsonEntityObj = JsonConvert.DeserializeObject(File.ReadAllText(pathToEntityJsonTemplate));

            //string path1 = JsonConvert.SerializeObject(jsonEntityObj["Data"]["RootChunk"]["compiledData"]["Data"]["Chunks"][4]["workspotResource"]["DepotPath"]["$value"], Formatting.Indented);

            //string path2 = JsonConvert.SerializeObject(jsonEntityObj["Data"]["RootChunk"]["components"][3]["workspotResource"]["DepotPath"]["$value"], Formatting.Indented);


            //string output = JsonConvert.SerializeObject(jsonEntityObj["Data"]["RootChunk"]["components"][3]["workspotResource"]["DepotPath"]["$value"], Formatting.Indented);


            // Update the ent file so that it points to the newly created workspot file.
            jsonEntityObj["Data"]["RootChunk"]["compiledData"]["Data"]["Chunks"][4]["workspotResource"]["DepotPath"]["$value"] = config.pathToWorkspotMFA;

            jsonEntityObj["Data"]["RootChunk"]["components"][3]["workspotResource"]["DepotPath"]["$value"] = config.pathToWorkspotMFA;


            // Serialize and write updated data to the entity JSON file.
            string output = JsonConvert.SerializeObject(jsonEntityObj, Formatting.Indented);
            string pathToOutput = config.getProjectControllerDirectory() + @"entity_output.json";
            File.WriteAllText(pathToOutput, output);
            config.pathToEntityJsonMFA = pathToOutput;
            Debug.WriteLine("DEBUG: Finished building entity JSON. Path: " + pathToOutput);


            return 1;
        }
    }
}
