using ObjectDetection;
using System.Threading.Tasks;
using System.Linq;
using System;
namespace Alexandre.Tournel.ObjectDetection;




public class ObjectDetection
{
    public async Task<IList<ObjectDetectionResult>>
        DetectObjectInScenesAsync(IList<byte[]> imagesSceneData)
    {
        await Task.Delay(1000);
// TODO implement your code here

        var tasks = imagesSceneData.Select(imageData => Task.Run(() =>
        {
            var tinyYolo = new Yolo();
            var result = tinyYolo.Detect(imageData);
            return new ObjectDetectionResult
            {
                ImageData = imageData,
                Box = result.Boxes.Select(box => new BoundingBox
                {
                    X = box.Dimensions.X,
                    Y = box.Dimensions.Y,
                    Width = box.Dimensions.Width,
                    Height = box.Dimensions.Height
                }).ToList()
            };
        })).ToList();

        return await Task.WhenAll(tasks);
     
        throw new NotImplementedException();
    }
}
