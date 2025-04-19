using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using phosAnalyticsApi.Models;

namespace phosAnalyticsApi.Services
{
    public class ChartDataForecastService
    {
        private readonly MLContext _mlContext;

        public ChartDataForecastService()
        {
            _mlContext = new MLContext(seed: 0);
        }

        public List<ChartPoint> SimpleForecast(ChartData aggregatedData, int horizon = 30)
        {
            if (aggregatedData?.Points == null || aggregatedData.Points.Count == 0)
            {
                return [];
            }

            var orderedPoints = aggregatedData.Points.OrderBy(p => p.Date).ToList();

            var dataView = _mlContext.Data.LoadFromEnumerable(
                orderedPoints.Select(p => new TimeSeriesData
                {
                    Date = p.Date,
                    Value = (float)p.Value
                })
            );

            var pipeline = _mlContext.Forecasting.ForecastBySsa(
                outputColumnName: "Forecast",
                inputColumnName: "Value",
                windowSize: 7,
                seriesLength: orderedPoints.Count,
                trainSize: orderedPoints.Count,
                horizon: horizon
            );

            var model = pipeline.Fit(dataView);

            var forecastEngine = model.CreateTimeSeriesEngine<TimeSeriesData, SimpleForecastResult>(_mlContext);

            var forecast = forecastEngine.Predict();

            var lastDate = orderedPoints.Last().Date;
            return Enumerable.Range(1, horizon)
                .Select(i => new ChartPoint
                {
                    ChartPointId = Guid.NewGuid(),
                    Date = lastDate.AddDays(i),
                    Value = forecast.Forecast[i - 1]
                })
                .ToList();
        }

        public class SimpleForecastResult
        {
            public float[] Forecast { get; set; }
        }

        public class TimeSeriesData
        {
            public DateTime Date { get; set; }
            public float Value { get; set; }
        }
    }
}
