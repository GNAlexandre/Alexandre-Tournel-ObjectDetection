using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var executingPath = GetExecutingPath();
        var scenesPath = Path.Combine(executingPath, "Scenes");

        // Ensure the Scenes directory exists
        if (!Directory.Exists(scenesPath))
        {
            Directory.CreateDirectory(scenesPath);
        }

        var imageScenesData = new List<byte[]>();
        foreach (var imagePath in Directory.EnumerateFiles(scenesPath))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            imageScenesData.Add(imageBytes);
        }

        var detectObjectInScenesResults = await new ObjectDetection().DetectObjectInScenesAsync(imageScenesData);
        foreach (var objectDetectionResult in detectObjectInScenesResults)
        {
            Console.WriteLine($"Box:{JsonSerializer.Serialize(objectDetectionResult.Box)}");
        }
    }

    private static string GetExecutingPath()
    {
        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
        var executingPath = Path.GetDirectoryName(executingAssemblyPath);
        return executingPath;
    }
}