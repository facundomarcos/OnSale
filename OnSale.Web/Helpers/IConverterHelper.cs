using OnSale.Common.Entities;
using OnSale.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnSale.Web.Helpers
{
    //esta interface se usa para convertir de categoryviewmodel a category
    public interface IConverterHelper
    {
        //bool isNew se usa para crear o actualizar
        Category ToCategory(CategoryViewModel model, Guid imageId, bool isNew);

        CategoryViewModel ToCategoryViewModel(Category category);
    }

}
