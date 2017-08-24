using System;

namespace TIROTAPI.Models.ActivityLogViewModels
{
    public class WISE4012ViewModel
    {
        public string PE { get; set; }
        public string UID { get; set; }
        public string MAC { get; set; }
        public DateTime TIM { get; set; }
        public double[,] Record { get; set; }
    }
}
