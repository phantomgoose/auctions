using System;

namespace netbelt.Models {
    public abstract class BaseEntity {
        public int ID {get; set;}
        public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
        public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;
    }
}