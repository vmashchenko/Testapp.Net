using System;

namespace TestApp.Domain
{
    public interface ICreationTrackable
    {
        DateTime CreatedDateTime { get; set; }

        long CreatedUserId { get; set; }        
    }
}
