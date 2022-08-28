using ecommerce.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ecommerce.Context
{
    [MetadataType(typeof(ProductMasterData))]
    public partial class Product
    {
        [NotMapped]
        public System.Web.HttpPostedFileBase ImageUpload { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }

        public static implicit operator Product(User v)
        {
            throw new NotImplementedException();
        }
    }

    [MetadataType(typeof(CategoryMasterData))]
    public partial class Category
    {
        [NotMapped]
        public System.Web.HttpPostedFileBase ImageUpload { get; set; }
    }

    [MetadataType(typeof(UserMasterData))]
    public partial class User
    {
    }

    [MetadataType(typeof(BrandMasterData))]
    public partial class Brand
    {
        [NotMapped]
        public System.Web.HttpPostedFileBase ImageUpload { get; set; }
    }

    [MetadataType(typeof(OrderMasterData))]
    public partial class Order
    {
        public User UserInfo { get; set; }
    }
  

}