using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnSale.Common.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} must contains less than {1} characters")]
        [Required]
        public string Name { get; set; }

        //las imagenes van a ser unicas
        //Guid es una coleccion alfanumerica que no se repite
        //"Image" es como lo va a ver el usuario
        [Display(Name = "Image")]
        //ImageId es como realmente se guarda
        public Guid ImageId { get; set; }

        //TODO: Pending to put the correct paths
        [Display(Name = "Image")]
        //ImageFullPath es la dirección donde se van a almacenar fisicamente
        //en un blob storage de azure
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:44390/images/noimage.png"
            : $"https://onsalefacundo.blob.core.windows.net/categories/{ImageId}";
    }

}
