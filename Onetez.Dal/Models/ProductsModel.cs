using System;
using System.Collections.Generic;

namespace Onetez.Dal.Models
{
    public class ProductsModel
    {
        public class Rootobject
        {
            public List<DataProduct> data { get; set; }
            public int page_number { get; set; }
            public int page_size { get; set; }
            public bool success { get; set; }
            public int total_entries { get; set; }
            public int total_pages { get; set; }
        }

        public class DataProduct
        {
            public string display_id { get; set; }
            public Field[] fields { get; set; }
            public string id { get; set; }
            public Product product { get; set; }
            public string product_id { get; set; }
            public int retail_price { get; set; }
        }

        public class Product
        {
            public Category[] categories { get; set; }
            public string display_id { get; set; }
            public string name { get; set; }
        }

        public class Category
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Field
        {
            public string id { get; set; }
            public string name { get; set; }
            public string value { get; set; }
        }
    }
}
