using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ObjectDetection;

namespace Leo.Poulet.ObjectDetection
{
    public class ObjectDetection
    {
        public async Task<IList<ObjectDetectionResult>> DetectObjectInScenesAsync(IList<byte[]> imagesSceneData)
        {
            // Récupère le chemin du modèle Yolo
            string modelPath = Path.Combine(AppContext.BaseDirectory, "Models", "TinyYolo2_model.onnx");

            // Instancie Yolo
            var tinyYolo = new Yolo();

            // Traite chaque image en parallèle
            var tasks = imagesSceneData.Select(image =>
                Task.Run(() =>
                {
                    // Configure Yolo pour utiliser le modèle
                    var detectionOutput = tinyYolo.Detect(image);

                    // Retourne le résultat au format attendu
                    return new ObjectDetectionResult
                    {
                        ImageData = detectionOutput.ImageData,
                        Box = detectionOutput.Boxes.Select(box => new BoundingBox
                        {
                            Confidence = box.Confidence,
                            Label = box.Label,
                            Dimensions = new BoundingBoxDimensions
                            {
                                X = (int)box.Dimensions.X,
                                Y = (int)box.Dimensions.Y,
                                Width = (int)box.Dimensions.Width,
                                Height = (int)box.Dimensions.Height
                            }
                        }).ToList()
                    };
                })
            );

            // Attend que tous les traitements parallèles soient terminés
            var results = await Task.WhenAll(tasks);

            return results.ToList();
        }

    }
    
    public class BoundingBox
    {
        public float Confidence { get; set; }
        public string Label { get; set; }
        public BoundingBoxDimensions Dimensions { get; set; }
    }

    public class BoundingBoxDimensions
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
