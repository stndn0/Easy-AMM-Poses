using System;
using System.Collections.Generic;
using System.Diagnostics;
using CliWrap;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

/* 
 * * This class acts as a brige between EAP and WolvenKit.
 * * It interacts with the WolvenKit CLI to perform operations (conversion, packing etc).
 * */
namespace Easy_AMM_Poses.src
{
    class WolvenKit
    {
        public static async Task<int> ConvertAnimToJson(String cliPath, String animPath)
        {
            Debug.WriteLine("DEBUG: CONVERTING ANIMATION TO JSON");
            var stdOutBuffer = new StringBuilder();
            var stdErrBuffer = new StringBuilder();

            //var animPath = "\"C:\\Users\\stndn\\Documents\\season7_tender_pose_pack_fa.anims\"";
            Debug.WriteLine("DEBUG: Animation path:" + animPath);

            // Start the WolvenKit CLI and pass the required arguments.
            var result = await Cli.Wrap(cliPath)
                //.WithArguments("convert s \"C:\\Users\\stndn\\Documents\\season7_tender_pose_pack_fa.anims\"")
                .WithArguments($"convert s {animPath}")
                .WithValidation(CommandResultValidation.None)
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                .ExecuteAsync();

            Debug.WriteLine(stdOutBuffer.ToString());
            Debug.WriteLine(stdErrBuffer.ToString());
            
            return 1;

        }
        public static async Task<int> ConvertJsonToWorkspot(String cliPath, String jsonPath)
        {
            Debug.WriteLine("DEBUG: CONVERTING WORKSPOT JSON TO WORKSPOT FILE");
            var stdOutBuffer = new StringBuilder();
            var stdErrBuffer = new StringBuilder();

            jsonPath = '"' + jsonPath + '"';

            Debug.WriteLine("DEBUG: JSON path:" + jsonPath);

            // Start the WolvenKit CLI and pass the required arguments.
            var result = await Cli.Wrap(cliPath)
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

        public static void AddWorkspotExtension(string jsonPath)
        {
            Debug.WriteLine("DEBUG: ADDING .WORKSPOT EXTENSION TO WORKSPOT FILE");

            // Rename the output file to workspot file
            var workspotFilePath = jsonPath.Replace(".json", ".workspot");
            File.Move(jsonPath.Replace(".json", ""), workspotFilePath);

        }

    }
}
