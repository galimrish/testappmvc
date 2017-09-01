using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestWebApplication.WebUI.Infrastructure.Attributes;
//using System.Web.Mvc;

namespace TestWebApplication.WebUI.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "Введите ваш email")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Запомнить браузер?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        
        [Required(ErrorMessage = "Введите ваш email")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите ваш email")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        [Display(Name = "Email")]        
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        //[DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        [Display(Name = "Email")]
        [System.Web.Mvc.Remote("EmailAvailableAsync", "Account")]
        public string Email { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "Минимальная длина пароля - {2} символов", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        [ValidatePassword]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        //[System.Web.Mvc.Remote("EmailAvailableAsync", "AccountController")]
        //public string Email { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "Минимальная длина пароля - {2} символов", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        [ValidatePassword]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        
        [Display(Name = "Подтвердить пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        public string UserId { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
