using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Common.Entities
{
    public class Country
    {
        public int Id { get; set; }
        //"el campo tal debe contener menos de 50 caracteres"
        [MaxLength(50, ErrorMessage = "The field {0} must contains less than {1} characters")]
        [Required]
        public string Name { get; set; }
        //un country tiene una relacion de departamentos
        public ICollection<Department> Departments { get; set; }
        //y la propiedad de lectura... llamada Departments number para contar los departamentos
        [DisplayName("Departments Number")]
        public int DepartmentsNumber => Departments == null ? 0 : Departments.Count;

    }

}
