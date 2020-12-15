using BlazorApp1.Data.entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorApp1.Data.dao
{
    [Serializable]
    public class ToyStorage
    {
        public int CurrentId { get; set; }

        public List<ToyEntity> Toys { get; set; }
    }

    public class ToysDao
    {
        static private readonly string DATABASE_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "data.txt");

        public static async Task<ToyStorage> ReadToys()
        {
            if (!File.Exists(DATABASE_PATH))
            {
                await CreateTable();
            }

            using FileStream fs = new FileStream(DATABASE_PATH, FileMode.OpenOrCreate);
            var toys = await JsonSerializer.DeserializeAsync<ToyStorage>(fs);

            return toys;
        }
        
        private static async Task CreateTable()
        {
            var storage = new ToyStorage() { CurrentId = 0, Toys = new List<ToyEntity>() };

            await SaveToys(storage);
        }

        public static async Task SaveToys(ToyStorage toys)
        {
            string serializedToys = JsonSerializer.Serialize(toys);

            await File.WriteAllTextAsync(DATABASE_PATH, serializedToys);
        }
        
    }
}
