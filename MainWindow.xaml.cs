using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.Json;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Easy_AMM_Poses.src;
using Newtonsoft.Json.Linq;

namespace Easy_AMM_Poses
{
    public partial class MainWindow : Window
    {
        // Create a new class to store the configuration data, and set the default values.
        public class Config
        {
            public string cliPath = "Select the path to your WolvenKit CLI";
            public string modFolderPath = "Select the path to your Cyberpunk 2077 mod folder";
            public string configFilePath = "config/config.json";
        }
        Config config = new Config();

        // List of poses to be populated from the users animation JSON file.
        List<Pose> poseList = new List<Pose>();


        public MainWindow()
        {
            InitializeComponent();
            InitializeConfiguration();

            // Read the animation data from the JSON file.
            readAnimData(); 

            // Construct the workspot json using animation data.
            Workspot.BuildWorkspotJson(poseList);       // example of a static class member

            // Convert the workspot from json to .workspot.

        }






        // Initialize the configuration file and set the appropriate variables.
        // The config file is used to store paths to the CLI & the mod folder.
        private void InitializeConfiguration()
        {
            // Create the configuration directory. If the folder already exists, it'll be ignored.
            Directory.CreateDirectory("config");

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

            // Frontend XAML varaiables.
            pathToCli.Text = config.cliPath;
            pathToGame.Text = config.modFolderPath;
            myLabel.Content = "Debug: " + config.cliPath;
        }

        /// <summary>
        /// Load poses from animation JSON file.
        /// </summary>
        private void readAnimData()
        {
            var pathToJson2 = @"C:\Users\stndn\Documents\season7_allaccess_pose_pack.anims.json";

            // Load the JSON and deserialize it into a JToken object.
            var result = JsonConvert.DeserializeObject<JToken>(File.ReadAllText(pathToJson2));

            // Iterate through the JToken object - we're only iterating through what we want.
            foreach (var item in result["Data"]["RootChunk"]["animations"])
            {
                // The item we're after is called $value - this is the name of the animation.
                // Store all $value items into a new list
                // Debug.WriteLine(item["Data"]["animation"]["Data"]["name"]["$value"]);
                string poseName = item["Data"]["animation"]["Data"]["name"]["$value"].ToString();
                entries.Items.Add(poseName);

                // Create a new pose object for each value in the list.
                poseList.Add(new Pose(poseName));
            }
            Debug.WriteLine("DEBUG: List of pose objects");
            foreach (var pose in poseList)
            {
                //Debug.WriteLine(pose.Name);
            }

        }


        // Event handler for when the user clicks the cli path textbox.
        private void CliPathClickHandler(object sender, RoutedEventArgs e)
        {
            // Open a file dialog to select the path to the WolvenKit CLI
            string value = FileIO.OpenFile();
            if (value != null)
            {
                System.Diagnostics.Debug.WriteLine("File selected, " + value);
                config.cliPath = value;
                pathToCli.Text = config.cliPath;
                myLabel.Content = "Debug: " + config.cliPath;
                Json.WriteConfigData(config);
            }
        }

        // Event handler for when the user clicks the game path textbox.
        private void ModFolderPathClickHandler(object sender, RoutedEventArgs e)
        {
            // Open file dialog to select the path to the Cyberpunk 2077 mod folder
            string value = FileIO.OpenFolder();
            if (value != null)
            {
                System.Diagnostics.Debug.WriteLine("Folder selected, " + value);
                config.modFolderPath = value;
                pathToGame.Text = config.modFolderPath;
                Json.WriteConfigData(config);
            }
        }

        private void CliPathEventHandler(object sender, EventArgs e)
        {
            config.cliPath = pathToCli.Text;
            System.Diagnostics.Debug.WriteLine(config.cliPath);

            // Update the label myLabel to show the cliPath string
            myLabel.Content = "Debug: " + config.cliPath;
        }

        private void AnimFilePathClickHandler(object sender, EventArgs e)
        {
            string value = FileIO.OpenAnim();
            if (value != null)
            {
                System.Diagnostics.Debug.WriteLine("DEBUG: Anim file, " + value);
                pathToAnim.Text = value;
            }
        }

        private void ButtonConvertHandler(object sender, RoutedEventArgs e)
        {
            WolvenKit.ConvertAnimToJson(config.cliPath);
        }
    }
}