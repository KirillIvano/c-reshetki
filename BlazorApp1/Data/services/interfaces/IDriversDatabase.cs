using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp1.Data.dto;

namespace BlazorApp1.Data.services.interfaces
{
    public interface IDriversDatabase
    {
        DriverDto this[int driverId]
        {
            get;
        }

        public List<DriverDto> GetAllDrivers();
        public Task<DriverDto> CreateDriver(CreateDriverDto createDriverDto);
        public Task<DriverDto> UpdateDriver(UpdateDriverDto updateDriverDto);
        public Task DeleteDriver(int driverId);

        public Task Connect();
    }
}
