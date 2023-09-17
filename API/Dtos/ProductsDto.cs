using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace API.Dtos
{
    public class Color
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ProductDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("price")]
        public int? Price { get; set; }

        [JsonProperty("texture")]
        public string? Texture { get; set; }

        [JsonProperty("wash")]
        public string? Wash { get; set; }

        [JsonProperty("place")]
        public string? Place { get; set; }

        [JsonProperty("note")]
        public string? Note { get; set; }

        [JsonProperty("story")]
        public string? Story { get; set; }

        [JsonProperty("main_image")]
        public string? MainImage { get; set; }

        [JsonProperty("images")]
        public List<string>? Images { get; set; }

        [JsonProperty("variants")]
        public List<Variant>? Variants { get; set; }

        [JsonProperty("colors")]
        public List<Color>? Colors { get; set; }

        [JsonProperty("sizes")]
        public List<string>? Sizes { get; set; }
    }

    public class ProductsDto
    {
        [JsonProperty("data")]
        public List<ProductDto> Data { get; set; }

        [JsonProperty("next_paging")]
        public int? NextPaging { get; set; }
    }

    public class Variant
    {
        [JsonProperty("color_code")]
        public string ColorCode { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("stock")]
        public int? Stock { get; set; }
    }
}