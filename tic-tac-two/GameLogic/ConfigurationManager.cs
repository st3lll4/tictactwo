using System.Text.Json;

namespace GameLogic
{
    public class ConfigurationManager
    {
        // Path to the file where configurations will be saved
        private const string ConfigFilePath = "configurations.json";

        // Initialize the list to store all game configurations
        private List<ConfigurationEntry> _configurations = [];

        // Constructor to load configurations from the file and initialize defaults
        public ConfigurationManager()
        {
            LoadConfigurationsFromFile();  // Load configurations from file at startup
            InitializeDefaultConfigurations(); // Initialize default configurations
        }

        // Method to load configurations from the file at the start of the program
        private void LoadConfigurationsFromFile()
        {
            if (File.Exists(ConfigFilePath))
            {
                try
                {
                    var json = File.ReadAllText(ConfigFilePath);
                    var loadedConfigs = JsonSerializer.Deserialize<List<ConfigurationEntry>>(json);

                    // If loadedConfigs is null, default to an empty list
                    if (loadedConfigs != null)
                    {
                        _configurations = loadedConfigs;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading configurations: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("No previous configurations found.");
            }
        }


        private void InitializeDefaultConfigurations()
        {
            if (_configurations.Count != 0) return;

            var defaultConfig1 = new GameConfiguration()
            {
                GameName = "Tic-Tac-Toe",
                Width = 3,
                Height = 3,
                Player1Symbol = 'X',
                Player2Symbol = 'O',
                StartingPlayer = "Player 1"
            };
            _configurations.Add(new ConfigurationEntry { ConfigName = defaultConfig1.GameName, Config = defaultConfig1 });

            var defaultConfig2 = new GameConfiguration()
            {
                GameName = "Tic-Tac-Two with a 10x10 board",
                Width = 10,
                Height = 10,
                Player1Symbol = 'X',
                Player2Symbol = 'O',
                StartingPlayer = "Player 1"
            };
            _configurations.Add(new ConfigurationEntry { ConfigName = defaultConfig2.GameName, Config = defaultConfig2 });

            SaveConfigurationsToFile();
        }


        // Method to save the current configuration to the list and to a file
        private void SaveConfiguration(GameConfiguration config, string configName)
        {
            // Check if a configuration with the same name exists
            var existingConfig = _configurations.Find(c => c.ConfigName == configName);
            if (existingConfig != null)
            {
                _configurations.Remove(existingConfig);  // Remove the existing configuration if found
            }

            // Add the new configuration
            _configurations.Add(new ConfigurationEntry { ConfigName = configName, Config = config });
            Console.WriteLine($"Configuration '{configName}' saved successfully.");

            // Save the configurations to a file
            SaveConfigurationsToFile();
        }


        // Save the configurations to a file in JSON format
        private void SaveConfigurationsToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(_configurations);
                File.WriteAllText(ConfigFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving configurations: {ex.Message}");
            }
        }


        // Method to load a configuration by name from the list
        public GameConfiguration LoadConfiguration(string configName)
        {
            // Find the configuration in the list by name
            var configEntry = _configurations.Find(c => c.ConfigName == configName);
            if (configEntry == null) throw new Exception($"Configuration '{configName}' not found.");
            Console.WriteLine($"Configuration '{configName}' loaded successfully.");
            return configEntry.Config;
        }


        // Method to get all saved configurations (returns the names)
        public List<string> GetSavedConfigurations()
        {
            // Extract the configuration names from the list
            List<string> configNames = new List<string>();
            foreach (var configEntry in _configurations)
            {
                configNames.Add(configEntry.ConfigName);
            }

            return configNames;
        }



        // Method to create a new configuration by interacting with the user
        public void CreateConfiguration()
        {

            Console.Clear();
            Console.WriteLine("Game configuration creation:");

            var config = new GameConfiguration();

            Console.WriteLine("Name your configuration:");
            
            var name = Console.ReadLine() ?? $"Config nr {_configurations.Count + 1}";
            
            config.GameName = name;
            
            config.Width = GetValidInput("Enter the width of the board (at least 3):");
            config.Height = GetValidInput("Enter the height of the board (at least 3):");

            // Configure player symbols
            Console.WriteLine("Enter the symbol for Player 1 (default is X):");
            config.Player1Symbol = GetValidSymbol();

            Console.WriteLine("Enter the symbol for Player 2 (default is O):");
            config.Player2Symbol = GetValidSymbol();

            // Configure starting player
            Console.WriteLine("Who should start? Enter '1' for Player 1 or '2' for Player 2:");
            var startingPlayerChoice = Console.ReadLine();
                if (startingPlayerChoice == "1")
                {
                    config.StartingPlayer = "Player 1";
                }
                else if (startingPlayerChoice == "2")
                {
                    config.StartingPlayer = "Player 2";
                }
                else
                {
                    Console.WriteLine("Invalid input. Enter '1' for Player 1 or '2' for Player 2:");
                }            

            SaveConfiguration(config, name);
        }
        
        // Method to delete a configuration by its name
        public void DeleteConfiguration()
        {
            Console.WriteLine("Saved Configurations:");
            for (int i = 0; i < _configurations.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_configurations[i].ConfigName}");
            }

            Console.Write("Enter the number of the configuration to delete: ");
            if (int.TryParse(Console.ReadLine(), out int configIndex) && configIndex >= 1 && configIndex <= _configurations.Count)
            {
                string selectedConfig = _configurations[configIndex - 1].ConfigName;
                _configurations.RemoveAt(configIndex - 1);  // Remove by index
                Console.WriteLine($"Configuration '{selectedConfig}' deleted successfully.");

                // Save the updated configurations to the file
                SaveConfigurationsToFile();
            }
            else
            {
                Console.WriteLine("Invalid input. Press Enter to return to the menu.");
            }

            Console.ReadLine(); // Wait for user input before returning
        }


        // Helper method to get valid integer input >= 3
        private int GetValidInput(string prompt)
        {
            int result;
            Console.WriteLine(prompt);

            // Loop until a valid integer >= 3 is entered
            while (!int.TryParse(Console.ReadLine(), out result) || result < 3)
            {
                Console.WriteLine("Invalid input! Please enter an integer of at least 3:");
            }

            return result;
        }

        // Helper method to get a valid single-character symbol for players
        private char GetValidSymbol()
        {
            char symbol;
            while (!char.TryParse(Console.ReadLine(), out symbol) || char.IsWhiteSpace(symbol))
            {
                Console.WriteLine("Invalid symbol! Please enter a valid non-space character:");
            }
            return symbol;
        }
        
        public void AddConfiguration(GameConfiguration config)
        {
            SaveConfiguration(config, config.GameName);
        }
    }
}
