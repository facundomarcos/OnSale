using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnSale.Common.Entities;
using OnSale.Web.Data;

namespace OnSale.Web.Controllers
{
    public class CountriesController : Controller
    {
        private readonly DataContext _context;

        public CountriesController(DataContext context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index()
        {
            //devuelve una vista con una inyeccion de dependencias de context, el cual trae
            //el listado de los countries con el metodo tolistasync
            return View(await _context.Countries
                //incluya el pais... con los departamentos (es como un inner join)
                .Include(c => c.Departments)
                .ToListAsync());
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                //incluya el pais... con los departamentos (es como un inner join)
                .Include(c => c.Departments)
                //despues de que incluya los departamentos, que incluya las ciudades
                .ThenInclude(d => d.Cities)
                //
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            //valida si que los datos del pais ingresado sean validos
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                //si hay una excepcion en la actualizacion por pais duplicado
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
                }//o si revienta algo
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(country);
        }


        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //verificamos si el pais existe
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            //y si existe volvemos a la vista para completar el formulario
            return View(country);
        }
        //cuando vuelve de la vista con los datos del pais

        // POST: Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }
            //si el modelo es valido lo guarda en la base de datos
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                    //y redirecciona a la vista index
                    return RedirectToAction(nameof(Index));
                }
                //por si al editar tambien duplicamos el pais
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
                }//o si revienta por otro motivo
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(country);
        }




        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                //incluya los departments
                .Include(c => c.Departments)
                //y las cities
                .ThenInclude(d => d.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        //metodo para agregar departamentos
        //el id puede llegar null entonces se valida
        public async Task<IActionResult> AddDepartment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Country country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            //cuando agrego el departamento, se hace uso de la propiedad NotMapped
            //porque ya conozco el id del country gracias a propiedad IdCountry (del modelo)
            //
            Department model = new Department { IdCountry = country.Id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDepartment(Department department)
        {
            //se valida que los datos del modelo sean validos
            if (ModelState.IsValid)
            {
                //busca en la base de datos
                Country country = await _context.Countries
                    //incluya el departamento en el pais
                    .Include(c => c.Departments)
                    .FirstOrDefaultAsync(c => c.Id == department.IdCountry);
                //que el country sea null no se va a dar nunca pero es conveniente validarlo
                if (country == null)
                {
                    return NotFound();
                }

                try
                {
                    //se pone el id 0 porque sino, lo puede tomar como una actualizacion
                    department.Id = 0;
                    //trae el pais y le agrega un departamento
                    country.Departments.Add(department);
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                    //devolvemos a la vista Details, el cual pide un parametro...
                    //el id del country
                    return RedirectToAction($"{nameof(Details)}/{country.Id}");

                }
                catch (DbUpdateException dbUpdateException)
                {
                    //validamos que no sea un department duplicado
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        //que el texto no este vacio
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                //o si pasa otra excepcion no prevista
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(department);
        }



        //edit department
        //donde id cambia de contexto, ya no es el country sino el department
        public async Task<IActionResult> EditDepartment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Department department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            //tambien busco el pais para poder devolverlo
            //busco el pais... cuyo departamento es igual al id del departamento que trajimos con el id
            
            Country country = await _context.Countries.FirstOrDefaultAsync(c => c.Departments.FirstOrDefault(d => d.Id == department.Id) != null);
            //y le asigna el id del departamento
            department.IdCountry = country.Id;
           //y lo manda a la vista department
            return View(department);
        }
        //mande el department en el get
        //y vuelve en el post para poder guardar los cambios

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartment(Department department)
        {
            //si cumple con todas las data anotation del modelo
            if (ModelState.IsValid)
            {
                try
                {
                    //actualizo
                    _context.Update(department);
                    //y guardo cambios
                    await _context.SaveChangesAsync();
                    //y retorna a la vista  de los detalles del country
                    return RedirectToAction($"{nameof(Details)}/{department.IdCountry}");

                }
                //y el manejo de las excepciones
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
            return View(department);
        }

        //delete department
        public async Task<IActionResult> DeleteDepartment(int? id)
        {
            //valida que le pasaron como parametro el id
            if (id == null)
            {
                return NotFound();
            }

            Department department = await _context.Departments
                //va a incluir en la busqueda las ciudades asociadas
                //para hacer un borrado en cascada
                .Include(d => d.Cities)
                //al hacer el include solo podemos buscar con el FirstOrDefaultAsync
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            //antes de borrar el departamento busco el pais al cual pertenece
            //donde el la coleccion de countries este el department id
            Country country = await _context.Countries.FirstOrDefaultAsync(c => c.Departments.FirstOrDefault(d => d.Id == department.Id) != null);
            //hace el borrado
            _context.Departments.Remove(department);
            
            await _context.SaveChangesAsync();
            //y retorna a la vista details del country
            return RedirectToAction($"{nameof(Details)}/{country.Id}");
        }

        public async Task<IActionResult> DetailsDepartment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Department department = await _context.Departments
                .Include(d => d.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            Country country = await _context.Countries.FirstOrDefaultAsync(c => c.Departments.FirstOrDefault(d => d.Id == department.Id) != null);
            department.IdCountry = country.Id;
            return View(department);
        }

        //metodo para agregar ciudades a los departamentos
        //el id que recibe como parametro corresponde al departamento que pertenece a la ciudad
        public async Task<IActionResult> AddCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Department department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            //creamos el modelo con el id precargado
            City model = new City { IdDepartment = department.Id };
            //y se lo mandamos a la vista para que complete el resto
            return View(model);
        }

        [HttpPost]
        //
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCity(City city)
        {
            if (ModelState.IsValid)
            {
                //modificamos el departamento agregando la ciudad
                //buscamos el departamento, le agregamos las ciudades que tiene
                Department department = await _context.Departments
                    .Include(d => d.Cities)
                    .FirstOrDefaultAsync(c => c.Id == city.IdDepartment);
                if (department == null)
                {
                    return NotFound();
                }

                try
                {
                    //y agregamos la ciudad
                    //el id = 0 es IMPORTANTE, xq sino, lo puede tomar como una actualizacion
                    city.Id = 0;
                    //le agregamos la nueva ciudad
                    department.Cities.Add(city);
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                    //y lo devolvemos a la vista departamentos con el id del departamento
                    return RedirectToAction($"{nameof(DetailsDepartment)}/{department.Id}");

                }
                //y el manejo de las excepciones
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

            return View(city);
        }
        //editar ciudad
        public async Task<IActionResult> EditCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            City city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            Department department = await _context.Departments.FirstOrDefaultAsync(d => d.Cities.FirstOrDefault(c => c.Id == city.Id) != null);
            city.IdDepartment = department.Id;
            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCity(City city)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                    return RedirectToAction($"{nameof(DetailsDepartment)}/{city.IdDepartment}");

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
            return View(city);
        }

        //y el metodo para borrar ciudades
        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            City city = await _context.Cities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            Department department = await _context.Departments.FirstOrDefaultAsync(d => d.Cities.FirstOrDefault(c => c.Id == city.Id) != null);
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction($"{nameof(DetailsDepartment)}/{department.Id}");
        }

    }
}
