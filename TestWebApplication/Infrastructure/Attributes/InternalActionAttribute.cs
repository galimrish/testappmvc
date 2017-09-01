using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebApplication.WebUI.Infrastructure.Attributes
{
    public class InternalActionAttribute : ActionMethodSelectorAttribute 
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
        {
            if (controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                return true;
            if (controllerContext.IsChildAction)
                return true;
            return false;
        }
    }

    public class ValidatePasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string pass = value.ToString();
            if (pass.Length < 6)
                return new ValidationResult("Минимальная длина пароля - 6 символов");
            if (!System.Text.RegularExpressions.Regex.IsMatch(pass,  @"^[a-zA-Z0-9]+$"))
                return new ValidationResult("Пароль может содержать только буквы латинского алфавита и цифры");
            return ValidationResult.Success;
        }
    }
}