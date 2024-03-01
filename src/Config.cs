using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy_AMM_Poses.src
{
    // Store user configuration settings.
    public class Config
    {
        public string cliPath = "Select the path to your WolvenKit CLI";
        public string modFolderPath = "Select the path to your Cyberpunk 2077 mod folder";
        public string configFilePath = "config/config.json";
        public string animPathFemaleAvg = "";
        public string animJsonPathFemaleAvg = "";
        public string animPathMaleAvg = "";
        public string animJsonPathMaleAvg = "";
        public string pathToWorkspotJsonMFA = "";       // The workspot for Male/Fem Average is shared.

        // Hard coded for now. 
        public string projectName = "project1";
        public string projectPath = "projects/project1";
        public string internalProjectName = "testmod";

        public void SetConfigFile(Config config)
        {
            // Create the configuration directory. If the folder already exists, it'll be ignored.
            Directory.CreateDirectory("config");
            Directory.CreateDirectory("temp");

            // Store raw mod files
            //Directory.CreateDirectory(@"projects\project1\project1\archive\base\testmod\controller");

            Directory.CreateDirectory(@"projects\" + projectName + @"\" + projectName + @"\archive\base\" + internalProjectName + @"\controller");

            // Store mod resources (.lua files)
            //Directory.CreateDirectory(@"projects\project1\resources\");
            Directory.CreateDirectory(@"projects\" + projectName + @"\resources\");

            //Directory.CreateDirectory(@"projects\project1\resources\bin\x64\plugins\cyber_engine_tweaks\mods\AppearanceMenuMod\Collabs\Custom Poses\NAMEHERE");

            // If the config file doesn't exist, create it.
            if (!File.Exists(config.configFilePath))
            {
                File.Create(config.configFilePath).Close();
                Json.WriteConfigData(config);
            }
            else
            // If the file already exists, read it's properties and update the variables.
            {
                Json.ReadConfigData(config);
            }
        }
    }


}
