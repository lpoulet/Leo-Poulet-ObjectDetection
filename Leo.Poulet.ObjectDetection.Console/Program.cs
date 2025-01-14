using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Leo.Poulet.ObjectDetection;

namespace Leo.Poulet.ObjectDetection.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                System.Console.WriteLine("Usage: dotnet run <scenesDirectoryPath>");
                return;
            }

            var scenesDirectoryPath = args[0];
            
            if (!Directory.Exists(scenesDirectoryPath))
            {
                System.Console.WriteLine($"Error: Directory '{scenesDirectoryPath}' not found.");
                return;
            }
            
            var sceneImagePaths = Directory.GetFiles(scenesDirectoryPath);
            var sceneImageData = new List<byte[]>();

            foreach (var sceneImagePath in sceneImagePaths)
            {
                var imageData = await File.ReadAllBytesAsync(sceneImagePath);
                sceneImageData.Add(imageData);
            }
            
            var objectDetection = new ObjectDetection();
            
            var detectObjectInScenesResults = await objectDetection.DetectObjectInScenesAsync(sceneImageData);
            
            foreach (var objectDetectionResult in detectObjectInScenesResults)
            {
                System.Console.WriteLine($"Box: {JsonSerializer.Serialize(objectDetectionResult.Box)}");
            }
        }
    }
}