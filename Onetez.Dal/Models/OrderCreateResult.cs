using System;
using System.Collections.Generic;

namespace Onetez.Dal.Models
{
    public class OrderCreateResult
    {
        public class Rootobject
        {
            public DataOrder data { get; set; }
            public bool success { get; set; }
        }

        public class DataOrder
        {
            public string bill_full_name { get; set; }
            public string bill_phone_number { get; set; }
            public string id { get; set; }
            public bool is_free_shipping { get; set; }
            public Item[] items { get; set; }
            public string note { get; set; }
            public object note_print { get; set; }
            public Shipping_Address shipping_address { get; set; }
            public int shipping_fee { get; set; }
            public int shop_id { get; set; }
            public int total_discount { get; set; }
            public Warehouse_Info warehouse_info { get; set; }
        }

        public class Shipping_Address
        {
            public string address { get; set; }
            public object commune_id { get; set; }
            public string country_code { get; set; }
            public object district_id { get; set; }
            public string full_address { get; set; }
            public string full_name { get; set; }
            public string phone_number { get; set; }
            public object post_code { get; set; }
            public object province_id { get; set; }
        }

        public class Warehouse_Info
        {
            public string district_id { get; set; }
            public string full_address { get; set; }
            public string name { get; set; }
            public string phone_number { get; set; }
            public string province_id { get; set; }
        }

        public class Item
        {
            public int discount_each_product { get; set; }
            public bool is_bonus_product { get; set; }
            public bool is_discount_percent { get; set; }
            public bool is_wholesale { get; set; }
            public bool one_time_product { get; set; }
            public string product_id { get; set; }
            public int quantity { get; set; }
            public string variation_id { get; set; }
            public Variation_Info variation_info { get; set; }
        }

        public class Variation_Info
        {
            public object detail { get; set; }
            public string display_id { get; set; }
            public Field[] fields { get; set; }
            public string name { get; set; }
            public string product_display_id { get; set; }
            public int retail_price { get; set; }
            public int weight { get; set; }
        }

        public class Field
        {
            public string id { get; set; }
            public string name { get; set; }
            public string value { get; set; }
        }
    }
}
