using Newtonsoft.Json;

namespace EnemyReader_JSON
{
    internal class Program
    {
        static void Main()
        {
            string[] filenames = new string[]
            {
                "Goblin_Warrior.json",
                "Goblin_Archer.json",
                "Goblin_Mage.json"
            };
            foreach (string name in filenames)
            {
                Enemy enemy = JsonToEnemy(name);
                if (enemy.name.Length > 0)
                {
                    Console.WriteLine(enemy.ToString());
                }
            }
        }
        /// <summary>
        /// Reads given file and tries to convert it to Enemy object.
        /// </summary>
        /// <param name="filename">Name of the JSON file</param>
        /// <returns>Enemy object. The default Enemy is returned on error.</returns>
        public static Enemy JsonToEnemy(string filename)
        {
            // Create a default empty enemy
            Enemy enemy = new Enemy();
            // Try to open the file and read the contents
            if (File.Exists(filename) == false)
            {
                PrintError($"Could not read file {filename}");
            }
            else
            {
                // Product deserializedProduct = JsonConvert.DeserializeObject<Product>(output);
                string text = File.ReadAllText(filename);

                // If the JSON data is not valid, an exception is thrown
                PrintLog($">>> Reading {filename}");
                try
                {
                    enemy = JsonConvert.DeserializeObject<Enemy>(text);
                }
                catch (JsonReaderException exp)
                {
                    PrintError($"Error in file {filename} at {exp.LineNumber}:{exp.LinePosition}");
                    Console.WriteLine(exp.Message);
                }
            }
            return enemy;
        }
        public static void PrintError(string text)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public static void PrintLog(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
