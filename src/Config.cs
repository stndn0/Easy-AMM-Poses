using System.IO;


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

            // Create mod directory
            Directory.CreateDirectory(getProjectAnimsDirectory());

            // Store mod resources (.lua files)
            Directory.CreateDirectory(getProjectResourcesDirectory());
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

        public string getProjectControllerDirectory()
        {
            return @"projects\" + projectName + @"\" + projectName + @"\archive\base\" + internalProjectName + @"\controller\";
        }

        public string getProjectResourcesDirectory()
        {
            return @"projects\" + projectName + @"\resources\";
        }

        public string getProjectAnimsDirectory()
        {
            return @"projects\" + projectName + @"\" + projectName + @"\archive\base\" + internalProjectName + @"\controller\anims\";
        }
    }
}
