using System;
using System.ComponentModel.DataAnnotations;

namespace Infra.Models
{
    public class LoginInfoDto
    {
        [Display(Name = "Guid")]
        public Guid Guid { get; set; }

        [Display(Name = "信箱")]
        public string Email { get; set; }

        [Display(Name = "名稱")]
        public string Name { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        [Display(Name = "角色名")]
        public string RoleName { get; set; }

        [Display(Name = "角色")]
        public string Role { get; set; }
    }
}