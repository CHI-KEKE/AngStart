    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    namespace Core.Entities
    {
        public class Product:BaseEntity
        {
            public string Category { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int? Price { get; set; }

            public string Texture { get; set; }

            public string Wash { get; set; }

            public string Place { get; set; }

            public string Note { get; set; }

            public string Story { get; set; }

            public string? MainImage { get; set; }

            public List<Image> Images { get; set; } = new List<Image>();

            public List<Color> Colors { get; set; } = new List<Color>();
            public List<Size> Sizes { get; set; } = new List<Size>();
            public List<Variant> Variants { get; set; }  = new List<Variant>();
        }
    }