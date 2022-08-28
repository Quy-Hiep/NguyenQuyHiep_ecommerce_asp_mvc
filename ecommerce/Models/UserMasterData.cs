using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ecommerce.Models
{
    public class UserMasterData
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nhập họ")]
        [Display(Name = "Họ")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Nhập tên")]
        [Display(Name = "Tên")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Nhập email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Nhập mật khẩu")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Chọn quyền")]
        [Display(Name = "Quyền quản trị")]
        public Nullable<bool> IsAdmin { get; set; }
    }
}