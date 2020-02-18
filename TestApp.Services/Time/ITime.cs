using System;

namespace TestApp.Services.Time
{
    public interface ITime
    {
        DateTime Now { get; }
    }

    public class Time : ITime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
