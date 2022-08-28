using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ecommerce.Models
{
    public partial class ProductMasterData
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Nhập tên sản phẩm")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }


        [Display(Name = "Hình ảnh")]
        public string Avartar { get; set; }


        [Required(ErrorMessage = "Chọn danh mục")]
        [Display(Name = "Danh mục")]
        public Nullable<int> CategoryId { get; set; }


        [Required(ErrorMessage = "Nhập mô tả")]
        [Display(Name = "Mô tả ngắn")]
        public string ShortDes { get; set; }


        [Required(ErrorMessage = "Nhập mô tả")]
        [Display(Name = "Mô tả đầy đủ")]
        public string FullDescription { get; set; }


        [Required(ErrorMessage = "Nhập giá sản phẩm")]
        [Display(Name = "Giá gốc")]
        public Nullable<double> Price { get; set; }


        [Required(ErrorMessage = "Nhập giá khuyến mãi")]
        [Display(Name = "Giá khuyến mãi")]
        public Nullable<double> PriceDiscount { get; set; }


        [Required(ErrorMessage = "Chọn loại sản phẩm")]
        [Display(Name = "Loại sản phẩm")]
        public Nullable<int> TypeId { get; set; }
        public string Slug { get; set; }


        [Required(ErrorMessage = "Chọn thương hiệu sản phẩm")]
        [Display(Name = "Thương hiệu")]
        public Nullable<int> BrandId { get; set; }


        [Display(Name = "Trạng thái xóa")]
        public Nullable<bool> Deleted { get; set; }


        public Nullable<bool> ShowOnHomePage { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public Nullable<System.DateTime> CreatedOnUtc { get; set; }
        public Nullable<System.DateTime> UpdatedOnUtc { get; set; }
        public Nullable<int> CreateUser { get; set; }
        public Nullable<int> UpdateUser { get; set; }
    }
}