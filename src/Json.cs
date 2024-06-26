﻿using Newtonsoft.Json;
using System.IO;

namespace Easy_AMM_Poses.src
{

    class Json
    {
        // Write configuration data to JSON file
        public static void WriteConfigData(Config config)
        {
            using (StreamWriter streamWriter = new StreamWriter(config.configFilePath, false))
            {
                var jsonData = new
                {
                    cliPath = config.cliPath,
                    modFolderPath = config.modFolderPath,
                    projectUsername = config.projectUsername
                };

                // Write new data to the configuration file
                var serializedData = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
                streamWriter.WriteLine(serializedData);
                System.Diagnostics.Debug.WriteLine($"DEBUG: Updated {config.configFilePath} with data: {serializedData}");
            }
        }

        // Read configuration data from JSON file and update variables.
        public static void ReadConfigData(Config config)
        {
            var deserializedData = JsonConvert.DeserializeObject<Config>(File.ReadAllText(config.configFilePath));
            System.Diagnostics.Debug.WriteLine($"DEBUG: Read {config.configFilePath} with data: {deserializedData.cliPath}, {deserializedData.modFolderPath}");

            config.cliPath = deserializedData.cliPath;
            config.modFolderPath = deserializedData.modFolderPath;
            config.projectUsername = deserializedData.projectUsername;
        }
    }
}
