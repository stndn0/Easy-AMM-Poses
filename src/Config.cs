using System.IO;


namespace Easy_AMM_Poses.src
{
    // Store user configuration settings.
    public class Config
    {
        Random rnd = new Random();                      // Used in method to set internal project name.

        // Project and general configuration settings
        public string cliPath = "";
        public string modFolderPath = "Select the path to your Cyberpunk 2077 mod folder";
        public string configFilePath = "config/config.json";
        public string luaCategoryName = "";
        public string projectUsername = "Johnny Silverhand";
        public string projectName = "";
        public string projectPath = "";
        public string internalProjectName = "";

        // Animation file paths - workspot #1
        public string animPathFemaleAvg = "";
        public string animPathFemaleBig = "";
        public string animJsonPathFemaleAvg = "";
        public string animJsonPathFemaleBig = "";
        public string animPathMaleAvg = "";
        public string animPathMaleBig = "";
        public string animJsonPathMaleAvg = "";
        public string animJsonPathMaleBig = "";
        public string pathToWorkspotJson1 = "";      
        public string pathToWorkspot1 = "";           // Path to final generated workspot file.
        public string pathToEntityJson1 = "";
        public string pathToEntity1 = "";             // Path to final generated entity file

        // Animation file paths - workspot #2
        public string pathToWorkspotJson2 = "";       
        public string pathToWorkspot2 = "";           // Path to final generated workspot file.
        public string pathToEntityJson2 = "";
        public string pathToEntity2 = "";             // Path to final generated entity file

        // Gender and rig types
        public string womanAverage = "WA";
        public string womanBig = "WB";
        public string manAverage = "MA";
        public string manBig = "MB";


        public void SetConfigFile(Config config)
        {
            // Create the configuration directory. If the folder already exists, it'll be ignored.
            Directory.CreateDirectory("config");
            Directory.CreateDirectory("temp");

            // Create mod directory
            //Directory.CreateDirectory(getProjectAnimsDirectory());

            // Store mod resources (.lua files)
            //Directory.CreateDirectory(getProjectResourcesDirectory());
            //Directory.CreateDirectory(@"projects\project1\resources\bin\x64\plugins\cyber_engine_tweaks\mods\AppearanceMenuMod\Collabs\Custom Poses\NAMEHERE");

            internalProjectName = "eap_" + rnd.Next(100000, 10000000);

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
            return @"projects\" + projectName + @"\" + projectName + @"\base\" + internalProjectName + @"\controller\";
        }

        public string getProjectResourcesDirectory()
        {
            return @"projects\" + projectName + @"\resources\";
        }


        public string getProjectAnimsDirectory()
        {
            return @"projects\" + projectName + @"\" + projectName + @"\base\" + internalProjectName + @"\controller\anims\";
        }

        public string getProjectRootDirectory()
        {
            return @"projects\" + projectName + @"\" + projectName;
        }

        /// <summary>
        /// For a given filepath, strip everything before the "base" substring.
        /// When we're generating JSON files, the filepaths contains extra folders that we don't need.
        /// Example: "projects\project1\project1\archive\base\testmod\controller\workspot_output.workspot"
        /// We want to remove "projects\project1\project1\archive\" from the filepath so that
        /// we have a relative path to the "base" folder for the mod.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string convertToRedengineFilepath(string filepath)
        {
            // Bug warning
            // If user project name contains "base" substring, this will return the wrong index.
            // This needs to be re-evaulated.

            string substring = @"base";

            // Return index of substring in filepath
            int index = filepath.IndexOf(substring);

            if (index >= 0)
            {
                return filepath.Substring(index);
            }

            return "null";
        }

        public bool checkIfAllAnimPathsEmpty(Config config)
        {
            if (string.IsNullOrEmpty(config.animPathFemaleAvg) && string.IsNullOrEmpty(config.animPathMaleAvg) && string.IsNullOrEmpty(config.animPathMaleBig) && string.IsNullOrEmpty(config.animPathFemaleBig))
            {
                // All paths are empty. The user hasn't selected any animation files.
                return true;
            }
            return false;
        }

        public void setInternalProjectName()
        {
            // Internal project name needs to be unique and random so that it 
            // doesn't conflict with other mods created by the user, or other
            // mods created by EAP.
            Random rnd = new Random();
            internalProjectName = "eap_" + rnd.Next(1, 20000);
        }
    }
}
