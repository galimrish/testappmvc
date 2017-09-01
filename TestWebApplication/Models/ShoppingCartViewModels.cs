using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebApplication.Domain.Entities;

namespace TestWebApplication.WebUI.Models
{
    public class ShoppingCartViewModel
    {
        public string CartId { get; set; }
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
        public int Count { get; set; }
        public List<OrderInfoDto> OrderInfoDto { get; set; }
        public UserInfoDto UserInfoDto { get; set; }
    }

    public class UserInfoDto
    {
        public string Username { get; set; }

        [Required(ErrorMessage = "Введите ваше имя")]
        [RegularExpression("((^[a-zA-Z ]+$)|(^[а-яА-Я ]+$))",
            ErrorMessage = "Некорректное имя")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "Некорректное имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите номер вашего телефона")]
        [StringLength(11, MinimumLength = 6,
            ErrorMessage = "Некорректный номер")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Введите ваш email")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        //[Remote("EmaiAvailableAsync", "ShoppingCart")]
        public string Email { get; set; }
        public string Address { get; set; }
    }

    public class OrderInfoDto
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public decimal PricePerItem { get; set; }
        public int Quantity { get; set; }
    }
}