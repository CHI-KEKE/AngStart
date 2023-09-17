using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.Marketing
{
    public class Campaign:BaseEntity
    {
        public int Product_Id { get; set; }
        public string Story { get; set; }
        public string Picture { get; set; }
    }
}