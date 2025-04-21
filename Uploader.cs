using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace PasswordAPIAssessment
{
    public class Uploader
    {
        public void CreateZipAndUpload(string uploadUrl, string cvPath, string projectRoot)
        {
            // Define the path for the zip file to be created
            string zipPath = Path.Combine(projectRoot, "submission.zip");

            // List of required .cs files to include in the zip
            string[] requiredCsFiles = new[]
            {
                "Program.cs",
                "Uploader.cs"
            };

            Console.WriteLine($"Upload URL: {uploadUrl}");

            // Create zip archive
            using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Create))
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
            {
                // Add CV file to the zip
                archive.CreateEntryFromFile(cvPath, Path.GetFileName(cvPath));

                // Add dict.txt if it exists
                string dictPath = Path.Combine(projectRoot, "dict.txt");
                if (File.Exists(dictPath))
                    archive.CreateEntryFromFile(dictPath, "dict.txt");

                // Add required .cs files to the zip
                foreach (string fileName in requiredCsFiles)
                {
                    string filePath = Path.Combine(projectRoot, fileName);
                    if (File.Exists(filePath))
                        archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
                }
            }

            // Verify that all required files are in the zip
            if (!VerifyZip(zipPath, requiredCsFiles, Path.GetFileName(cvPath)))
            {
                Console.WriteLine("\n❌ ZIP verification failed.");
                return;
            }

            Console.WriteLine("\n✅ ZIP verified. Uploading...");

            // Read the zip as bytes and convert to Base64 string
            byte[] fileBytes = File.ReadAllBytes(zipPath);
            string base64Zip = Convert.ToBase64String(fileBytes);

            // Create the payload object to send in the request
            var payload = new
            {
                Data = base64Zip,
                Name = "Simbongile",
                Surname = "Dyi",
                Email = "nwabisamxa@gmail.com"
            };

            // Serialize payload to JSON
            string json = JsonSerializer.Serialize(payload);

            // Send HTTP POST request
            using HttpClient client = new HttpClient();
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(uploadUrl, content).Result;

            // Handle response
            
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("✅ Upload successful!");
            }
            else
            {
                Console.WriteLine($"❌ Upload failed: {response.StatusCode}");
                string responseBody = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine($"Upload error response body: {responseBody}");
            }
        }

        // Verify the contents of the created zip file
        private bool VerifyZip(string zipPath, string[] requiredCsFiles, string cvFileName)
        {
            bool hasDict = false;
            bool hasCV = false;
            HashSet<string> foundCsFiles = new();

            using (ZipArchive zip = ZipFile.OpenRead(zipPath))
            {
                foreach (var entry in zip.Entries)
                {
                    if (entry.FullName.Equals("dict.txt", StringComparison.OrdinalIgnoreCase))
                        hasDict = true;

                    if (entry.FullName.Equals(cvFileName, StringComparison.OrdinalIgnoreCase))
                        hasCV = true;

                    if (requiredCsFiles.Contains(entry.FullName))
                        foundCsFiles.Add(entry.FullName);
                }
            }

            // Return true only if all required files are found
            return hasDict && hasCV && foundCsFiles.Count == requiredCsFiles.Length;
        }
    }
}
