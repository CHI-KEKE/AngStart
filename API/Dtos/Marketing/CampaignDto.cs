using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Marketing
{
    public class CampaignDto
    {
        public int Product_Id { get; set; }
        public string Story { get; set; }
        public IFormFile? Picture { get; set; }
    }
}