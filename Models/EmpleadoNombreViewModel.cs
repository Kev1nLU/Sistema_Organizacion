using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema_Organizacion.Models
{
    public class EmpleadoNombreViewModel
    {
        public List<Empleado>? Empleados { get; set; }
        public SelectList? Nombres { get; set; }
        public string? NombreCompleto { get; set; }
        public string? SearchString { get; set; }
    }
}
