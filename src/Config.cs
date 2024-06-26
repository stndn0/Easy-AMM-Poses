﻿using System.Diagnostics;
using System.IO;


namespace Easy_AMM_Poses.src
{
    /// <summary>
    /// Class pertaining to the configuration settings of the application.
    /// The "memory" of the application. A lot of key variables are stored here.
    /// </summary>
    public class Config
    {
        // **********************************************************
        // **********************************************************
        //                  Key application variables
        // **********************************************************
        // **********************************************************

        Random rnd = new Random();                      // Used in method to set internal project name.
        int randMin = 10000000;
        int randMax = 100000000;

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
        public string animPathFemaleAvg2 = "";
        public string animPathFemaleBig2 = "";
        public string animJsonPathFemaleAvg2 = "";
        public string animJsonPathFemaleBig2 = "";
        public string animPathMaleAvg2 = "";
        public string animPathMaleBig2 = "";
        public string animJsonPathMaleAvg2 = "";
        public string animJsonPathMaleBig2 = "";

        // Gender and rig types
        public string womanAverage = "WA";
        public string womanBig = "WB";
        public string manAverage = "MA";
        public string manBig = "MB";


        /// <summary>
        /// Set the configuration json file for the application.
        /// The configuration file stores the user's CLI path and username and is persistent.
        /// </summary>
        /// <param name="config"></param>
        public void SetConfigFile(Config config)
        {
            // Create the configuration directory. If the folder already exists, it'll be ignored.
            Directory.CreateDirectory("config");
            Directory.CreateDirectory("temp");

            internalProjectName = "eap_" + rnd.Next(randMin, randMax);

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

        public void setInternalProjectName()
        {
            // Internal project name needs to be unique and random so that it 
            // doesn't conflict with other mods created by the user, or other
            // mods created by EAP.
            Random rnd = new Random();
            internalProjectName = "eap_" + rnd.Next(1, 20000);
        }


        // **********************************************************
        //              Key methods pertaining to filepaths
        // **********************************************************

        public string getProjectPackedDirectory()
        {
            return @"projects\" + projectName + "_" + internalProjectName + @"\## Packed Folder\archive\pc\mod\";
        }

        public string getProjectControllerDirectory()
        {
            return @"projects\" + projectName + "_" + internalProjectName + @"\" + projectName + @"\base\" + internalProjectName + @"\controller\";
        }

        public string getProjectResourcesDirectory()
        {
            return @"projects\" + projectName + "_" + internalProjectName + @"\resources\";
        }

        public string getProjectAnimsDirectory()
        {
            return @"projects\" + projectName + "_" + internalProjectName + @"\" + projectName + @"\base\" + internalProjectName + @"\controller\anims\";
        }

        public string getProjectRootDirectory()
        {
            return @"projects\" + projectName + "_" + internalProjectName + @"\" + projectName;
        }

        public string getProjectDirectory()
        {
            return @"projects\" + projectName + "_" + internalProjectName + @"\";
        }

        public string getPathToPackedArchive()
        {
            return @"projects\" + projectName + "_" + internalProjectName + @"\" + projectName + ".archive";
        }

        public string getProjectLuaDirectory()
        {
            return @"projects\" + projectName + "_" + internalProjectName + @"\## Packed Folder\bin\x64\plugins\cyber_engine_tweaks\mods\AppearanceMenuMod\Collabs\Custom Poses\" + projectName + "_" + internalProjectName;
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

        /// <summary>
        /// Check if all animation paths are empty.
        /// </summary>
        /// <param name="config"></param>
        /// <returns>true if all paths empty, else false.</returns>
        public bool checkIfAllAnimPathsEmpty(Config config)
        {
            // At least one animation path must be provided.
            bool allPathsEmpty = true;

            if (!string.IsNullOrEmpty(config.animPathFemaleAvg))  {
                allPathsEmpty = false;
            }
            else if (!string.IsNullOrEmpty(config.animPathMaleAvg))
            {
                allPathsEmpty = false;
            }
            else if (!string.IsNullOrEmpty(config.animPathMaleBig))
            {
                allPathsEmpty = false;
            }
            else if (!string.IsNullOrEmpty(config.animPathFemaleBig))
            {
                allPathsEmpty = false;
            }
            else if (!string.IsNullOrEmpty(config.animPathFemaleAvg2))
            {
                allPathsEmpty = false;
            }
            else if (!string.IsNullOrEmpty(config.animPathMaleAvg2))
            {
                allPathsEmpty = false;
            }
            else if (!string.IsNullOrEmpty(config.animPathMaleBig2))
            {
                allPathsEmpty = false;
            }
            else if (!string.IsNullOrEmpty(config.animPathFemaleBig2))
            {
                allPathsEmpty = false;
            }

            if (allPathsEmpty)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Check if optional animations (workspot 2) are provided. 
        /// </summary>
        /// <param name="config"></param>
        /// <returns>true if optional animations (workspot 2) are provided. Else false.</returns>
        public bool checkIfOptionalAnimsProvided(Config config)
        {
            if (string.IsNullOrEmpty(config.animPathFemaleAvg2) && string.IsNullOrEmpty(config.animPathMaleAvg2) && string.IsNullOrEmpty(config.animPathFemaleBig2) && string.IsNullOrEmpty(config.animPathMaleBig2))
            {
                return false;
            }
            return true;
        }


        // **********************************************************
        // Handle variables for woman animations (WA1, WA2, WB1, WB2)
        // **********************************************************
        public void setFemaleAvgAnimation1(Config config, string filepath)
        {
            Debug.WriteLine("Updated female avg animation path: " + filepath);
            config.animPathFemaleAvg = filepath;
            config.animJsonPathFemaleAvg = config.getProjectAnimsDirectory() + Path.GetFileName(filepath) + ".json";
        }

        public void resetFemaleAvgAnimation1(Config config)
        {
            config.animPathFemaleAvg = "";
            config.animJsonPathFemaleAvg = "";
        }

        public void setFemaleAvgAnimation2(Config config, string filepath)
        {
            config.animPathFemaleAvg2 = filepath;
            config.animJsonPathFemaleAvg2 = config.getProjectAnimsDirectory() + Path.GetFileName(filepath) + ".json";
        }

        public void resetFemaleAvgAnimation2(Config config)
        {
            config.animPathFemaleAvg2 = "";
            config.animJsonPathFemaleAvg2 = "";
        }
        public void setFemaleBigAnimation1(Config config, string filepath)
        {
            config.animPathFemaleBig = filepath;
            config.animJsonPathFemaleBig = config.getProjectAnimsDirectory() + Path.GetFileName(filepath) + ".json";
        }

        public void resetFemaleBigAnimation1(Config config)
        {
            config.animPathFemaleBig = "";
            config.animJsonPathFemaleBig = "";
        }

        public void setFemaleBigAnimation2(Config config, string filepath)
        {
            config.animPathFemaleBig2 = filepath;
            config.animJsonPathFemaleBig2 = config.getProjectAnimsDirectory() + Path.GetFileName(filepath) + ".json";
        }

        public void resetFemaleBigAnimation2(Config config)
        {
            config.animPathFemaleBig2 = "";
            config.animJsonPathFemaleBig2 = "";
        }

        // **********************************************************
        // Handle variables for man animations (MA1, MA2, MB1, MB2)
        // **********************************************************

        public void setMaleAvgAnimation1(Config config, string filepath)
        {
            config.animPathMaleAvg = filepath;
            config.animJsonPathMaleAvg = config.getProjectAnimsDirectory() + Path.GetFileName(filepath) + ".json";
        }

        public void resetMaleAvgAnimation1(Config config)
        {
            config.animPathMaleAvg = "";
            config.animJsonPathMaleAvg = "";
        }

        public void setMaleBigAnimation1(Config config, string filepath)
        {
            config.animPathMaleBig = filepath;
            config.animJsonPathMaleBig = config.getProjectAnimsDirectory() + Path.GetFileName(filepath) + ".json";
        }

        public void resetMaleBigAnimation1(Config config)
        {
            config.animPathMaleBig = "";
            config.animJsonPathMaleBig = "";
        }

        public void setMaleBigAnimation2(Config config, string filepath)
        {
            config.animPathMaleBig2 = filepath;
            config.animJsonPathMaleBig2 = config.getProjectAnimsDirectory() + Path.GetFileName(filepath) + ".json";
        }

        public void resetMaleBigAnimation2(Config config)
        {
            config.animPathMaleBig2 = "";
            config.animJsonPathMaleBig2 = "";
        }

        public void setMaleAvgAnimation2(Config config, string filepath)
        {
            config.animPathMaleAvg2 = filepath;
            config.animJsonPathMaleAvg2 = config.getProjectAnimsDirectory() + Path.GetFileName(filepath) + ".json";
        }

        public void resetMaleAvgAnimation2(Config config)
        {
            config.animPathMaleAvg2 = "";
            config.animJsonPathMaleAvg2 = "";
        }


        // Reset all project variables.
        public void resetProject(Config config)
        {
            config.animPathFemaleAvg = "";
            config.animPathFemaleBig = "";
            config.animJsonPathFemaleAvg = "";
            config.animJsonPathFemaleBig = "";
            config.animPathMaleAvg = "";
            config.animPathMaleBig = "";
            config.animJsonPathMaleAvg = "";
            config.animJsonPathMaleBig = "";
            config.pathToWorkspotJson1 = "";
            config.pathToWorkspot1 = "";
            config.pathToEntityJson1 = "";
            config.pathToEntity1 = "";

            config.pathToWorkspotJson2 = "";
            config.pathToWorkspot2 = "";
            config.pathToEntityJson2 = "";
            config.pathToEntity2 = "";
            config.animPathFemaleAvg2 = "";
            config.animPathFemaleBig2 = "";
            config.animJsonPathFemaleAvg2 = "";
            config.animJsonPathFemaleBig2 = "";
            config.animPathMaleAvg2 = "";
            config.animPathMaleBig2 = "";
            config.animJsonPathMaleAvg2 = "";
            config.animJsonPathMaleBig2 = "";

            luaCategoryName = "";
            projectName = "";
            projectPath = "";
            internalProjectName = "eap_" + rnd.Next(randMin, randMax);
        }
    }
}