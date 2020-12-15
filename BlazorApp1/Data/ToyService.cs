using BlazorApp1.Data.dto;
using BlazorApp1.Data.persistance;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp1.Data
{
    public class ToyService
    {
        ToyDatabase ToyDB;

        public async Task Initialize()
        {
            ToyDB = new ToyDatabase();

            await ToyDB.Connect();
        }

        public async Task<List<ToyDto>> GetToyByParams(int age, int price)
        {
            var toys = ToyDB.GetAllToys();

            var res = await Task.FromResult(toys.Where(toy => toy.Age <= age && toy.Price <= price).ToList());
            System.Console.Out.WriteLine(res.Count);

            return res;
        }

        public async Task DeleteToy(int toyId)
        {
            await ToyDB.DeleteToy(toyId);
        }

        public async Task UpdateToy(UpdateToyDto updateDto)
        {
            await ToyDB.UpdateToy(updateDto);
        }

        public async Task CreateToy(string name, int age, int price)
        {
            await ToyDB.CreateToy(new CreateToyDto() { 
                Name = name,
                Age = age,
                Price = price
            });
        }

        public Task<List<ToyDto>> GetToys()
        {
            return Task.FromResult(ToyDB.GetAllToys());
        }

        public Task<ToyDto> GetToyById(int toyId)
        {
            return Task.FromResult(ToyDB[toyId]);
        }
    }
}
