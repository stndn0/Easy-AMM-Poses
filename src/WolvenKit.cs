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
        /// <returns></returns>
        public static async Task<int> ConvertAnimToJson(String cliPath, String animPath, Config config)
        {
            Debug.WriteLine("DEBUG: CONVERTING ANIMATION TO JSON");
            var stdOutBuffer = new StringBuilder();
            var stdErrBuffer = new StringBuilder();

            // First check if the animation file was provided - handle empty path
            if (animPath == "")
            {
                return 0;
            }

            // Copy animation to project folder - this is where we do our work
            // Note: File.Copy is a synchronous operation. 
            // Until the copy is complete, code below it will not run.
            var newAnimationPath = config.getProjectAnimsDirectory() + Path.GetFileName(animPath);
            File.Copy(animPath, newAnimationPath, true);

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
            
            return 1;
        }

        /// <summary>
        /// Use the WolvenKit command line to deserialize json file to REDEngine.
        /// </summary>
        /// <param name="cliPath">Path to WolvenKit command line.</param>
        /// <param name="jsonPath">Path to serialized workspot.</param>
        /// <returns></returns>
        public static async Task<int> ConvertJsonToWorkspot(String cliPath, String jsonPath)
        {
            Debug.WriteLine("DEBUG: CONVERTING WORKSPOT JSON TO WORKSPOT FILE");
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

            // Rename the output file to workspot file
            Debug.WriteLine(stdOutBuffer.ToString());
            Debug.WriteLine(stdErrBuffer.ToString());

            return 1;
        }

        /// <summary>
        /// Add .workspot extension to deserialized REDEngine file.
        /// </summary>
        /// <param name="jsonPath">Path to serialized workspot</param>
        public static void AddWorkspotExtension(string jsonPath, Config config)
        {
            Debug.WriteLine("DEBUG: ADDING .WORKSPOT EXTENSION TO WORKSPOT FILE");

            // Rename the output file to workspot file
            var workspotFilePath = jsonPath.Replace(".json", ".workspot");

            // FIX REQUIRED
            // TODO: Can cause crash if file already exists - delete it first.
            File.Move(jsonPath.Replace(".json", ""), workspotFilePath);

            string finalPath = config.convertToRedengineFilepath(workspotFilePath);
            if (finalPath != "null")
            {
                config.pathToWorkspotMFA = finalPath;
            }
            else
            {
                MessageBox.Show("Error: Could not convert path to REDEngine format.");
            }
        }
    }
}
