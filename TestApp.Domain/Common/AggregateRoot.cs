using System;

namespace TestApp.Domain
{
    public class AggregateRoot : EntityBase<long>, ICreationTrackable, ILastChangeTrackable
    {
        public long CreatedUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public long ModifiedUserId { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
