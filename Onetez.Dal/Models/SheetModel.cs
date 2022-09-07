using System;

namespace Onetez.Dal.Models
{
  public class SheetModel
  {
    public string Id { get; set; }

    public DateTime Date { get; set; }

    public string DateStr { get; set; }

    public string Name { get; set; }

    public string Phone { get; set; }

    public string Address { get; set; }

    public string Product { get; set; }

    public string Others { get; set; }

    public string Link { get; set; }

    public string Note { get; set; }

    public string Size { get; set; }
    
    public string Color { get; set; }

    public string Process { get; set; }

    public int ShopId { get; set; }


    public class AddressInfo
    {
      /// <summary>State</summary>
      public string state { get; set; }

      /// <summary>City/ District/ Town/ Village</summary>
      public string city { get; set; }

      /// <summary>Locality/ Landmark</summary>
      public string locality { get; set; }

      /// <summary>Address</summary>
      public string address { get; set; }

      /// <summary>Pin Code</summary>
      public string pin { get; set; }
    }
  }
}