using System.Text;

namespace PasswordAPIAssessment
{
    // Class responsible for generating permutations of the word "password"
    // using character substitutions to create a dictionary
    public class PasswordGenerator
    {
        // The base word to generate permutations for
        private readonly string passWord = "password";

        // Substitution rules for each character (e.g., 'a' => 'a', 'A', '@')
        private readonly Dictionary<char, List<char>> substitutes = new()
        {
            {'a', new List<char> {'a', 'A', '@'}},
            {'s', new List<char> {'s', 'S', '5'}},
            {'o', new List<char> {'o', 'O', '0'}},
            {'p', new List<char> {'p', 'P'}},
            {'w', new List<char> {'w', 'W'}},
            {'r', new List<char> {'r', 'R'}},
            {'d', new List<char> {'d', 'D'}},
        };

        // List to store all generated password combinations
        private List<string> permutations = new();

        // Public method to start generating permutations and save to dict.txt
        public void GenerateAllPermutations()
        {
            StringBuilder sb = new();

            // Begin recursive generation from the first character
            Recurse(0, sb);

            Console.WriteLine($"Generated {permutations.Count} permutations.");

            // Move up from the bin\Debug\net8.0 directory to the project root
            string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!
                                           .Parent!.Parent!.Parent!.FullName;

            // Set the output file path
            string filePath = Path.Combine(projectRoot, "dict.txt");

            // Write unique permutations to the file
            File.WriteAllLines(filePath, permutations.Distinct());

            Console.WriteLine($"✅ Saved dictionary to: {filePath}");
        }

        // Recursive method to build permutations
        private void Recurse(int index, StringBuilder sb)
        {
            // Base case: reached the end of the password
            if (index == passWord.Length)
            {
                permutations.Add(sb.ToString());
                return;
            }

            // Get the character at the current index
            char currentC = passWord[index];

            // Try each substitution option for the current character
            foreach (char option in substitutes[currentC])
            {
                sb.Append(option);         // Add the character to the current string
                Recurse(index + 1, sb);    // Recurse to the next character
                sb.Length--;               // Backtrack: remove last char for the next loop
            }
        }
    }
}
