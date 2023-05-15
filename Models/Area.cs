using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Sistema_Organizacion.Models
{
    public partial class Area
    {
        public Area()
        {
            Empleado = new HashSet<Empleado>();
        }
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del área es requerido")]
        [StringLength(100)]
        [Display(Name = "Nombre del área")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(300)]
        [Display(Name = "Descripción del Área")]
        public string Descripcion { get; set; }

        public virtual ICollection<Empleado> Empleado { get; set; }
    }
}
