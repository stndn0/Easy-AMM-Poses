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

        public static void readLuaTemplate(List<Pose> poseList, Config config)
        {
            Debug.WriteLine("DEBUG: Reading Lua template");
            string luaFile = File.ReadAllText(@"templates\lua_template.lua");

            string maleAveragePoses = "";
            string femaleAveragePoses = "";
            string bigPoses = "";

            //  Update entity path
            string pattern = @"(entity_path\s*=\s*)"".*?""";
            string replacement = "$1" + '"' + config.pathToEntityMFA + '"';
            luaFile = Regex.Replace(luaFile, pattern, replacement);


            for (int i = 0; i < poseList.Count; i++)
            {
                if (poseList[i].BodyType == "MA" || poseList[i].ExtraBodyTypes.Contains("MA"))
                {
                    // If last iteration, add the comma and space.
                    if (i != poseList.Count - 1)
                    {
                        maleAveragePoses += poseList[i].Name + ", ";
                    }
                    // If last iteration, don't add comma and space.
                    else
                    {
                        maleAveragePoses += poseList[i].Name;
                    }
                }

                if (poseList[i].BodyType == "WA" || poseList[i].ExtraBodyTypes.Contains("WA"))
                {
                    // If last iteration, add the comma and space.
                    if (i != poseList.Count - 1)
                    {
                        femaleAveragePoses += poseList[i].Name + ", ";
                    }
                    // If last iteration, don't add comma and space.
                    else
                    {
                        femaleAveragePoses += poseList[i].Name;
                    }
                }

                if (poseList[i].BodyType == "MB" || poseList[i].ExtraBodyTypes.Contains("MB"))
                {
                    // If last iteration, add the comma and space.
                    if (i != poseList.Count - 1)
                    {
                        bigPoses += poseList[i].Name + ", ";
                    }
                    // If last iteration, don't add comma and space.
                    else
                    {
                        bigPoses += poseList[i].Name;
                    }
                }

                if (poseList[i].BodyType == "WB" || poseList[i].ExtraBodyTypes.Contains("WB"))
                {
                    // Check if exact pose name already exists in string
                    // We don't want to add the same pose twice.
                    if (bigPoses.Contains(poseList[i].Name))
                    {
                        break;
                    }
                    else
                    {
                        // If last iteration, add the comma and space.
                        if (i != poseList.Count - 1)
                        {
                            bigPoses += poseList[i].Name + ", ";
                        }
                        // If last iteration, don't add comma and space.
                        else
                        {
                            bigPoses += poseList[i].Name;
                        }
                    }
                }
            }


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

            Debug.WriteLine("Lua file: \n" + luaFile);
        }
    }
}
