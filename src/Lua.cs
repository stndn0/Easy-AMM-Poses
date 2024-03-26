using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Easy_AMM_Poses.src
{
   /// <summary>
   /// This class creates the lua file that is used by AMM to load poses into the game.
   /// Ideally you'd use a library like NLua to interact with Lua files but NLua wouldn't work so
   /// I rolled my own manual solution.
   /// </summary>
    public class Lua
    {
        public static async Task<int> readLuaTemplate(List<Pose> poseList, Config config, string pathToEntity, int fileNumber, string femAnimPath, string manAnimPath, string femBigAnimPath, string manBigAnimPath)
        {
            Debug.WriteLine("DEBUG: Reading Lua template. File number is " + fileNumber);
            string luaFile = File.ReadAllText(@"templates\lua_template.lua");

            // For the optional lua file, if there aren't any poses assigned to it then don't create the file
            if (fileNumber == 2 && !config.checkIfOptionalAnimsProvided(config))
            {
                Debug.WriteLine("DEBUG: No optional animations provided. Skipping lua file creation...");
                return 0;
            }

            string maleAveragePoses = "";
            string femaleAveragePoses = "";
            string bigPoses = "";

            //  Update entity path. Add extra backlash within the path string or else .lua will be unhappy.
            string pattern = @"(entity_path\s*=\s*)"".*?""";
            string replacement = "$1" + '"' + pathToEntity.Replace(@"\", @"\\") + '"';
            luaFile = Regex.Replace(luaFile, pattern, replacement);

            // Update category field
            pattern = @"(category\s*=\s*)"".*?""";
            replacement = "$1" + '"' + config.luaCategoryName + '"';
            if (fileNumber == 2)
            {
                replacement = "$1" + '"' + config.luaCategoryName + " (Extra)" + '"';
            }
            luaFile = Regex.Replace(luaFile, pattern, replacement);

            // Update username field
            pattern = @"(modder\s*=\s*)"".*?""";
            replacement = "$1" + '"' + config.projectUsername + '"';
            luaFile = Regex.Replace(luaFile, pattern, replacement);


            for (int i = 0; i < poseList.Count; i++)
            {
                //Debug.WriteLine("DEBUG: Pose name: " + poseList[i].Name + " Body: " + poseList[i].BodyType + " Pose slot: " + poseList[i].Slot + " fileNumber: " + fileNumber);
                // Check if pose belongs to the current file number
                // We don't want to add poses from another anim file that isn't linked to this lua.
                if (poseList[i].Slot.Contains(fileNumber))
                {
                    // Don't use else-if for this block. Multiple anim files may share the same pose name.
                    if (poseList[i].BodyType == "MA" || poseList[i].ExtraBodyTypes.Contains("MA"))
                    {
                        if (manAnimPath != "")
                        {
                            Debug.WriteLine("Added MA pose " + poseList[i].Name + " to workspot " + fileNumber);
                            maleAveragePoses += '"' + poseList[i].Name + '"' + ", ";
                        }
                    }

                    if (poseList[i].BodyType == "WA" || poseList[i].ExtraBodyTypes.Contains("WA"))
                    {
                        if (femAnimPath != "")
                        {
                            Debug.WriteLine("Added WA pose " + poseList[i].Name + " to workspot " + fileNumber);
                            femaleAveragePoses += '"' + poseList[i].Name + '"' + ", ";
                        }
                    }

                    if (poseList[i].BodyType == "MB" || poseList[i].ExtraBodyTypes.Contains("MB"))
                    {
                        if (manBigAnimPath != "")
                        {
                            Debug.WriteLine("Added MB pose " + poseList[i].Name + " to workspot " + fileNumber);
                            bigPoses += '"' + poseList[i].Name + '"' + ", ";
                        }
                    }

                    if (poseList[i].BodyType == "WB" || poseList[i].ExtraBodyTypes.Contains("WB"))
                    {
                        // Check if exact pose name already exists in string
                        // We don't want to add the same pose twice.
                        if (bigPoses.Contains(poseList[i].Name))
                        {
                            Debug.WriteLine("DEBUG: Pose already exists in big poses. Skipping...");
                            continue;
                        }
                        else
                        {
                            if (femBigAnimPath != "")
                            {
                                Debug.WriteLine("Added WB pose " + poseList[i].Name + " to workspot " + fileNumber);
                                bigPoses += '"' + poseList[i].Name + '"' + ", ";
                            }
                        }
                    }
                }
            }

            // Remove trailing whitespace and comma from end of string
            maleAveragePoses = maleAveragePoses.TrimEnd().TrimEnd(',');
            femaleAveragePoses = femaleAveragePoses.TrimEnd().TrimEnd(',');
            bigPoses = bigPoses.TrimEnd().TrimEnd(',');

            // Use regex to find and update the Man Average field
            pattern = @"\[\""Man Average\""\] = \{\}";
            replacement = @"[""Man Average""] = " + "{" + maleAveragePoses + "}"; 
            luaFile = Regex.Replace(luaFile, pattern, replacement);

            // Use regex to find and update the Woman Average field
            pattern = @"\[\""Woman Average\""\] = \{\}";
            replacement = @"[""Woman Average""] = " + "{" + femaleAveragePoses + "}";
            luaFile = Regex.Replace(luaFile, pattern, replacement);

            // Use regex to find and update the Big field
            pattern = @"\[\""Big\""\] = \{\}";
            replacement = @"[""Big""] = " + "{" + bigPoses + "}";
            luaFile = Regex.Replace(luaFile, pattern, replacement);

            // Write updated lua file to disk
            string pathToOutput = config.getProjectResourcesDirectory() + @"poses" + fileNumber + @".lua";

            File.WriteAllText(pathToOutput, luaFile);
            Debug.WriteLine("Lua file: \n" + luaFile);
            return 1;
        }
    }
}
