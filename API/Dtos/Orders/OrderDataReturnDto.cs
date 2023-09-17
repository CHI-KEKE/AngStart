using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Orders
{
    public class OrderDataReturnDto
    {
        public long Revenue { get; set; }
        // public int SilerCount { get; set; }
        // // public decimal SilerPercent { get; set; }
        // public int GreenCount { get; set; }
        // // public decimal GreenPercent { get; set; }
        // public int AquaCount { get; set; }
        // // public decimal AquaPercent { get; set; }
        // public int FuchsiaCount { get; set; }
        // // public decimal FuchsiaPercent { get; set; }
        // public int YellowCount { get; set; }
        // public decimal YellowPercent { get; set; }
        public List<ColorCodeCountDto> ColorData { get; set; }

        public List<int> Prices { get; set; }

        // public List<SizeCountDto> SizeData { get; set; }
        public List<TopProductBySizeDto> TopProductBySize { get; set; }

    }


    public class ColorCodeCountDto
    {
        public string ColorName { get; set; }
        public int ColorCount { get; set; }
        public string ColorCode { get; set; }
    }

    public class TopProductBySizeDto
    {
        public List<int> Ids { get; set; }
        public List<int> Count { get; set; }
        public string Size { get; set; }
    }


}