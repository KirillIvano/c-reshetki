using BlazorApp1.Data.dto;
using BlazorApp1.Data.services.interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp1.Data
{
    public class DriverService
    {
        private IDriversDatabase DriverDB;

        public DriverService(
            IDriversDatabase driverDB
        )
        {
            DriverDB = driverDB;
        }

        public async Task DeleteDriver(int carId)
        {
            await DriverDB.DeleteDriver(carId);
        }

        public async Task UpdateDriver(UpdateDriverDto updateDto)
        {
            await DriverDB.UpdateDriver(updateDto);
        }

        public async Task CreateDriver(CreateDriverDto createDriverDto)
        {
            await DriverDB.CreateDriver(createDriverDto);
        }

        public Task<List<DriverDto>> GetDrivers()
        {
            return Task.FromResult(DriverDB.GetAllDrivers());
        }

        public Task<DriverDto> GetDriverById(int toyId)
        {
            return Task.FromResult(DriverDB[toyId]);
        }
    }
}
