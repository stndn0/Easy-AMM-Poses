using Easy_AMM_Poses.src;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy_AMM_Poses
{
    class FileIO
    {
        public static string OpenFile()
        {
            // Open file dialogue to select file path
            Microsoft.Win32.OpenFileDialog dialogue = new Microsoft.Win32.OpenFileDialog();
            dialogue.DefaultExt = ".exe";
            dialogue.Filter = "Executable Files (*.exe)|*.exe";

            Nullable<bool> result = dialogue.ShowDialog();

            // If the user has selected an executable file, return the file path
            if (result == true)
            {
                System.Diagnostics.Debug.WriteLine("return " + dialogue.FileName);
                return dialogue.FileName;
            }

            else
            {
                return null;
            }
        }

        public static string OpenFolder()
        {
            // Configure open folder dialog box
            Microsoft.Win32.OpenFolderDialog dialog = new();

            dialog.Multiselect = false;
            dialog.Title = "Select a folder";

            // Show open folder dialog box
            bool? result = dialog.ShowDialog();

            // Process open folder dialog box results
            if (result == true)
            {
                // Get the selected folder
                return dialog.FolderName;
            }
            else
            {
                return null;
            }
        }


        public static string OpenAnim()
        {
            // Open file dialogue to select file path
            Microsoft.Win32.OpenFileDialog dialogue = new Microsoft.Win32.OpenFileDialog();
            dialogue.DefaultExt = ".anims";
            dialogue.Filter = "ANIMS File (*.ANIMS)|*.ANIMS";
            dialogue.Title = "Select the Cyberpunk 2077 .ANIMS file";

            Nullable<bool> result = dialogue.ShowDialog();

            // If the user has selected an executable file, return the file path
            if (result == true)
            {
                System.Diagnostics.Debug.WriteLine("return " + dialogue.FileName);
                return dialogue.FileName;
            }

            else
            {
                return null;
            }
        }

        public static async Task<int> movePackedToDir(Config config)
        {
            // Move the packed .archive to the correct directory
            string pathToArchive = Path.GetFullPath(config.getPathToPackedArchive());
            string pathToModFolder = Path.GetFullPath(config.getProjectPackedDirectory()) + config.projectName + ".archive";


            Debug.WriteLine("Moving " + pathToArchive + " to " + pathToModFolder);

            // Delete file if it already exists before moving.
            Debug.WriteLine("\n\nCheck already exists: " + pathToModFolder);

            if (File.Exists(pathToModFolder))
            {
                Debug.WriteLine("Deleting...");
                File.Delete(pathToModFolder);
            }
            
            // Move the file
            File.Move(pathToArchive, pathToModFolder);


            // Move pose lua(s) to the correct directory
            DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(config.getProjectResourcesDirectory()));

            FileInfo[] files = dir.GetFiles("*.lua");

            Debug.WriteLine(dir);
            Debug.WriteLine("FileInfo:" + files);

            foreach (FileInfo file in files)
            {
                Debug.WriteLine("Lua: " + file.FullName);
                string temppath = Path.Combine(config.getProjectLuaDirectory(), file.Name);
                file.CopyTo(temppath, true);
            }



            return 1;
        }
    }
}
