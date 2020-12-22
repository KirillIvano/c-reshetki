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

    public class DriverDatabase : IDriversDatabase
    {
        private Storage<DriverEntity> DriversTable;
        private DriversDao driversDao;

        public DriverDatabase()
        {
            this.driversDao = new DriversDao();
        }

        public async Task Connect()
        {
            DriversTable = await this.driversDao.ReadDrivers();
        }

        private DriverEntity GetDriverById(int driverId)
        {
            return DriversTable.Rows.Find(driver => driver.Id == driverId);
        }

        public List<DriverDto> GetAllDrivers()
        {
            return DriversTable.Rows.ConvertAll(driver => ClientifyDriver(driver));
        }

        private int GetDriverIndById(int driverId)
        {
            return DriversTable.Rows.FindIndex(driver => driver.Id == driverId);
        }

        public DriverDto this[int driverId]
        {
            get
            {
                var driver = GetDriverById(driverId);

                if (driver != null)
                {
                    return ClientifyDriver(driver);
                }
                else
                {
                    throw new DatabaseException($"Водитель с id = {driverId} не найдена");
                }
            }
        }

        public async Task DeleteDriver(int driverId)
        {
            var driver = GetDriverById(driverId);

            if (driver != null)
            {
                DriversTable.Rows.Remove(driver);
                await UpdateDatabase();
            } else
            {
                throw new DatabaseException($"Водитель с id = {driverId} не найдена");
            }
        }


        public async Task<DriverDto> UpdateDriver(UpdateDriverDto updateDriverDto)
        {
            var driverInd = GetDriverIndById(updateDriverDto.Id);

            if (driverInd != -1)
            {
                var updatedDriver = new DriverEntity()
                {
                    Id = updateDriverDto.Id,
                    Name = updateDriverDto.Name,
                    Address = updateDriverDto.Address,
                };

                DriversTable.Rows[driverInd] = updatedDriver;
                await UpdateDatabase();

                return ClientifyDriver(updatedDriver);
            } else
            {
                throw new DatabaseException("Такой водитель не найден");
            }
        }

        public async Task<DriverDto> CreateDriver(CreateDriverDto createDriverDto)
        {
            var toy = new DriverEntity() { 
                Id = GetNewId(),
                Name = createDriverDto.Name,
                Address = createDriverDto.Address,
            };

            DriversTable.Rows.Add(toy);

            await UpdateDatabase();

            return ClientifyDriver(toy);
        }

        private int GetNewId()
        {
            var id = DriversTable.CurrentId;

            DriversTable.CurrentId++;

            return id;
        }

        private async Task UpdateDatabase()
        {
            await this.driversDao.SaveDrivers(DriversTable);
        }

        private static DriverDto ClientifyDriver(DriverEntity driver)
        {
            return new DriverDto()
            {
                Id = driver.Id,
                Name = driver.Name,
                Address = driver.Address,
            };
        }
    }
}
