using BlazorApp1.Data.entities;
using BlazorApp1.Data.shared;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorApp1.Data.dao
{
    public class DriversDao
    {
        static private readonly string DATABASE_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "drivers.txt");

        public static async Task<Storage<DriverEntity>> ReadDrivers()
        {
            if (!File.Exists(DATABASE_PATH))
            {
                await CreateTable();
            }

            var fileContent = await File.ReadAllTextAsync(DATABASE_PATH, Encoding.UTF8);
            var drivers = JsonSerializer.Deserialize<Storage<DriverEntity>>(fileContent);

            return drivers;
        }

        private static async Task CreateTable()
        {
            var storage = new Storage<DriverEntity>();

            await SaveDrivers(storage);
        }

        public static async Task SaveDrivers(Storage<DriverEntity> toys)
        {
            string serializedToys = JsonSerializer.Serialize(toys);

            await File.WriteAllTextAsync(DATABASE_PATH, serializedToys, Encoding.UTF8);
        }
    }
}
