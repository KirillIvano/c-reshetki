using BlazorApp1.Data.dto;
using BlazorApp1.Data.services.interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp1.Data
{
    public class CarService
    {
        private ICarsDatabase CarDB;
        private IDriversDatabase DriverDB;

        public CarService(
            ICarsDatabase carDB,
            IDriversDatabase driverDB
        ) {
            CarDB = carDB;
            DriverDB = driverDB;
        }

        public async Task<List<CarFullInfoDto>> GetCarsFullInfo(string model, string color)
        {
            var rawCars = CarDB.GetAllCars();

            foreach (var each in rawCars)
            {
                System.Console.Out.WriteLine(model + " " + color);
                System.Console.Out.WriteLine(each.Model + " " + each.Color);
            }

            var cars = await Task.FromResult(
                rawCars
                    .Where(car => car.Model.Equals(model) && car.Color.Equals(color))
                    .ToList()
                    .ConvertAll<CarFullInfoDto>(car =>
                        {
                            DriverDto driver = DriverDB[car.DriverId];

                            return new CarFullInfoDto() {
                                Id = car.Id,
                                Color = car.Color,
                                Model = car.Model,
                                DriverId = car.DriverId,
                                DriverAddress = driver.Address,
                                DriverName = driver.Name
                            };
                        }
                    )
            );
            
            return cars;
        }

        public async Task DeleteCar(int carId)
        {
            await CarDB.DeleteCar(carId);
        }

        public async Task UpdateCar(UpdateCarDto updateDto)
        {
            try
            {
                var driver = DriverDB[updateDto.DriverId];
            }
            catch
            {
                throw new KeyNotFoundException("Водителя с таким id не существует");
            }

            await CarDB.UpdateCar(updateDto);
        }

        public async Task CreateCar(CreateCarDto createCarDto)
        {
            try
            {
                var driver = DriverDB[createCarDto.DriverId];
            } catch
            {
                throw new KeyNotFoundException("Водителя с таким id не существует");
            }

            await CarDB.CreateCar(createCarDto);
        }

        public Task<List<CarDto>> GetCars()
        {
            return Task.FromResult(CarDB.GetAllCars());
        }

        public Task<CarDto> GetCarById(int toyId)
        {
            return Task.FromResult(CarDB[toyId]);
        }
    }
}
