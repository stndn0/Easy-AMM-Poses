using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;


namespace Easy_AMM_Poses.src
{

    /// <summary>
    /// Class pertaining to the creation and manipulation of .ent files.
    /// </summary>
    class Entity
    {
        // Builds the entity JSON file via a JSON template and the path to the workspot file.
        public static async Task<int> buildEntityJson(Config config, int fileNumber, String pathToWorkspot)
        {
            Debug.WriteLine("DEBUG: Building entity JSON");
            //Debug.WriteLine("DEBUG: Path to workspot is " + pathToWorkspot);

            // Load entity template and deserialize to dynamic object.
            string pathToEntityJsonTemplate = @"templates\entity_template.json";
            dynamic jsonEntityObj = JsonConvert.DeserializeObject(File.ReadAllText(pathToEntityJsonTemplate));

            // Update the ent file so that it points to the newly created workspot file.
            jsonEntityObj["Data"]["RootChunk"]["compiledData"]["Data"]["Chunks"][4]["workspotResource"]["DepotPath"]["$value"] = pathToWorkspot;

            jsonEntityObj["Data"]["RootChunk"]["components"][3]["workspotResource"]["DepotPath"]["$value"] = pathToWorkspot;

            // Serialize and write updated data to the entity JSON file.
            string output = JsonConvert.SerializeObject(jsonEntityObj, Formatting.Indented);
            string pathToOutput = config.getProjectControllerDirectory() + @"entity_01.json";

            if (fileNumber == 1)
            {
                File.WriteAllText(pathToOutput, output);
                config.pathToEntityJson1 = pathToOutput;
                Debug.WriteLine("DEBUG: Finished building entity JSON. Path: " + pathToOutput);
            }
            else if (fileNumber == 2)
            {
                pathToOutput = config.getProjectControllerDirectory() + @"entity_02.json";
                File.WriteAllText(pathToOutput, output);
                config.pathToEntityJson2 = pathToOutput;
                Debug.WriteLine("DEBUG: Finished building entity JSON. Path: " + pathToOutput);
            }
            return 1;
        }
    }
}
