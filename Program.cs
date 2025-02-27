using System.Text.Json;
using TimberAPI.Services; 

namespace TimberAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var fakeDataService = new FakeDataService();
            fakeDataService.AddDailyFakeData(); 

            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", async context =>
            {
                var data = fakeDataService.GetAllData();

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
            });

            app.Run("http://0.0.0.0:3000");
        }
    }
}
