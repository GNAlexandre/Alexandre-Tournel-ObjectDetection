using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Alexandre.Tournel.ObjectDetection.Tests;

public class ObjectDetectionUnitTest
{
    [Fact]
    public async Task ObjectShouldBeDetectedCorrectly()
    {
        var executingPath = GetExecutingPath();
        var scenesPath = Path.Combine(executingPath, "Scenes");

        // Ensure the Scenes directory exists
        if (!Directory.Exists(scenesPath))
        {
            Directory.CreateDirectory(scenesPath);
        }
        
        
        
        var imageScenesData = new List<byte[]>();
        foreach (var imagePath in Directory.EnumerateFiles(Path.Combine(executingPath,
                     "Scenes")))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            imageScenesData.Add(imageBytes);
        }
        var detectObjectInScenesResults = await new
            ObjectDetection().DetectObjectInScenesAsync(imageScenesData);
            Assert.Equal("[{\"X\":262,74255\"Y\":106,59372\"Height\":71,61657\"Width\":210,25934}]", JsonSerializer.Serialize(detectObjectInScenesResults[0].Box));
            Assert.Equal("[{\"Dimensions\":{\"X\":262,74255\"Y\":106,59372\"Height\":71,61657\"Width\":210,25934},\"Label\":\"Car\",\"Confidence\":0.5}]", JsonSerializer.Serialize(detectObjectInScenesResults[0].Box));
            Assert.Equal("[{\"Dimensions\":{\"X\":262,74255\"Y\":106,59372\"Height\":71,61657\"Width\":210,25934},\"Label\":\"Car\",\"Confidence\":0.5}]", JsonSerializer.Serialize(detectObjectInScenesResults[1].Box));
    }
    private static string GetExecutingPath()
    {
        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
        var executingPath = Path.GetDirectoryName(executingAssemblyPath);
        return executingPath;
    }
}