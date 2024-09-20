using Python.Runtime;
using System.Diagnostics;

public class EmbeddingService : IDisposable
{
    private readonly string _scriptPath;
    private readonly string _workingDirectory;

    public EmbeddingService(string scriptPath, string workingDirectory)
    {
        _scriptPath = scriptPath;
        _workingDirectory = workingDirectory;

        PythonEngine.PythonHome = @"C:\Program Files\Python38"; // Set Python home to your virtual environment
        PythonEngine.Initialize(); // Initialize Python Runtime
    }

    public float[] GetEmbedding(string inputText)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = @"C:\Program Files\Python38\python.exe",  // Path to Python executable
                Arguments = $"\"{_scriptPath}\" \"{inputText}\"",  // Path to Python script
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = _workingDirectory  // Set working directory to IIS application path
            };

            using (var process = Process.Start(psi))
            {
                using (var outputReader = process.StandardOutput)
                using (var errorReader = process.StandardError)
                {
                    var output = outputReader.ReadToEnd();
                    var error = errorReader.ReadToEnd();
                    process.WaitForExit();

                    // Log standard error output for debugging
                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine($"Python script error: {error}");
                    }

                    if (string.IsNullOrEmpty(output))
                    {
                        Console.WriteLine("No output received from Python script.");
                        return null;
                    }

                    // Parse the output
                    var embeddings = Newtonsoft.Json.JsonConvert.DeserializeObject<float[]>(output);
                    return embeddings;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
            Console.WriteLine($"Error in GetEmbedding: {ex.Message}");
            return null;
        }
    }

    public void Dispose()
    {
        // Shutdown the Python runtime
        PythonEngine.Shutdown();
    }
}
