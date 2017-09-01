using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApplication.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Введите ваше имя")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите адрес доставки")]
        [Display(Name = "Адрес доставки")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Введите ваш номер телефона")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Электронная почта")]
        public string EmailAddress { get; set; }
    }
}
