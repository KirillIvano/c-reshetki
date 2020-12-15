using BlazorApp1.Data.dto;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BlazorApp1.Data.services.interfaces
{
    public interface ICarsDatabase
    {
        CarDto this[int carId]
        {
            get;
        }

        public List<CarDto> GetAllCars();
        public Task<CarDto> CreateCar(CreateCarDto createCarDto);
        public Task<CarDto> UpdateCar(UpdateCarDto updateCarDto);
        public Task DeleteCar(int carId);

        public Task Connect();
    }
}
