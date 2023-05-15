using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Sistema_Organizacion.Models
{
    public partial class EmpleadoHabilidad
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Empleado")]
        public int IdEmpleado { get; set; }
        [Required(ErrorMessage = "La habilidad es requerida")]
        [Display(Name = "Habilidad")]
        public string NombreHabilidad { get; set; }

        public virtual Empleado IdEmpleadoNavigation { get; set; }
    }
}
