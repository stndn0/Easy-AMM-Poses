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
                btnConvert.IsEnabled = false;
            }
            else
            {
                updateAppStatus("WolvenKit online. Waiting for input...");
                btnConvert.IsEnabled = true;
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
                btnConvert.IsEnabled = true;
                updateAppStatus("WolvenKit online. Waiting for input...");
            }
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
                pathToFemaleAverageAnim2.IsEnabled = true;
                pathToMaleAverageAnim2.IsEnabled = true;
                pathToFemaleBigAnim2.IsEnabled = true;
                pathToMaleBigAnim2.IsEnabled = true;

            }
            else
            {
                config.projectName = "project1";
                config.projectPath = "projects/project1";
                pathToFemaleAverageAnim.IsEnabled = false;
                pathToMaleAverageAnim.IsEnabled = false;
                pathToFemaleBigAnim.IsEnabled = false;
                pathToMaleBigAnim.IsEnabled = false;
                pathToFemaleAverageAnim2.IsEnabled = false;
                pathToMaleAverageAnim2.IsEnabled = false;
                pathToFemaleBigAnim2.IsEnabled = false;
                pathToMaleBigAnim2.IsEnabled = false;
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

        private void buttonClearPathFemaleAvg1(object sender, EventArgs e)
        {
            config.resetFemaleAvgAnimation1(config);
            pathToFemaleAverageAnim.Text = "";
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

        private void buttonClearPathMaleAvg1(object sender, EventArgs e)
        {
            config.resetMaleAvgAnimation1(config);
            pathToMaleAverageAnim.Text = "";
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

        private void buttonClearPathFemaleBig1(object sender, EventArgs e)
        {
            config.resetFemaleBigAnimation1(config);
            pathToFemaleBigAnim.Text = "";
        }

        private void TextboxWBAnimPathHandler2(object sender, EventArgs e)
        {
            string value = FileIO.OpenAnim();
            if (value != null)
            {
                config.setFemaleBigAnimation2(config, value);
                pathToFemaleBigAnim2.Text = config.animPathFemaleBig2;
            }
        }

        private void buttonClearPathFemaleBig2(object sender, EventArgs e)
        {
            config.resetFemaleBigAnimation2(config);
            pathToFemaleBigAnim2.Text = "";
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

        private void buttonClearPathMaleBig1(object sender, EventArgs e)
        {
            config.resetMaleBigAnimation1(config);
            pathToMaleBigAnim.Text = "";
        }

        private void TextboxMBAnimPathHandler2(object sender, EventArgs e)
        {
            string value = FileIO.OpenAnim();
            if (value != null)
            {
                config.setMaleBigAnimation2(config, value);
                pathToMaleBigAnim2.Text = config.animPathMaleBig2;
            }
        }

        private void buttonClearPathMaleBig2(object sender, EventArgs e)
        {
            config.resetMaleBigAnimation2(config);
            pathToMaleBigAnim2.Text = "";
        }

        private void TextboxFemAnimPathHandler2(object sender, EventArgs e)
        {
            string value = FileIO.OpenAnim();
            if (value != null)
            {
                Debug.WriteLine("DEBUG: Anim file 2 [WA], " + value);
                config.setFemaleAvgAnimation2(config, value);
                pathToFemaleAverageAnim2.Text = config.animPathFemaleAvg2;
            }
        }

        private void buttonClearPathFemaleAvg2(object sender, EventArgs e)
        {
            config.resetFemaleAvgAnimation2(config);
            pathToFemaleAverageAnim2.Text = "";
        }
  
        private void TextboxMascAnimPathHandler2(object sender, EventArgs e)
        {
            string value = FileIO.OpenAnim();
            if (value != null)
            {
                Debug.WriteLine("DEBUG: Anim file 2 [MA], " + value);
                config.setMaleAvgAnimation2(config, value);
                pathToMaleAverageAnim2.Text = config.animPathMaleAvg2;
            }
        }

        private void buttonClearPathMaleAvg2(object sender, EventArgs e)
        {
            config.resetMaleAvgAnimation2(config);
            pathToMaleAverageAnim2.Text = "";
        }

        /// <summary>
        /// Handle button press for "Load Poses from .ANIM".
        /// Note that this method is quite verbose due to the amount of checks going on.
        /// Perhaps this could be refactored into a separate sub-methods.
        /// </summary>
        private async void ButtonConvertHandler(object sender, RoutedEventArgs e)
        {
            // If the user has not provided any animation files.
            if (config.checkIfAllAnimPathsEmpty(config))
            {
                updateAppStatus("Error: No animation files were provided.");
                MessageBox.Show("You need to provide at least one animation file to proceed.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // When the user provides a project name, build the project directory automatically. 
            if (config.projectName != "")
            {
                Directory.CreateDirectory(config.getProjectAnimsDirectory());
                Directory.CreateDirectory(config.getProjectResourcesDirectory());
                Directory.CreateDirectory(config.getProjectPackedDirectory());
                Directory.CreateDirectory(config.getProjectLuaDirectory());
                Debug.WriteLine("DEBUG: Project directory created: " + config.getProjectAnimsDirectory());

                // Prevent user from changing the project name now that the directories are already set up.
                textboxProjectName.IsEnabled = false;
            }
            else
            {
                // The user cannot proceed without a project name.
                updateAppStatus("Error: You need to provide a project name first.");
                return;
            }

            // Disable parts of the UI while the conversion is running
            interfaceToggle(false);

            try
            {
                updateAppStatus("Converting animation file(s). Please wait 5 to 30 seconds..");

                // You can't pass the actual text box values into the async task, so we need to store them in a string first.
                string fem1 = pathToFemaleAverageAnim.Text;
                string masc1 = pathToMaleAverageAnim.Text;
                string fem2 = pathToFemaleBigAnim.Text;
                string masc2 = pathToMaleBigAnim.Text;
                string fem3 = pathToFemaleAverageAnim2.Text;
                string masc3 = pathToMaleAverageAnim2.Text;
                string fem4 = pathToFemaleBigAnim2.Text;
                string masc4 = pathToMaleBigAnim2.Text;

                // Call conversion method. Each call runs on a separate thread, allowing them to run concurrently.
                // Using async await so that we can don't block the main thread. UI can now update while tasks are running.
                // Without async, the GUI would be unresponsive until the tasks are completed.
                Task task1 = Task.Run(async () => await WolvenKit.ConvertAnimToJson(config.cliPath, fem1, 1, config, config.womanAverage));
                Task task2 = Task.Run(async () => await WolvenKit.ConvertAnimToJson(config.cliPath, masc1, 1, config, config.manAverage));
                Task task3 = Task.Run(async () => await WolvenKit.ConvertAnimToJson(config.cliPath, fem2, 1, config, config.manBig));
                Task task4 = Task.Run(async () => await WolvenKit.ConvertAnimToJson(config.cliPath, masc2, 1, config, config.womanBig));
                Task task5 = Task.Run(async () => await WolvenKit.ConvertAnimToJson(config.cliPath, fem3, 2, config, config.womanAverage));
                Task task6 = Task.Run(async () => await WolvenKit.ConvertAnimToJson(config.cliPath, masc3, 2, config, config.manAverage));
                Task task7 = Task.Run(async () => await WolvenKit.ConvertAnimToJson(config.cliPath, fem4, 2, config, config.manBig));
                Task task8 = Task.Run(async () => await WolvenKit.ConvertAnimToJson(config.cliPath, masc4, 2, config, config.womanBig));

                // Wait until all tasks are completed before proceeding. Await temporarily suspends the method.
                await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8);

                // Animation data will only be read after all tasks have completed. Only read data for rigs that the user has provided.
                // Note: This is very verbose - perhaps refactor into a separate method.
                if (!string.IsNullOrEmpty(config.animJsonPathFemaleAvg))
                {
                    updateAppStatus("Reading female average animation data...");
                    Debug.WriteLine("DEBUG: Reading animation data [WA]");
                    readAnimData(config.animJsonPathFemaleAvg, config.womanAverage, 1);
                }

                if (!string.IsNullOrEmpty(config.animJsonPathMaleAvg))
                {
                    Debug.WriteLine("DEBUG: Reading animation data [MA]");
                    updateAppStatus("Reading male average animation data...");
                    readAnimData(config.animJsonPathMaleAvg, config.manAverage, 1);
                }

                if (!string.IsNullOrEmpty(config.animJsonPathFemaleBig))
                {
                    Debug.WriteLine("DEBUG: Reading animation data [WB]");
                    updateAppStatus("Reading female big animation data...");
                    readAnimData(config.animJsonPathFemaleBig, config.womanBig, 1);
                }

                if (!string.IsNullOrEmpty(config.animJsonPathMaleBig))
                {
                    Debug.WriteLine("DEBUG: Reading animation data [MB]");
                    updateAppStatus("Reading male big animation data...");
                    readAnimData(config.animJsonPathMaleBig, config.manBig, 1);
                }

                if (!string.IsNullOrEmpty(config.animJsonPathFemaleAvg2))
                {
                    Debug.WriteLine("DEBUG: Reading animation data [WA2]");
                    updateAppStatus("Reading female average animation data (2)");
                    readAnimData(config.animJsonPathFemaleAvg2, config.womanAverage, 2);
                }

                if (!string.IsNullOrEmpty(config.animJsonPathMaleAvg2))
                {
                    Debug.WriteLine("DEBUG: Reading animation data [MA2]");
                    updateAppStatus("Reading male average animation data (2)");
                    readAnimData(config.animJsonPathMaleAvg2, config.manAverage, 2);
                }

                if (!string.IsNullOrEmpty(config.animJsonPathFemaleBig2))
                {
                    Debug.WriteLine("DEBUG: Reading animation data [WB2]");
                    updateAppStatus("Reading female big animation data (2)");
                    readAnimData(config.animJsonPathFemaleBig2, config.womanBig, 2);
                }

                if (!string.IsNullOrEmpty(config.animJsonPathMaleBig2))
                {
                    Debug.WriteLine("DEBUG: Reading animation data [MB2]");
                    updateAppStatus("Reading male big animation data (2)");
                    readAnimData(config.animJsonPathMaleBig2, config.manBig, 2);
                }
                updateAppStatus("Conversion complete. Ready to build.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DEBUG: Error converting animation file to JSON: " + ex.Message);
                updateAppStatus("Error #1. One or more animations couldn't be read by Wolvenkit CLI");

                // Reset project folder and clear pose list
                MessageBox.Show($"Error #1. Error reading animation file(s). \n\n(1) Please make sure you're using CLI 8.13 stable or above. Version 8.13 in particular should have the most compatibility.\n(2) Is your .ANIM file valid? Can you open it in the regular WolvenKit GUI? \n\nIf you're still having issues please let me know and i'll try to help. Sorry about that. \n\n\nError Log: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            interfaceToggle(true);
        }

        /// <summary>
        /// Load poses from the converted animation JSON file.
        /// This method really shouldn't be in MainWindow...
        /// </summary>
        /// <param name="pathToAnimJson"></param>
        /// <param name="bodyType"></param>
        public void readAnimData(string pathToAnimJson, string bodyType, int animSlot)
        {
            var pathToJson2 = pathToAnimJson;

            // Load the JSON and deserialize it into a JToken object.
            var result = JsonConvert.DeserializeObject<JToken>(File.ReadAllText(pathToJson2));

            // Iterate through the JToken object - we're only iterating through what we want.
            foreach (var item in result["Data"]["RootChunk"]["animations"])
            {
                // The item we're after is called $value - this is the name of the animation.
                // Store all $value items into a new list
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
                        //pose.Slot = animSlot;
                        poseExists = true;
                        break;
                    }
                }
                if (!poseExists)
                {
                    entries.Items.Add(poseName);
                    poseList.Add(new Pose(poseName, bodyType, animSlot)); ;
                }
            }
        }
        /// <summary>
        /// Handle button press for "Build Workspot".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonBuildHandler(object sender, RoutedEventArgs e)
        {
            updateAppStatus("Building workspot JSON...");
            interfaceToggle(false);

            try
            {
                // Build the raw workspot JSON file.
                Task task1 = Task.Run(async () => await Workspot.BuildWorkspotJson(poseList, config, config.animPathFemaleAvg, config.animPathMaleAvg, config.animPathFemaleBig, config.animPathMaleBig, 1));
                Task task2 = Task.Run(async () => await Workspot.BuildWorkspotJson(poseList, config, config.animPathFemaleAvg2, config.animPathMaleAvg2, config.animPathFemaleBig2, config.animPathMaleBig2, 2));
                await Task.WhenAll(task1, task2);
                Debug.WriteLine("Finished building workspot JSON..");

                // Convert the raw JSON file to RedEngine format.
                updateAppStatus("Converting workspot to RedEngine format...please wait.");
                Task task3 = Task.Run(async () => await WolvenKit.ConvertJsonToRedEngine(config.cliPath, config.pathToWorkspotJson1));
                Task task4 = Task.Run(async () => await WolvenKit.ConvertJsonToRedEngine(config.cliPath, config.pathToWorkspotJson2));
                await Task.WhenAll(task3, task4);

                // Add the ".workspot" file extension so that the game can read it.
                WolvenKit.AddFileExtension(config.pathToWorkspotJson1, config, ".workspot", 1);
                WolvenKit.AddFileExtension(config.pathToWorkspotJson2, config, ".workspot", 2);

                Debug.WriteLine("Finished building workspot.workspot");
                updateAppStatus("Finished building workspot. Path: " + config.pathToWorkspotJson1);

                Debug.WriteLine("PATH to WS1: " + config.pathToWorkspot1);
                Debug.WriteLine("PATH to WS2: " + config.pathToWorkspot2);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DEBUG: Error building workspot: " + ex.Message);
                updateAppStatus("Error: Could not build workspot file.");
                MessageBox.Show($"Error building workspot file. \n\n(1) Please make sure you're using CLI 8.13 stable or above. Version 8.13 should have the most compatibility.\n\n\nIf you're still having issues please let me know and i'll try to help. Sorry about that. \n\n\nError Log: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Build ent and lua file
            ButtonEntityHandler(sender, e);
        }

        private async void ButtonEntityHandler(Object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("DEBUG: Building entity JSON...");
                updateAppStatus("Building entity JSON...");

                // Build the entity JSON file.
                Task task1 = Task.Run(async () => await Entity.buildEntityJson(config, 1, config.pathToWorkspot1));
                Task task2 = Task.Run(async () => await Entity.buildEntityJson(config, 2, config.pathToWorkspot2));
                await Task.WhenAll(task1, task2);

                // Convert the raw JSON file to RedEngine format.
                Task task3 = Task.Run(async () => await WolvenKit.ConvertJsonToRedEngine(config.cliPath, config.pathToEntityJson1));
                Task task4 = Task.Run(async () => await WolvenKit.ConvertJsonToRedEngine(config.cliPath, config.pathToEntityJson2));

                await Task.WhenAll(task3, task4);
                // Add the ".ent" file extension so that the game can read it.
                WolvenKit.AddFileExtension(config.pathToEntityJson1, config, ".ent", 1);
                WolvenKit.AddFileExtension(config.pathToEntityJson2, config, ".ent", 2);

                // Build lua file
                ButtonLuaHandler(sender, e);

                updateAppStatus("Finished building mod. Ready to pack!");
                interfaceToggle(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DEBUG: Error building ent file: " + ex.Message);
                updateAppStatus("Error: Could not build ent file.");
                MessageBox.Show($"Error building ent file. \n\n(1) Please make sure you're using CLI 8.13 stable or above. Version 8.13 should have the most compatibility.\n\n\nIf you're still having issues please let me know and i'll try to help. Sorry about that. \n\n\nError Log: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ButtonLuaHandler(Object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("DEBUG: Building lua file...");
                updateAppStatus("Building lua file...");

                Task task1 = Task.Run(async () => await Lua.readLuaTemplate(poseList, config, config.pathToEntity1, 1));
                Task task2 = Task.Run(async () => await Lua.readLuaTemplate(poseList, config, config.pathToEntity2, 2));
                await Task.WhenAll(task1, task2);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DEBUG: Error building lua file: " + ex.Message);
                updateAppStatus("Error: Error building lua file.");
                MessageBox.Show($"Error building lua file. \n\nThis is a rare error that shouldn't happen... contact me for help. \n\n\nError Log: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private async void ButtonPackHandler(Object sender, RoutedEventArgs e)
        {
            interfaceToggle(false);
            try
            {
                updateAppStatus("Packing mod...");
                Debug.WriteLine("DEBUG: Packing mod...");
                Task task1 = Task.Run(async () => await WolvenKit.packMod(config));
                await Task.WhenAll(task1);
                Debug.WriteLine("DEBUG: Finished packing mod...");

                Debug.WriteLine("DEBUG: Moving mod files to packed directory...");
                Task task2 = Task.Run(async () => await FileIO.movePackedToDir(config));
                await Task.WhenAll(task2);

                updateAppStatus("Finished packing mod.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DEBUG: Error packing project: " + ex.Message);
                updateAppStatus("Error: WolvenKit couldn't pack your project.");
                MessageBox.Show($"Error packing project. \n\nYour copy of WolvenKit CLI wasn't able to pack the project file. Ensure you're using CLI 8.13 stable or above. \n\n\nError Log: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            interfaceToggle(true);
        }

        private void ButtonOpenProjectFolderHandler(Object sender, RoutedEventArgs e)
        {
            if (config.projectPath != null)
            {
                string folderPath = config.getProjectDirectory();
                Process.Start("explorer.exe", $"\"{folderPath}\"");
            }
            else
            {
                Debug.WriteLine("DEBUG: Could not find project folder to open.");
            }
        }

        private void ButtonOpenLuaFolderHandler(Object sender, RoutedEventArgs e)
        {
            if (config.projectPath != null)
            {
                string folderPath = config.getProjectLuaDirectory();
                Process.Start("explorer.exe", $"\"{folderPath}\"");
            }
            else
            {
                Debug.WriteLine("DEBUG: Could not find lua folder to open.");
            }
        }

        private void ButtonRestartHandler(Object sender, RoutedEventArgs e)
        {
            poseList.Clear();
            config.resetProject(config);
            
            textboxCategory.Text = "";
            textboxProjectName.Text = "";
            pathToFemaleAverageAnim.Text = "";
            pathToMaleAverageAnim.Text = "";
            pathToFemaleBigAnim.Text = "";
            pathToMaleBigAnim.Text = "";
            pathToFemaleAverageAnim2.Text = "";
            pathToMaleAverageAnim2.Text = "";
            pathToFemaleBigAnim2.Text = "";
            pathToMaleBigAnim2.Text = "";
            entries.Items.Clear();
            textboxProjectName.IsEnabled = true;
            interfaceToggle(false);
            btnConvert.IsEnabled = true;

            updateAppStatus("Project has been reset.");
        }

        // Enable or disable UI elements
        // Value must be a boolean
        private void interfaceToggle(Boolean val)
        {
            btnConvert.IsEnabled = val;
            btnBuild.IsEnabled = val;
            btnPackMod.IsEnabled = val;
            btnOpenProjectFolder.IsEnabled = val;
            btnOpenLuaFolder.IsEnabled = val;
        }


        private void ListView_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            listScrollView.ScrollToVerticalOffset(listScrollView.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}