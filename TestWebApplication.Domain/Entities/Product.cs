using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TestWebApplication.Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        public int ProductId { get; set; }


        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название товара")]
        public string ProductName { get; set; }
        
        [ForeignKey("Brand")]
        [Display(Name = "Марка")]
        [Required(ErrorMessage = "Введите марку товара")]
        public string BrandName { get; set; }

        //[DataType(DataType.MultilineText)]
        //[Display(Name = "Описание")]
        //[Required(ErrorMessage = "Введите описание товара")]
        //public string Description { get; set; }

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Укажите категорию товара")]
        public string ProductCategory { get; set; }

        [Display(Name = "Цена (руб)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Введите положительное значение для цены")]
        public decimal Price { get; set; }
        // public string ImageMimeType { get; set; }

        public virtual ICollection<ImageLink> ImageLinks { get; set; }
        public virtual PhoneAttribute PhoneAttribute { get; set; }
        public virtual PhoneCasesAttribute PhoneCasesAttribute { get; set; }
        public virtual Brand Brand { get; set; }
    }


    public class ImageLink
    {
        [Key]
        public int ImageLinkId { get; set; }
        public string Link { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
    public class PhoneAttribute
    {
        //public int PhoneAttributeId { get; set; }
        public string Model { get; set; }
        public string Platform { get; set; }
        public string LTE { get; set; }
        public string GPS { get; set; }
        public string SIMtype { get; set; }
        public string DualSIM { get; set; }
        public string DisplaySize { get; set; }
        public string DisplayResolution { get; set; }
        public string CameraPixels { get; set; }
        public string FrontalCamera { get; set; }
        public string RAM { get; set; }
        public string InternalMemory { get; set; }
        public string BatteryCapacity { get; set; }
        public string CPUfreq { get; set; }
        public string Size { get; set; }

        [Key, ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }

    public class PhoneCasesAttribute
    {
        public string Applicability { get; set; }
        public string Type { get; set; }
        public string Material { get; set; }
        public string DisplaySize { get; set; }
        public string Color { get; set; }

        [Key, ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }

    public class Brand
    {
        [Key]
        public string Name { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
