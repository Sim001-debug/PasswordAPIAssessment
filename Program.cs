using PasswordAPIAssessment;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Password Assessment App...");

        string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
        string dictPath = Path.Combine(projectRoot, "dict.txt");

        PasswordGenerator passwordGenerator = new();
        passwordGenerator.GenerateAllPermutations();

        PasswordCracker passwordCracker = new();
        string? uploadUrl = passwordCracker.CrackPassword(dictPath);

        if (string.IsNullOrEmpty(uploadUrl))
        {
            Console.WriteLine("Upload URL is null or empty.");
            return;
        }

        Console.WriteLine($"Upload URL confirmed: {uploadUrl}");

        string cvPath = Path.Combine(projectRoot, "Simbongile Dyi - CV.pdf");

        if (!File.Exists(cvPath))
        {
            Console.WriteLine("Simbongile Dyi CV not found!");
            return;
        }

        Uploader uploader = new();
        uploader.CreateZipAndUpload(uploadUrl, cvPath, projectRoot);
    }
}
