using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ecommerce.Models
{
    public class OrderMasterData
    {
        public int Id { get; set; }
        [Display(Name = "Tên đơn hàng")]
        public string Name { get; set; }
        [Display(Name = "Người tạo đơn")]
        public Nullable<int> UserId { get; set; }
        [Display(Name = "Trạng thái")]
        public Nullable<int> Status { get; set; }
        [Display(Name = "Ngày tạo đơn")]
        public Nullable<System.DateTime> CreatedOnUtc { get; set; }
    }
}