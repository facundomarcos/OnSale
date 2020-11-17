using Microsoft.AspNetCore.Http;
using OnSale.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnSale.Web.Models
{
    //ViewModel tiene otro significado en xamarin
    //esta clase es para agregar las imagenes de las categorias
    //hereda de Category
    public class CategoryViewModel : Category
    {
        [Display(Name = "Image")]
        //IFormFile nos permite capturar la imagen en memoria
        public IFormFile ImageFile { get; set; }
    }

}
