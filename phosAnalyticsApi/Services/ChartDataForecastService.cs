using Microsoft.ML;

namespace phosAnalyticsApi.Services
{
    public class ChartDataForecastService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _trainedModel;

        public ChartDataForecastService()
        {
            _mlContext = new MLContext(seed: 0);
            _trainedModel = null;
        }

    }
}
