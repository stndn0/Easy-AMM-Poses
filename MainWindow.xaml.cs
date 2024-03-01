using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Windows;
using Easy_AMM_Poses.src;
using Newtonsoft.Json.Linq;

namespace Easy_AMM_Poses
{
    public partial class MainWindow : Window
    {
        Config config = new Config();

        // Create list of poses to be populated from the users animation json files.
        List<Pose> poseList = new List<Pose>();

        string womanAverage = "WA";
        string womanBig = "MB";
        string manAverage = "MA";
        string manBig = "MB";

        public MainWindow()
        {
            InitializeComponent();
            InitializeConfiguration();
        }


        /// <summary>
        /// Initialize the configuration file and set the appropriate variables.
        /// </summary>
        private void InitializeConfiguration()
        {
            // Create or load configuration file.
            config.SetConfigFile(config);

            // Set frontend XAML varaiables.
            pathToCli.Text = config.cliPath;
            pathToGame.Text = config.modFolderPath;
        }

        /// <summary>
        /// Update the GUI status label.
        /// </summary>
        /// <param name="message">name of status message</param>
        /// <returns></returns>
        private int updateAppStatus(string message)
        {
            appStatus.Content = "Debug: " + message;
            return 1;
        }

        /// <summary>
        /// Handle user input on textbox for CLI path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextboxCliPathHandler(object sender, RoutedEventArgs e)
        {
            // Open a file dialog to select the path to the WolvenKit CLI
            string value = FileIO.OpenFile();
            if (value != null)
            {
                Debug.WriteLine("File selected, " + value);
                config.cliPath = value;
                pathToCli.Text = config.cliPath;
                Json.WriteConfigData(config);
            }
        }

        /// <summary>
        /// Handle user input on textbox for mod folder path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextboxGamePathHandler(object sender, RoutedEventArgs e)
        {
            string value = FileIO.OpenFolder();
            if (value != null)
            {
                Debug.WriteLine("Folder selected, " + value);
                config.modFolderPath = value;
                pathToGame.Text = config.modFolderPath;
                Json.WriteConfigData(config);
            }
        }

        /// <summary>
        /// Handle user input on textbox for WA animation file path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextboxFemAnimPathHandler(object sender, EventArgs e)
        {
            string value = FileIO.OpenAnim();
            if (value != null)
            {
                Debug.WriteLine("DEBUG: Anim file [WA], " + value);
                config.animPathFemaleAvg = value;
                config.animJsonPathFemaleAvg = config.getProjectAnimsDirectory() + Path.GetFileName(value) + ".json";
                pathToFemaleAverageAnim.Text = config.animPathFemaleAvg;
            }
        }
        /// <summary>
        /// Handle user input on textbox for MA animation file path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextboxMascAnimPathHandler(object sender, EventArgs e)
        {
            string value = FileIO.OpenAnim();
            if (value != null)
            {
                Debug.WriteLine("DEBUG: Anim file [MA], " + value);
                config.animPathMaleAvg = value;
                config.animJsonPathMaleAvg = config.getProjectAnimsDirectory() + Path.GetFileName(value) + ".json";
                pathToMaleAverageAnim.Text = config.animPathMaleAvg;
            }
        }

        /// <summary>
        /// Handle button press for "Load Poses from .ANIM".
        /// </summary>
        private async void ButtonConvertHandler(object sender, RoutedEventArgs e)
        {
            // If the user has not provided any animation files
            if (string.IsNullOrEmpty(config.animPathFemaleAvg) && string.IsNullOrEmpty(config.animPathMaleAvg))
            {
                Debug.WriteLine("DEBUG: No animation files provided.");
                updateAppStatus("No animation files provided.");
                return;
            }

            // Call conversion method. Each call runs on a separate thread, allowing them to run concurrently.
            // Using async await so that we can don't block the main thread.
            // This lets us update the UI while the tasks are running.
            // Without async, the GUI would be unresponsive for the user until the tasks are completed.
            updateAppStatus("Converting animation file(s). Please wait 5 to 30 seconds..");
            Task task1 = Task.Run(async () => await WolvenKit.ConvertAnimToJson(config.cliPath, config.animPathFemaleAvg, config));
            Task task2 = Task.Run(async () => await WolvenKit.ConvertAnimToJson(config.cliPath, config.animPathMaleAvg, config));

            // Wait until all tasks are completed before proceeding. Await temporarily suspends the method.
            await Task.WhenAll(task1, task2);

            // Animation data will only be read after both tasks have completed.
            // Read data for all applicable rigs.
            if (!string.IsNullOrEmpty(config.animJsonPathFemaleAvg))
            {
                updateAppStatus("Reading female animation data...");
                Debug.WriteLine("DEBUG: Reading animation data [WA]");
                readAnimData(config.animJsonPathFemaleAvg, womanAverage);
            }

            if (!string.IsNullOrEmpty(config.animJsonPathMaleAvg))
            {
                updateAppStatus("Reading male animation data...");
                readAnimData(config.animJsonPathMaleAvg, manAverage);
            }
            updateAppStatus("Conversion complete. Ready to build.");
        }

        /// <summary>
        /// Handle button press for "Build Workspot".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonBuildHandler(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("DEBUG: Build requested...");
            updateAppStatus("Building workspot JSON...");

            Task task1 = Task.Run(async () => await Workspot.BuildWorkspotJson(poseList, config));
            await Task.WhenAll(task1);

            Debug.WriteLine("Finished building workspot JSON..");
            updateAppStatus("Converting workspot to RedEngine format...please wait.");

            Task task2 = Task.Run(async () => await WolvenKit.ConvertJsonToWorkspot(config.cliPath, config.pathToWorkspotJsonMFA));
            await Task.WhenAll(task2);
            WolvenKit.AddWorkspotExtension(config.pathToWorkspotJsonMFA);
            Debug.WriteLine("Finished building workspot.workspot");
            updateAppStatus("Finished building workspot. Path: " + config.pathToWorkspotJsonMFA);

        }

        private async void ButtonEntityHandler(Object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("DEBUG: Building entity JSON...");
            updateAppStatus("Building entity JSON...");

            Task task1 = Task.Run(async () => await Entity.buildEntityJson(config));
            await Task.WhenAll(task1);

            updateAppStatus("Finished building entity JSON.");
        }

        /// <summary>
        /// Load poses from the converted animation JSON file.
        /// </summary>
        /// <param name="pathToAnimJson"></param>
        /// <param name="bodyType"></param>
        public void readAnimData(string pathToAnimJson, string bodyType)
        {
            //var pathToJson2 = @"C:\Users\stndn\Documents\season7_allaccess_pose_pack.anims.json";
            var pathToJson2 = pathToAnimJson;

            // Load the JSON and deserialize it into a JToken object.
            var result = JsonConvert.DeserializeObject<JToken>(File.ReadAllText(pathToJson2));

            // Iterate through the JToken object - we're only iterating through what we want.
            foreach (var item in result["Data"]["RootChunk"]["animations"])
            {
                // The item we're after is called $value - this is the name of the animation.
                // Store all $value items into a new list
                // Debug.WriteLine(item["Data"]["animation"]["Data"]["name"]["$value"]);
                string poseName = item["Data"]["animation"]["Data"]["name"]["$value"].ToString();

                // Todo - if posename already in list of poses, skip it.
                bool poseExists = false;
                foreach (Pose pose in poseList)
                {
                    if (pose.Name == poseName)
                    {
                        Debug.WriteLine("DEBUG: Pose already in list, skipping... " + poseName);
                        poseExists = true;
                        break;
                    }
                }
                if (!poseExists)
                {
                    entries.Items.Add(poseName);
                    poseList.Add(new Pose(poseName, bodyType)); ;
                }

            }
        }
    }
}