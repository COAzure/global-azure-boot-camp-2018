using Microsoft.ProjectOxford.Vision.Contract;

namespace FunctionApp1.Models
{
    public class ImageInfo
    {
        public string Id { get; set; }
        public string ImagePath { get; set; }
        public AnalysisResult Analysis { get; set; }
    }
}