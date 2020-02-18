using System;

namespace TestApp.Domain
{
    public interface ILastChangeTrackable
    {
        DateTime ModifiedDateTime { get; set; }

        long ModifiedUserId { get; set; }        
    }
}
