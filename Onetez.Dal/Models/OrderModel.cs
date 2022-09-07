using System;
using System.Collections.Generic;

namespace Onetez.Dal.Models
{
  /// <summary>
  /// Dùng cho việc tạo đơn
  /// </summary>
  public class OrderModel
  {
    public string id { get; set; }

    public string bill_full_name { get; set; }

    public string bill_phone_number { get; set; }

    public bool is_free_shipping { get; set; }

    public bool received_at_shop { get; set; }

    public List<OrderItem> items { get; set; }

    public string note { get; set; } // Ghi chú nội bộ

    public string note_print { get; set; }

    public PartnerShip partner { get; set; } // Thông tin vận chuyển

    public string warehouse_id { get; set; }

    public string shipping_address { get; set; }
    //public ShippingAddress shipping_address { get; set; }

    public int shipping_fee { get; set; }

    public int shop_id { get; set; }

    public int? total_discount { get; set; }

    public WarehouseInfo warehouse_info { get; set; }

    public string custom_id { get; set; }

    public int status { get; set; }
  }


  /// <summary>
  /// Dùng cho việc lấy đơn
  /// </summary>
  public class OrderDataModel
  {
    public string id { get; set; }

    public string bill_full_name { get; set; }

    public string bill_phone_number { get; set; }

    public bool is_free_shipping { get; set; }

    public bool received_at_shop { get; set; }

    public List<OrderItem> items { get; set; }

    public string note { get; set; } // Ghi chú nội bộ

    public string note_print { get; set; }

    public PartnerShip partner { get; set; } // Thông tin vận chuyển

    public string warehouse_id { get; set; }

    public ShippingAddress shipping_address { get; set; }

    public int shipping_fee { get; set; }

    public int shop_id { get; set; }

    public int? total_discount { get; set; }

    public WarehouseInfo warehouse_info { get; set; }

    public string custom_id { get; set; }

    public int status { get; set; }
  }


  public class ShippingAddress
  {
    public string address { get; set; }
    public string commune_id { get; set; }
    public object country_code { get; set; }
    public string district_id { get; set; }
    public string full_address { get; set; }
    public string full_name { get; set; }
    public string phone_number { get; set; }
    public object post_code { get; set; }
    public string province_id { get; set; }
  }

  public class WarehouseInfo
  {
    public string district_id { get; set; }
    public string full_address { get; set; }
    public string name { get; set; }
    public string phone_number { get; set; }
    public string province_id { get; set; }
  }

  public class OrderItem
  {
    public int discount_each_product { get; set; }
    public bool is_bonus_product { get; set; }
    public bool is_discount_percent { get; set; }
    public bool is_wholesale { get; set; }
    public bool one_time_product { get; set; }
    public int quantity { get; set; }
    public string product_id { get; set; }
    public string variation_id { get; set; }
    public VariationInfo variation_info { get; set; }
  }

  public class VariationInfo
  {
    public string detail { get; set; }
    public VariationField[] fields { get; set; }
    public string display_id { get; set; }
    public string name { get; set; }
    public string product_display_id { get; set; }
    public int retail_price { get; set; }
    public int weight { get; set; }
  }

  public class VariationField
  {
    public string id { get; set; }
    public string name { get; set; }
    public string value { get; set; }
  }

  public class PartnerShip
  {
    public int cod { get; set; }
    public string extend_code { get; set; } // Mã vận đơn
    public string order_number_vtp { get; set; } // Mã vận đơn của ViettelPost
    public List<Extend_Update> extend_update { get; set; } // Lịch sử vận chuyển
    public int partner_id { get; set; } // Id đơn vị vận chuyển trên hệ thống Pancake | 15: JT, 3: VT
                                        //public string partner_status { get; set; }
    public DateTime updated_at { get; set; } // Theo chuẩn ISO, phải chuyển về múi giờ VN
  }

  public class Extend_Update // Lịch sử xử lý của vận chuyển
  {
    public string location { get; set; }
    public string note { get; set; }
    public string status { get; set; }
    public string status_code { get; set; }
    public DateTime? updated_at { get; set; }
    public DateTime? update_at { get; set; }
  }

  public class ShopLogs // Lịch sử xử lý của shop
  {
    public string note { get; set; }
    public string user { get; set; }
    public DateTime date { get; set; }
  }

  public class OrderObject
  {
    public List<OrderDataModel> data { get; set; }
  }
}
