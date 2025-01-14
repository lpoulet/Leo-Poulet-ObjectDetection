using System.Collections.Generic; 
using System.IO; 
using System.Reflection; 
using System.Text.Json; 
using System.Threading.Tasks; 
using Xunit;   
namespace Leo.Poulet.ObjectDetection.Tests;
public class ObjectDetectionUnitTest {
    [Fact]     
    public async Task ObjectShouldBeDetectedCorrectly()     
    {         
        var executingPath = GetExecutingPath();         
        var imageScenesData = new List<byte[]>();
        foreach (var imagePath in Directory.EnumerateFiles(Path.Combine(executingPath, "Scenes")))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);             
            imageScenesData.Add(imageBytes);
        }           
        var detectObjectInScenesResults = await new 
            ObjectDetection().DetectObjectInScenesAsync(imageScenesData); 
        
        Assert.Equal(
            "[{\"Confidence\":0.7687561,\"Label\":\"person\",\"Dimensions\":{\"X\":75,\"Y\":119,\"Width\":188,\"Height\":298}}]",
            JsonSerializer.Serialize(detectObjectInScenesResults[0].Box)
        );

    //    Assert.Equal(
    //        "[{\"Confidence\":0.59720665,\"Label\":\"chair\",\"Dimensions\":{\"X\":80,\"Y\":65,\"Width\":191,\"Height\":279}},{\"Confidence\":0.4045571,\"Label\":\"chair\",\"Dimensions\":{\"X\":249,\"Y\":137,\"Width\":169,\"Height\":263}}]",
    //        JsonSerializer.Serialize(detectObjectInScenesResults[1].Box)
    //    );
    }

    private static string GetExecutingPath()
    {
        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;         
        var executingPath = Path.GetDirectoryName(executingAssemblyPath);         
        return executingPath;
    } 
}