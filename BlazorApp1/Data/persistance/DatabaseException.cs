using System;

namespace BlazorApp1.Data.persistance
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message) { }
    }
}
