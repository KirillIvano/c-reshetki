using System;

namespace BlazorApp1.Data.entities
{

    [Serializable]
    public class DriverEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
