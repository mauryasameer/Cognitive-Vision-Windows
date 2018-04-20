using System;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

namespace VisionAPI_MVC_Sample.Controllers
{
    public class HomeController : Controller
    {

        VisionServiceClient visionClient = new VisionServiceClient(
                "xxxx",
                "xxxx"
                );


        public async Task<ActionResult> AnalyzeInDomain()
        {
            var result = await visionClient.AnalyzeImageInDomainAsync(
                "https://i.ndtvimg.com/i/2017-10/taj-mahal-unsplash-650_650x400_51508150014.jpg",
                "landmarks"
                );

            var result1 = await visionClient.AnalyzeImageInDomainAsync(
                "https://i.ndtvimg.com/i/2016-10/amitabh-bachchan_650x400_61476137341.jpg",
                "celebrities"
                );

            return View();
        }



        public async Task<ActionResult> Analyze()
        {
            VisualFeature[] features = new VisualFeature[] {
                VisualFeature.Adult,
                VisualFeature.Categories,
                VisualFeature.Color,
                VisualFeature.Description,
                VisualFeature.Faces,
                VisualFeature.ImageType,
                VisualFeature.Tags
            };

            var result = await visionClient.AnalyzeImageAsync(
                "http://cricket361.com/wp-content/uploads/2014/10/test-cricket.jpg",
                features
                );

            return View();
        }



        public async Task<ActionResult> Describe()
        {
            var result = await visionClient.DescribeAsync(
                "http://ste.india.com/sites/default/files/2016/01/22/453182-sachin-amitabh-ipl-smile-7.jpg"
                );

            return View();
        }



        public async Task<ActionResult> HandwritingRecognition()
        {
            var result = await HandwritingRecognitionTask(async (VisionServiceClient VisionServiceClient)
                => await VisionServiceClient.CreateHandwritingRecognitionOperationAsync(
                "https://www.nayuki.io/res/overwriting-confidential-handwritten-text/overwriting-handwriting.jpg"
                ));

            return View();
        }

        private async Task<HandwritingRecognitionOperationResult> HandwritingRecognitionTask(Func<VisionServiceClient, Task<HandwritingRecognitionOperation>> Func)
        {
            int MaxRetryTimes = 3;

            HandwritingRecognitionOperationResult result;

            try
            {
                HandwritingRecognitionOperation operation = await Func(visionClient);

                result = await visionClient.GetHandwritingRecognitionOperationResultAsync(operation);

                int i = 0;
                while ((result.Status == HandwritingRecognitionOperationStatus.Running || result.Status == HandwritingRecognitionOperationStatus.NotStarted) && i++ < MaxRetryTimes)
                {
                    result = await visionClient.GetHandwritingRecognitionOperationResultAsync(operation);
                }

            }
            catch (ClientException ex)
            {
                result = new HandwritingRecognitionOperationResult() { Status = HandwritingRecognitionOperationStatus.Failed };
            }

            return result;
        }



        public async Task<ActionResult> OCR()
        {
            var result = await visionClient.RecognizeTextAsync(
                "https://oxfordportal.blob.core.windows.net/vision/OpticalCharacterRecognition/6-1.jpg",
                "en"
                );

            return View();
        }



        public async Task<ActionResult> Tags()
        {
            var result = await visionClient.GetTagsAsync(
                "https://oxfordportal.blob.core.windows.net/vision/Analysis/11-1.jpg"
                );

            return View();
        }


        public async Task<ActionResult> Thumbnail()
        {
            var result = await visionClient.GetThumbnailAsync(
                "https://oxfordportal.blob.core.windows.net/vision/Thumbnail/6-1.jpg",
                50,
                50
                );

            var result1 = await visionClient.GetThumbnailAsync(
                "https://oxfordportal.blob.core.windows.net/vision/Thumbnail/6-1.jpg",
                100,
                100
                );

            return View();
        }
    }
}