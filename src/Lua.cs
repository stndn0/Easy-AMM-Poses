using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Easy_AMM_Poses.src
{
    /**
     * Q: "Why are you rolling your own manual solution? Why don't you use NLua?"
     * A: Because when I tried using a third party library (NLua), the package simply
     * would not install. And when it did install, it had issues that weren't covered
     * by the documentation, nor did I find anything helpful on their bug tracker on GitHub.
     * 
     * Hence, i'm rolling my own manual solution. It's not ideal but it's the only way.
    */


    public class Lua
    {

        public static async Task<int> readLuaTemplate(List<Pose> poseList, Config config, string pathToEntity, int fileNumber)
        {
            Debug.WriteLine("DEBUG: Reading Lua template");
            string luaFile = File.ReadAllText(@"templates\lua_template.lua");

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
            luaFile = Regex.Replace(luaFile, pattern, replacement);

            // Update username field
            pattern = @"(modder\s*=\s*)"".*?""";
            replacement = "$1" + '"' + config.projectUsername + '"';
            luaFile = Regex.Replace(luaFile, pattern, replacement);


            for (int i = 0; i < poseList.Count; i++)
            {
                if (poseList[i].BodyType == "MA" || poseList[i].ExtraBodyTypes.Contains("MA"))
                {
                    maleAveragePoses += '"' + poseList[i].Name + '"' + ", ";
                }

                if (poseList[i].BodyType == "WA" || poseList[i].ExtraBodyTypes.Contains("WA"))
                {
                    femaleAveragePoses += '"' + poseList[i].Name + '"' + ", ";
                }

                if (poseList[i].BodyType == "MB" || poseList[i].ExtraBodyTypes.Contains("MB"))
                {
                    bigPoses += '"' + poseList[i].Name + '"' + ", ";
                }

                if (poseList[i].BodyType == "WB" || poseList[i].ExtraBodyTypes.Contains("WB"))
                {
                    // Check if exact pose name already exists in string
                    // We don't want to add the same pose twice.
                    if (bigPoses.Contains(poseList[i].Name))
                    {
                        continue;
                    }
                    else
                    {
                        bigPoses += '"' + poseList[i].Name + '"' + ", ";
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
