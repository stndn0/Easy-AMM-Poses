using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy_AMM_Poses
{
    class fileIO
    {
        public static void helloWorld()
        {
            System.Diagnostics.Debug.WriteLine("Hello world");
        }

        public static string openFile()
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

        public static string openFolder()
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
    }
}
