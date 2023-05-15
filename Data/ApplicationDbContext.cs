using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Sistema_Organizacion.Models;

namespace Sistema_Organizacion.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Area> Area { get; set; }
        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<EmpleadoHabilidad> EmpleadoHabilidad { get; set; }
 
    }
}
