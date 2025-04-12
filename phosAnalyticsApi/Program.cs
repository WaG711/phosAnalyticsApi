using Microsoft.EntityFrameworkCore;
using phosAnalyticsApi.IRepositories;
using phosAnalyticsApi.Repositories;
using phosAnalyticsApi.Services;

namespace phosAnalyticsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<phosAnalyticsApiContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("phosAnalyticsApiContext") ?? throw new InvalidOperationException("Connection string 'phosAnalyticsApiContext' not found.")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IChartDataRpstr, ChartDataRpstr>();
            builder.Services.AddScoped<ChartDataForecastService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
