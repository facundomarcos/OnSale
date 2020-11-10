using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnSale.Common.Entities
{
    public class Department
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} must contains less than {1} characters")]
        [Required]
        public string Name { get; set; }
        //un departamento tiene ciudades
        public ICollection<City> Cities { get; set; }

        [DisplayName("Cities Number")]
        //propiedad de lectura
        //si cities es null entonces devuelve 0
        //sino... devuelve las que cuenta
        public int CitiesNumber => Cities == null ? 0 : Cities.Count;
        //para que no se guarde en la base de datos
        [NotMapped]
        public int IdCountry { get; set; }

    }

}
