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
            textboxUsername.Text = config.projectUsername;
            //pathToGame.Text = config.modFolderPath;

            if (config.cliPath == "")
            {
                updateAppStatus("Select the path to your WolvenKit console before proceeding...");
            }
            else
            {
                updateAppStatus("Waiting for input...");
            }
        }

        /// <summary>
        /// Update the GUI status label.
        /// </summary>
        /// <param name="message">name of status message</param>
        /// <returns></returns>
        private int updateAppStatus(string message)
        {
            appStatus.Content = "Status: " + message;
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
        //private void TextboxGamePathHandler(object sender, RoutedEventArgs e)
        //{
        //    string value = FileIO.OpenFolder();
        //    if (value != null)
        //    {
        //        Debug.WriteLine("Folder selected, " + value);
        //        config.modFolderPath = value;
        //        pathToGame.Text = config.modFolderPath;
        //        Json.WriteConfigData(config);
        //    }
        //}

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
                config.setFemaleAvgAnimation1(config, value);
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
                config.setMaleAvgAnimation1(config, value);
                pathToMaleAverageAnim.Text = config.animPathMaleAvg;
            }
        }

        /// <summary>
        /// Handle user input on textbox for WB animation file path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextboxWBAnimPathHandler(object sender, EventArgs e)
        {
            string value = FileIO.OpenAnim();
            if (value != null)
            {
                Debug.WriteLine("DEBUG: Anim file [WB], " + value);
                config.setFemaleBigAnimation1(config, value);
                pathToFemaleBigAnim.Text = config.animPathFemaleBig;
            }
        }

        /// <summary>
        /// Handle user input on textbox for MB animation file path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextboxMBAnimPathHandler(object sender, EventArgs e)
        {
            string value = FileIO.OpenAnim();
            if (value != null)
            {
                Debug.WriteLine("DEBUG: Anim file [WB], " + value);
                config.setMaleBigAnimation1(config, value);
                pathToMaleBigAnim.Text = config.animPathMaleBig;
            }
        }

        /// <summary>
        /// Handle button press for "Load Poses from .ANIM".
        /// </summary>
        private async void ButtonConvertHandler(object sender, RoutedEventArgs e)
        {
            // If the user has not provided any animation files
            if (config.checkIfAllAnimPathsEmpty(config))
            {
                updateAppStatus("Error: No animation files were provided.");
                return;
            }

            // When the user provides a project name, build the project directory automatically. 
            if (config.projectName != "")
            {
                Directory.CreateDirectory(config.getProjectAnimsDirectory());
                Directory.CreateDirectory(config.getProjectResourcesDirectory());
                Debug.WriteLine("DEBUG: Project directory created: " + config.getProjectAnimsDirectory());
            }
            else
            {
                // The user cannot proceed without a project name.
                updateAppStatus("Error: You need to provide a project name first.");
                return;
            }

            // Call conversion method. Each call runs on a separate thread, allowing them to run concurrently.
            // Using async await so that we can don't block the main thread. UI can now update while tasks are running.
            // Without async, the GUI would be unresponsive for the user until the tasks are completed.
            updateAppStatus("Converting animation file(s). Please wait 5 to 30 seconds..");
            Task task1 = Task.Run(async () => await WolvenKit.ConvertAnimToJson(config.cliPath, config.animPathFemaleAvg, config, config.womanAverage));
            Task task2 = Task.Run(async () => await WolvenKit.ConvertAnimToJson(config.cliPath, config.animPathMaleAvg, config, config.manAverage));
            Task task3 = Task.Run(async () => await WolvenKit.ConvertAnimToJson(config.cliPath, config.animPathMaleBig, config, config.manBig));
            Task task4 = Task.Run(async () => await WolvenKit.ConvertAnimToJson(config.cliPath, config.animPathFemaleBig, config, config.womanBig));

            // Wait until all tasks are completed before proceeding. Await temporarily suspends the method.
            await Task.WhenAll(task1, task2, task3, task4);

            // Animation data will only be read after all tasks have completed.
            // We should only read data for rigs that the user has provided.
            if (!string.IsNullOrEmpty(config.animJsonPathFemaleAvg))
            {
                updateAppStatus("Reading female average animation data...");
                Debug.WriteLine("DEBUG: Reading animation data [WA]");
                readAnimData(config.animJsonPathFemaleAvg, config.womanAverage);
            }

            if (!string.IsNullOrEmpty(config.animJsonPathMaleAvg))
            {
                Debug.WriteLine("DEBUG: Reading animation data [MA]");
                updateAppStatus("Reading male average animation data...");
                readAnimData(config.animJsonPathMaleAvg, config.manAverage);
            }

            if (!string.IsNullOrEmpty(config.animJsonPathFemaleBig))
            {
                Debug.WriteLine("DEBUG: Reading animation data [WB]");
                updateAppStatus("Reading female big animation data...");
                readAnimData(config.animJsonPathFemaleBig, config.womanBig);
            }

            if (!string.IsNullOrEmpty(config.animJsonPathMaleBig))
            {
                Debug.WriteLine("DEBUG: Reading animation data [MB]");
                updateAppStatus("Reading male big animation data...");
                readAnimData(config.animJsonPathMaleBig, config.manBig);
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
            updateAppStatus("Building workspot JSON...");

            // Build the raw workspot JSON file.
            Task task1 = Task.Run(async () => await Workspot.BuildWorkspotJson(poseList, config));
            await Task.WhenAll(task1);
            Debug.WriteLine("Finished building workspot JSON..");

            // Convert the raw JSON file to RedEngine format.
            updateAppStatus("Converting workspot to RedEngine format...please wait.");
            Task task2 = Task.Run(async () => await WolvenKit.ConvertJsonToRedEngine(config.cliPath, config.pathToWorkspotJson1));
            await Task.WhenAll(task2);

            // Add the ".workspot" file extension so that the game can read it.
            WolvenKit.AddFileExtension(config.pathToWorkspotJson1, ".workspot", config);

            Debug.WriteLine("Finished building workspot.workspot");
            updateAppStatus("Finished building workspot. Path: " + config.pathToWorkspotJson1);

        }

        private async void ButtonEntityHandler(Object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("DEBUG: Building entity JSON...");
            updateAppStatus("Building entity JSON...");

            // Build the entity JSON file.
            Task task1 = Task.Run(async () => await Entity.buildEntityJson(config));
            await Task.WhenAll(task1);

            // Convert the raw JSON file to RedEngine format.
            Task task2 = Task.Run(async () => await WolvenKit.ConvertJsonToRedEngine(config.cliPath, config.pathToEntityJson1));

            await Task.WhenAll(task2);
            // Add the ".ent" file extension so that the game can read it.
            WolvenKit.AddFileExtension(config.pathToEntityJson1, ".ent", config);


            updateAppStatus("Finished building entity .ent: " + config.pathToEntity1);
        }

        private async void ButtonLuaHandler(Object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("DEBUG: Building lua file...");
            updateAppStatus("Building lua file...");

            Task task1 = Task.Run(async () => await Lua.readLuaTemplate(poseList, config));
            await Task.WhenAll(task1);

            updateAppStatus("Finished buiding lua file.");

        }

        private async void ButtonPackHandler(Object sender, RoutedEventArgs e)
        {
            updateAppStatus("Packing mod...");

            Task task1 = Task.Run(async () => await WolvenKit.packMod(config));
            await Task.WhenAll(task1);

            updateAppStatus("Finished packing mod.");

        }

        private void TextboxCategoryHandler(object sender, EventArgs e)
        {
            Debug.WriteLine("DEBUG: AMM Category Name, " + textboxCategory.Text);
            if (textboxCategory.Text != null)
            {
                config.luaCategoryName = textboxCategory.Text;
            }
            else
            {
                config.luaCategoryName = "Uncategorized Pose Pack";
            }
        }

        private void TextboxUsernameHandler(object sender, EventArgs e)
        {
            Debug.WriteLine("DEBUG: Modder Name, " + textboxUsername.Text);
            if (textboxUsername.Text != null)
            {
                config.projectUsername = textboxUsername.Text;
                Json.WriteConfigData(config);
            }
        }

        private void TextboxProjectNameHandler(object sender, EventArgs e)
        {
            // If the user has provided a project name, enable the animation file input fields.
            if (textboxProjectName.Text != "")
            {
                config.projectName = textboxProjectName.Text;
                config.projectPath = "projects/" + textboxProjectName.Text;
                Debug.WriteLine("DEBUG: Project Name, " + config.projectName);
                Debug.WriteLine("DEBUG: Project Path, " + config.projectPath);

                pathToFemaleAverageAnim.IsEnabled = true;
                pathToMaleAverageAnim.IsEnabled = true;
                pathToFemaleBigAnim.IsEnabled = true;
                pathToMaleBigAnim.IsEnabled = true;
            }
            else
            {
                config.projectName = "project1";
                config.projectPath = "projects/project1";
                pathToFemaleAverageAnim.IsEnabled = false;
                pathToMaleAverageAnim.IsEnabled = false;
                pathToFemaleBigAnim.IsEnabled = false;
                pathToMaleBigAnim.IsEnabled = false;
            }
        }

        private void ListView_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            listScrollView.ScrollToVerticalOffset(listScrollView.VerticalOffset - e.Delta);
            e.Handled = true;
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

                // If posename already in list of poses, don't add a new pose.
                // Instead, update the pose and add the new body type.
                bool poseExists = false;
                foreach (Pose pose in poseList)
                {
                    if (pose.Name == poseName)
                    {
                        Debug.WriteLine("DEBUG: Pose already in list, skipping... " + poseName);
                        pose.ExtraBodyTypes.Add(bodyType);
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