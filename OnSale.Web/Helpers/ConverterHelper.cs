using OnSale.Common.Entities;
using OnSale.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnSale.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        //esta es la implementacion de IConveterHelper
        //que usamos para convertir categoria y viewmodelcategoria
        public Category ToCategory(CategoryViewModel model, Guid imageId, bool isNew)
        {
            return new Category
            {
                //si la categoy es nueva mandamos 0 para que la agregue en la base de datos
                //si es para actualizar le mandamos el id
                Id = isNew ? 0 : model.Id,
                ImageId = imageId,
                Name = model.Name
            };
        }

        public CategoryViewModel ToCategoryViewModel(Category category)
        {
            return new CategoryViewModel
            {
                Id = category.Id,
                ImageId = category.ImageId,
                Name = category.Name
            };
        }
    }

}
