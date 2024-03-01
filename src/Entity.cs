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

            string path1 = JsonConvert.SerializeObject(jsonEntityObj["Data"]["RootChunk"]["compiledData"]["Data"]["Chunks"][4]["workspotResource"]["DepotPath"]["$value"], Formatting.Indented);

            string path2 = JsonConvert.SerializeObject(jsonEntityObj["Data"]["RootChunk"]["components"][3]["workspotResource"]["DepotPath"]["$value"], Formatting.Indented);


            string output = JsonConvert.SerializeObject(jsonEntityObj["Data"]["RootChunk"]["components"][3]["workspotResource"]["DepotPath"]["$value"], Formatting.Indented);
            Debug.WriteLine(output);


            return 1;
        }
    }
}
