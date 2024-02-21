using System.Text;
using System.IO;
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


        public MainWindow()
        {
            InitializeComponent();

            // Create the configuration directory. If the folder already exists, it'll be ignored.
            Directory.CreateDirectory("config");

            // If the config file doesn't exist, create it.
            if (!File.Exists(config.configFilePath))
            {
                File.Create(config.configFilePath).Close();
                writeConfigData();
            }
            else
            // If the file already exists, read it's properties and update the variables.
            {
                readConfigData();
            }

            // Frontend XAML varaiables.
            pathToCli.Text = config.cliPath;
            pathToGame.Text = config.modFolderPath;
            myLabel.Content = "Debug: " + config.cliPath;
        }


        // Event handler for when the user clicks the cli path textbox.
        private void cliPathClickHandler(object sender, RoutedEventArgs e)
        {
            // Open a file dialog to select the path to the WolvenKit CLI
            string value = fileIO.openFile();
            if (value != null)
            {
                System.Diagnostics.Debug.WriteLine("File selected, " + value);
                config.cliPath = value;
                pathToCli.Text = config.cliPath;
                myLabel.Content = "Debug: " + config.cliPath;
                writeConfigData();
            }
        }

        // Event handler for when the user clicks the game path textbox.
        private void modFolderPathClickHandler(object sender, RoutedEventArgs e)
        {
            // Open file dialog to select the path to the Cyberpunk 2077 mod folder
            string value = fileIO.openFolder();
            if (value != null)
            {
                System.Diagnostics.Debug.WriteLine("Folder selected, " + value);
                config.modFolderPath = value;
                pathToGame.Text = config.modFolderPath;
                writeConfigData();
            }
        }


        private void cliPathEventHandler(object sender, EventArgs e)
        {
            config.cliPath = pathToCli.Text;
            System.Diagnostics.Debug.WriteLine(config.cliPath);

            // Update the label myLabel to show the cliPath string
            myLabel.Content = "Debug: " + config.cliPath;
        }

        // Write configuration data to JSON file
        private void writeConfigData()
        {
            using (StreamWriter streamWriter = new StreamWriter(config.configFilePath, false))
            {
                var jsonData = new
                {
                    cliPath = config.cliPath,
                    modFolderPath = config.modFolderPath
                };

                // Write new data to the configuration file
                var serializedData = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
                streamWriter.WriteLine(serializedData);
                System.Diagnostics.Debug.WriteLine($"DEBUG: Updated {config.configFilePath} with data: {serializedData}");
            }
        }

        // Read configuration data from JSON file and update variables.
        private void readConfigData()
        {
            var deserializedData = JsonConvert.DeserializeObject<Config>(File.ReadAllText(config.configFilePath));
            System.Diagnostics.Debug.WriteLine($"DEBUG: Read {config.configFilePath} with data: {deserializedData.cliPath}, {deserializedData.modFolderPath}");

            config.cliPath = deserializedData.cliPath;
            config.modFolderPath = deserializedData.modFolderPath;
        }
    }
}