using BlazorApp1.Data.dao;
using BlazorApp1.Data.dto;
using BlazorApp1.Data.entities;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp1.Data.persistance
{
    public class DatabaseException: Exception {
        public DatabaseException(string message): base(message) {}
    }

    public class ToyDatabase
    {
        private ToyStorage ToysTable;

        public bool IsConnected {get; set; }

        public async Task Connect()
        {
            ToysTable = await ToysDao.ReadToys();
        }

        private ToyEntity GetToyById(int toyId)
        {
            return ToysTable.Toys.Find(toy => toy.Id == toyId);
        }

        public List<ToyDto> GetAllToys()
        {
            return ToysTable.Toys.ConvertAll(toy => ClientifyEntity(toy));
        }

        private int GetToyIndById(int toyId)
        {
            return ToysTable.Toys.FindIndex(toy => toy.Id == toyId);
        }

        public ToyDto this[int toyId]
        {
            get
            {
                var toy = GetToyById(toyId);

                if (toy != null)
                {
                    return ClientifyEntity(toy);
                }
                else
                {
                    throw new DatabaseException($"Игрушка с id = {toyId} не найдена");
                }
            }
        }

        public async Task DeleteToy(int toyId)
        {
            var toy = GetToyById(toyId);

            if (toy != null)
            {
                ToysTable.Toys.Remove(toy);
                await UpdateDatabase();
            } else
            {
                throw new DatabaseException($"Игрушка с id = {toyId} не найдена");
            }
        }


        public async Task<ToyDto> UpdateToy(UpdateToyDto updateToyDto)
        {
            var toyInd = GetToyIndById(updateToyDto.Id);

            if (toyInd != -1)
            {
                var updatedToy = new ToyEntity()
                {
                    Id = updateToyDto.Id,
                    Name = updateToyDto.Name,
                    Age = updateToyDto.Age,
                    Price = updateToyDto.Price
                };

                ToysTable.Toys[toyInd] = updatedToy;
                await UpdateDatabase();

                return ClientifyEntity(updatedToy);
            } else
            {
                throw new DatabaseException("Такой элемент не найден");
            }
        }

        public async Task<ToyDto> CreateToy(CreateToyDto createToyDto)
        {
            var toy = new ToyEntity() { 
                Id = GetNewId(),
                Name = createToyDto.Name,
                Age = createToyDto.Age,
                Price = createToyDto.Price
            };

            ToysTable.Toys.Add(toy);
            Console.Out.WriteLine(ToysTable.Toys.Count);

            await UpdateDatabase();

            return ClientifyEntity(toy);
        }

        private int GetNewId()
        {
            var id = ToysTable.CurrentId;

            ToysTable.CurrentId++;

            return id;
        }

        private async Task UpdateDatabase()
        {
            await ToysDao.SaveToys(this.ToysTable);
        }

        private static ToyDto ClientifyEntity(ToyEntity toy)
        {
            return new ToyDto()
            {
                Name = toy.Name,
                Age = toy.Age,
                Id = toy.Id,
                Price = toy.Price
            };
        }
    }
}
