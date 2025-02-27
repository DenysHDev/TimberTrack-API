using System.Text.Json;
using TimberAPI.Models;

namespace TimberAPI.Services
{
    public class FakeDataService
    {
        private readonly string _filePath;
        private readonly List<string> _machineIds = ["M-001", "M-002", "M-003", "M-004", "M-005"];

        public FakeDataService()
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "data.json");
        }

        public void AddDailyFakeData()
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("Data file not found, creating new one...");
                File.WriteAllText(_filePath, "[]"); 
            }

            string jsonData = File.ReadAllText(_filePath);
            var data = JsonSerializer.Deserialize<List<MachineLog>>(jsonData) ?? new List<MachineLog>();

            string today = DateTime.UtcNow.ToString("yyyy-MM-dd");

            // If today's data already exists, skip adding new data
            if (data.Any(entry => entry.Date == today))
            {
                Console.WriteLine($"Data for {today} already exists.");
                return;
            }

            Random random = new();
            foreach (var machineId in _machineIds)
            {
                data.Add(new MachineLog
                {
                    Machine_id = machineId,
                    Date = today,
                    Trees_cut = random.Next(30, 71)
                });
            }

            // Save updated data back to file
            File.WriteAllText(_filePath, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));

            Console.WriteLine($"New data for {today} added successfully.");
        }


        public async Task StartDailyUpdateTask(){
            while(true){
                await Task.Delay(TimeSpan.FromDays(1));
                Console.WriteLine("Running scheduled daily data update...");
                AddDailyFakeData();
            }
        }

        public List<MachineLog> GetAllData()
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("Data file not found.");
                return [];
            }

            string jsonData = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<MachineLog>>(jsonData) ?? [];
        }
    }
}
