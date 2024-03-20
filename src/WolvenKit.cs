using System.Diagnostics;
using CliWrap;
using System.Text;
using System.IO;
using System.Windows;

/* 
 * * This class acts as a brige between EAP and WolvenKit.
 * * It interacts with the WolvenKit CLI to perform operations (conversion, packing etc).
 * */
namespace Easy_AMM_Poses.src
{
    class WolvenKit
    {
        /// <summary>
        /// Use the WolvenKit command line to serialize animation file to json file.
        /// </summary>
        /// <param name="cliPath">Path to WolvenKit command line.</param>
        /// <param name="animPath">Path to users animation.</param>
        /// <param name="animSlot">Slot 1 = main four anim paths, Slot 2 = optional extra four anim paths.</param>
        /// <returns></returns>
        public static async Task<int> ConvertAnimToJson(string cliPath, string animPath, int animSlot, Config config, string rigType)
        {
            Debug.WriteLine("DEBUG: CONVERTING ANIMATION TO JSON");
            var stdOutBuffer = new StringBuilder();
            var stdErrBuffer = new StringBuilder();

            // First check if the animation file was provided - handle empty path
            if (animPath == "")
            {
                Debug.WriteLine("DEBUG: Animation path is empty. Skipping...");
                return 0;
            }

            // Copy animation to project folder - this is where we do our work
            // Note: File.Copy is a synchronous operation. 
            // Until the copy is complete, code below it will not run.
            var newAnimationPath = config.getProjectAnimsDirectory() + Path.GetFileName(animPath);
            Debug.WriteLine("\n\nAnimation path is " + animPath);
            Debug.WriteLine("DEBUG: New animation path is: " + newAnimationPath);
            File.Copy(animPath, newAnimationPath, true);
            Debug.Write("Copied file");

            var newAnimationPath2 = '"' + Path.GetFullPath(newAnimationPath) + '"';
            Debug.WriteLine("DEBUG: Internal animation path is: " + newAnimationPath2);

            // Start the WolvenKit CLI and pass the required arguments.
            await Cli.Wrap(cliPath)
                .WithArguments($"convert s {newAnimationPath2}")
                .WithValidation(CommandResultValidation.None)
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                .ExecuteAsync();

            Debug.WriteLine(stdOutBuffer.ToString());
            Debug.WriteLine(stdErrBuffer.ToString());

            // Update file path for anim file 
            if (rigType == config.womanAverage)
            {
                if (animSlot == 1)
                {
                    config.animPathFemaleAvg = config.convertToRedengineFilepath(newAnimationPath);
                    Debug.WriteLine("DEBUG: Path to WA: " + config.animPathFemaleAvg);
                }
                else if (animSlot == 2)
                {
                    config.animPathFemaleAvg2 = config.convertToRedengineFilepath(newAnimationPath);
                    Debug.WriteLine("DEBUG: Path to WA2: " + config.animPathFemaleAvg2);
                }

            }
            else if (rigType == config.womanBig)
            {
                if (animSlot == 1)
                {
                    config.animPathFemaleBig = config.convertToRedengineFilepath(newAnimationPath);
                }
                else if (animSlot == 2)
                {
                    config.animPathFemaleBig2 = config.convertToRedengineFilepath(newAnimationPath);
                }
                config.animPathFemaleBig = config.convertToRedengineFilepath(newAnimationPath);
            }
            else if (rigType == config.manAverage)
            {
                if (animSlot == 1)
                {
                    config.animPathMaleAvg = config.convertToRedengineFilepath(newAnimationPath);
                }
                else if (animSlot == 2)
                {
                    config.animPathMaleAvg2 = config.convertToRedengineFilepath(newAnimationPath);
                }

            }
            else if (rigType == config.manBig)
            {
                if (animSlot == 1)
                {
                    config.animPathMaleBig = config.convertToRedengineFilepath(newAnimationPath);
                }
                else if (animSlot == 2)
                {
                    config.animPathMaleBig2 = config.convertToRedengineFilepath(newAnimationPath);
                }
            }
            return 1;
        }

        /// <summary>
        /// Use the WolvenKit command line to deserialize json file to REDEngine.
        /// </summary>
        /// <param name="cliPath">Path to WolvenKit command line.</param>
        /// <param name="jsonPath">Path to serialized json.</param>
        /// <returns></returns>
        public static async Task<int> ConvertJsonToRedEngine(String cliPath, String jsonPath)
        {
            Debug.WriteLine("DEBUG: CONVERTING JSON TO REDENGINE OUTPUT FILE");
            var stdOutBuffer = new StringBuilder();
            var stdErrBuffer = new StringBuilder();
            jsonPath = '"' + jsonPath + '"';

            Debug.WriteLine("DEBUG: JSON path:" + jsonPath);

            // Start the WolvenKit CLI and pass the required arguments.
            await Cli.Wrap(cliPath)
                .WithArguments($"convert d {jsonPath}")
                .WithValidation(CommandResultValidation.None)
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                .ExecuteAsync();

            Debug.WriteLine(stdOutBuffer.ToString());
            Debug.WriteLine(stdErrBuffer.ToString());

            return 1;
        }

        public static async Task<int> packMod(Config config)
        {
            Debug.WriteLine("DEBUG: Packing mod...");
            var stdOutBuffer = new StringBuilder();
            var stdErrBuffer = new StringBuilder();
            string projectPath = '"' + config.getProjectRootDirectory() + '"';


            // Start the WolvenKit CLI and pass the required arguments.
            await Cli.Wrap(config.cliPath)
                .WithArguments($"pack p {projectPath}")
                .WithValidation(CommandResultValidation.None)
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                .ExecuteAsync();

            Debug.WriteLine(stdOutBuffer.ToString());
            Debug.WriteLine(stdErrBuffer.ToString());
            return 1;
        }


        


        /// <summary>
        /// Add .workspot or .ent file extension to deserialized REDEngine file.
        /// </summary>
        /// <param name="jsonOutputPath">Path to json file,</param>
        public static void AddFileExtension(string jsonOutputPath, Config config, string newExtension, int fileNumber)
        {
            /* How this works.
             * 
             * Renaming a file is pretty weird. There is no .rename method.
             * We essentially create a new filepath that has the extension we want.
             * Then we move the current file to the new filepath.
             * 
            */

            if (jsonOutputPath == "")
            {
                Debug.WriteLine("DEBUG: JSON output path is empty. Skipping...");
                return;
            }

            Debug.WriteLine("DEBUG: ADDING EXTENSION TO FILE");
            var newFilePath = "";

            // Create a new file path to the output file but with the extension we want.
            if (newExtension == ".ent")
            {
                newFilePath = jsonOutputPath.Replace(".json", ".ent");
            }

            else if (newExtension == ".workspot")
            {
                newFilePath = jsonOutputPath.Replace(".json", ".workspot");
            }

            // If the file already exists, perhaps from earlier usage of the program, we
            // need to delete it or else it'll cause a crash.
            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }

            // Move the file to the new file path.
            File.Move(jsonOutputPath.Replace(".json", ""), newFilePath);

            // Strip the junk from the file path so that it starts with "base\...."
            string finalPath = config.convertToRedengineFilepath(newFilePath);

            // Set the final file path.
            if (finalPath != "null")
            {
                if (newExtension == ".ent")
                {
                    if (fileNumber == 1)
                    {
                        config.pathToEntity1 = finalPath;
                    }
                    else if (fileNumber == 2)
                    {
                        config.pathToEntity2 = finalPath;
                    }
                }
                else if (newExtension == ".workspot")
                {
                    if (fileNumber == 1)
                    {
                        config.pathToWorkspot1 = finalPath;
                    }
                    else if (fileNumber == 2)
                    {
                        config.pathToWorkspot2 = finalPath;
                    }
                }
            }
            else
            {
                MessageBox.Show("Error: Could not convert path to REDEngine format.");
            }
        }
    }
}
