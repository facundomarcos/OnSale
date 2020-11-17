using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace OnSale.Common.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} must contains less than {1} characters")]
        [Required]
        public string Name { get; set; }
        //que no es obligatoria
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        //guarda el formato de dinero en decimal y lo formatea para mostrarlo
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }
        //no se borran los productos porque dañan la integridad de la base de datos
        //se activan o inactivan
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        //si es un articulo destacado
        [DisplayName("Is Starred")]
        public bool IsStarred { get; set; }
        //le catalogamos la categoria
        public Category Category { get; set; }
        //y un producto puede tener muchas imagenes
        public ICollection<ProductImage> ProductImages { get; set; }
        //propiedad de solo lectura para saber cuantas imagenes tiene un producto
        [DisplayName("Product Images Number")]
        public int ProductImagesNumber => ProductImages == null ? 0 : ProductImages.Count;

        //TODO: Pending to put the correct paths
        [Display(Name = "Image")]
        //si la coleccion de imagen es null -> cargamos el noimage
        //y sino, cargamos la primer imagen de la coleccion
        public string ImageFullPath => ProductImages == null || ProductImages.Count == 0
            ? $"https://localhost:44390/images/noimage.png"
            : ProductImages.FirstOrDefault().ImageFullPath;
    }

}
