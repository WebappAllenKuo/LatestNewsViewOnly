using System.ComponentModel.DataAnnotations;
using Infra.Models.Vue;
using Infra.Parameters;

namespace Infra.Models.Account
{
    public class LoginFormDto
    {
        [Display(Name                   = "帳號", Prompt = "輸入Email")]
        [Required(ErrorMessage          = "請填寫{0}")]
        [StringLength(500, ErrorMessage = "{0} 長度要介於 {2} 及 {1} 之間")]
        [TableColumn(InputType          = VueInputType.Text)]
        public string Email { get; set; }

        [Display(Name                   = "密碼", Prompt = "輸入密碼")]
        [Required(ErrorMessage          = "請填寫{0}")]
        [StringLength(500, ErrorMessage = "{0} 長度要介於 {2} 及 {1} 之間")]
        [TableColumn(InputType          = VueInputType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
