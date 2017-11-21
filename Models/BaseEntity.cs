using System;

namespace Auctions.Models {
    public abstract class BaseEntity {
        public int ID {get; set;}
        // all times are in UTC to avoid messing with time zone conversion.
        public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
        public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;
    }
}