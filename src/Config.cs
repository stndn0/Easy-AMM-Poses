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
        public string pathToWorkspotMFA = "";           // Path to final generated workspot file.
        public string pathToEntityJsonMFA = "";
        public string pathToEntityMFA = "";             // Path to final generated entity file

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

        /// <summary>
        /// For a given filepath, strip everything before the base" substring.
        /// When we're generating JSON files, the filepaths contains extra folders that we don't need.
        /// Example: "projects\project1\project1\archive\base\testmod\controller\workspot_output.workspot"
        /// We want to remove "projects\project1\project1\archive\" from the filepath so that
        /// we have a relative path to the "base" folder for the mod.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string convertToRedengineFilepath(string filepath)
        {
            string substring = @"base";

            // Return index of substring in filepath
            int index = filepath.IndexOf(substring);

            if (index >= 0)
            {
                return filepath.Substring(index);
            }

            return "null";
        }
    }
}
