using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnSale.Common.Entities
{
    public class City
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} must contains less than {1} characters")]
        [Required]
        public string Name { get; set; }
        //estas propiedades son para manipular la data
        //no modifican la base de datos
        //por lo tanto no hay que correr ninguna migracion

        //
        [JsonIgnore]
        //para que no se guarde en la base de datos
        [NotMapped]
        public int IdDepartment { get; set; }

        //
        [JsonIgnore]
        //para que no se guarde en la base de datos
        [NotMapped]
        public int IdCountry { get; set; }
    }
    //es valido tambien crear la realacion de esta manera
    // public Department Department { get; set; }
    //pero si se deja la relacion en ambos lados va a haber un problema cuando agente los servicios
    //porque se crea una referencia circular

}
