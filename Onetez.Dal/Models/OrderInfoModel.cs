using System;
using System.Collections.Generic;

namespace Onetez.Dal.Models
{
    public class OrderInfoModel
    {
        public long Id { get; set; }
        public int ShopId { get; set; }
        public string OrderId { get; set; }
        public string BillName { get; set; }
        public string BillPhone { get; set; }
        public string Product { get; set; }
        public string ShipCode { get; set; }
        public List<Extend_Update> ShipLogs { get; set; }
        public string ShipUpdate { get; set; }
        public string ShipPhone { get; set; }
        public string ShipInStock { get; set; }
        public string ShipStatus { get; set; }
        public List<ShopLogs> ShopLogs { get; set; }
        public string ShopUpdate { get; set; }
        public string Complain { get; set; }
        public StaticModel Status { get; set; }
        public string UserHandling { get; set; }
        public string LastUpdate { get; set; }
    }
}
