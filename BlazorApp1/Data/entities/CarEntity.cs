using System;

namespace BlazorApp1.Data.entities
{

    [Serializable]
    public class CarEntity
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public string Model { get; set; }
        public int DriverId { get; set; }
    }
}
