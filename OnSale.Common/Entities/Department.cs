using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    }

}
