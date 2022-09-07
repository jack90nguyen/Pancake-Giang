using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Onetez.Dal.Models
{
  public class ShopModel
  {
    public int id { get; set; }

    //public int shop_id { get; set; }

    public string name { get; set; }

    public string api_key { get; set; }

    public string warehouse_id { get; set; }

    public string spreadsheet_id { get; set; }

    public string spreadsheet_tab { get; set; }

    public bool product_other_in_note { get; set; }

    public bool product_error_to_note { get; set; }

    public bool product_find_by_name { get; set; }

    public string product_to_order_empty { get; set; }

    public WarehouseInfo warehouse_info { get; set; }

    public SheetColumns sheet_columns { get; set; }


    public class SheetColumns
    {
      public int date { get; set; }

      public int name { get; set; }

      public int phone { get; set; }

      public int address { get; set; }

      public int product { get; set; }
      // Sản phẩm phụ
      public int other { get; set; }

      public int size { get; set; }

      public int color { get; set; }

      public int link { get; set; }

      public int note { get; set; }

      public int save { get; set; }
    }
  }
}
