using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        //TODO: Maybe add information about who created the item and who last updated it (needs to read from EntraID)
        
        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDeleted { get; set; } 
    }
}
