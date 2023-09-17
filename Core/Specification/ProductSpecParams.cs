using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Specification
{
    public class ProductSpecParams
    {
        public string? Category{get;set;}
        private int _paging = 0;
        public int paging
        {
            get { return _paging; }
            set
            {
                // Check if the assigned value is greater than or equal to 1
                if (value >= 0)
                {
                    _paging = value;
                }
                else
                {
                    // Optionally, you can throw an exception or handle the invalid value in some way
                    throw new ArgumentOutOfRangeException(nameof(value), "Page index must be greater than or equal to 0.");
                }
            }
        }
        public int PageSize {get;set;}= 6;

        public string? keyword { get; set; }
    }
}