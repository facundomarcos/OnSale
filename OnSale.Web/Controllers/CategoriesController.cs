using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnSale.Common.Entities;
using OnSale.Web.Data;
using OnSale.Web.Helpers;
using OnSale.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnSale.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;


        public CategoriesController(DataContext context, IBlobHelper blobHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;

        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }
        //cuando creamos una category
        //mandamos un categoryViewModel
        public IActionResult Create()
        {
            CategoryViewModel model = new CategoryViewModel();
            return View(model);
        }
        //y nos vuelve por post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            //la validacion del modelo
            if (ModelState.IsValid)
            {
                //inicializa la imagen vacia?
                Guid imageId = Guid.Empty;
                //si el usuario cargo una imagen
                if (model.ImageFile != null)
                {
                    //le mandamos la imagen al container categorias
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "categories");
                }

                try
                {
                    //y aca usamos el helper para convetir el categoryviewmodel a category
                    //y subir la imagen que puso el usuario
                    Category category = _converterHelper.ToCategory(model, imageId, true);
                    //agremamos la categoria
                    _context.Add(category);
                    //guardamos los cambios
                    await _context.SaveChangesAsync();
                    //retornamos al index
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(model);
        }

    }


}
