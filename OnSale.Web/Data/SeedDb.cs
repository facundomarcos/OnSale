﻿using Microsoft.EntityFrameworkCore;
using OnSale.Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace OnSale.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        //constructor para acceder a la base de datos mediante inyeccion de dependencias
        public SeedDb(DataContext context)
        {
            _context = context;
        }
        //metodo para poblar la base de datos
        public async Task SeedAsync()
        {
            //instruccion para correr la base de datos, si no existe, la crea
            await _context.Database.EnsureCreatedAsync();
            //este metodo nos va a garantizar que existan paises
            await CheckCountriesAsync();
        }

        private async Task CheckCountriesAsync()
        {
            //hay algun pais? si no hay nada ... agrego paises
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    Departments = new List<Department>
                {
                    new Department
                    {
                        Name = "Antioquia",
                        Cities = new List<City>
                        {
                            new City { Name = "Medellín" },
                            new City { Name = "Envigado" },
                            new City { Name = "Itagüí" }
                        }
                    },
                    new Department
                    {
                        Name = "Bogotá",
                        Cities = new List<City>
                        {
                            new City { Name = "Bogotá" }
                        }
                    },
                    new Department
                    {
                        Name = "Valle del Cauca",
                        Cities = new List<City>
                        {
                            new City { Name = "Calí" },
                            new City { Name = "Buenaventura" },
                            new City { Name = "Palmira" }
                        }
                    }
                }
                });
                _context.Countries.Add(new Country
                {
                    Name = "USA",
                    Departments = new List<Department>
                {
                    new Department
                    {
                        Name = "California",
                        Cities = new List<City>
                        {
                            new City { Name = "Los Angeles" },
                            new City { Name = "San Diego" },
                            new City { Name = "San Francisco" }
                        }
                    },
                    new Department
                    {
                        Name = "Illinois",
                        Cities = new List<City>
                        {
                            new City { Name = "Chicago" },
                            new City { Name = "Springfield" }
                        }
                    }
                }
                });
                //guardar en la base de datos
                await _context.SaveChangesAsync();
            }
        }
    }

}
