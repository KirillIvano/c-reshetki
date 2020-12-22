using BlazorApp1.Data.dao;
using BlazorApp1.Data.dto;
using BlazorApp1.Data.entities;
using BlazorApp1.Data.services.interfaces;
using BlazorApp1.Data.shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp1.Data.persistance
{

    public class CarDatabase : ICarsDatabase
    {
        private Storage<CarEntity> CarsTable;
        private readonly CarsDao carsDao;

        public CarDatabase()
        {
            this.carsDao = new CarsDao();
        }

        public async Task Connect()
        {
            CarsTable = await carsDao.ReadCars();
        }

        private CarEntity GetCarById(int carId)
        {
            return CarsTable.Rows.Find(car => car.Id == carId);
        }

        public List<CarDto> GetAllCars()
        {
            return CarsTable.Rows.ConvertAll(car => ClientifyCar(car));
        }

        private int GetCarIndById(int carId)
        {
            return CarsTable.Rows.FindIndex(car => car.Id == carId);
        }

        public CarDto this[int carId]
        {
            get
            {
                var car = GetCarById(carId);

                if (car != null)
                {
                    return ClientifyCar(car);
                }
                else
                {
                    throw new DatabaseException($"Игрушка с id = {carId} не найдена");
                }
            }
        }

        public async Task DeleteCar(int carId)
        {
            var car = GetCarById(carId);

            if (car != null)
            {
                CarsTable.Rows.Remove(car);
                await UpdateDatabase();
            } else
            {
                throw new DatabaseException($"Машина с id = {carId} не найдена");
            }
        }


        public async Task<CarDto> UpdateCar(UpdateCarDto updateCarDto)
        {
            var carInd = GetCarIndById(updateCarDto.Id);

            if (carInd != -1)
            {
                var updatedCar = new CarEntity()
                {
                    Id = updateCarDto.Id,
                    Color = updateCarDto.Color,
                    Model = updateCarDto.Model,
                    DriverId = updateCarDto.DriverId
                };

                CarsTable.Rows[carInd] = updatedCar;
                await UpdateDatabase();

                return ClientifyCar(updatedCar);
            } else
            {
                throw new DatabaseException("Такой элемент не найден");
            }
        }

        public async Task<CarDto> CreateCar(CreateCarDto createCarDto)
        {
            var car = new CarEntity() { 
                Id = GetNewId(),
                Color = createCarDto.Color,
                Model = createCarDto.Model,
                DriverId = createCarDto.DriverId
            };

            CarsTable.Rows.Add(car);
            Console.Out.WriteLine(CarsTable.Rows.Count);

            await UpdateDatabase();

            return ClientifyCar(car);
        }

        private int GetNewId()
        {
            var id = CarsTable.CurrentId;

            CarsTable.CurrentId++;

            return id;
        }

        private async Task UpdateDatabase()
        {
            await carsDao.SaveCars(CarsTable);
        }

        private static CarDto ClientifyCar(CarEntity car)
        {
            return new CarDto()
            {
                Id = car.Id,
                Color = car.Color,
                Model = car.Model,
                DriverId = car.DriverId
            };
        }
    }
}
