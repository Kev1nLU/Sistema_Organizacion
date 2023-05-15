using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Sistema_Organizacion.Models
{
    public partial class Empleado
    {
        public Empleado()
        {
            EmpleadoHabilidad = new HashSet<EmpleadoHabilidad>();
            InverseIdJefeNavigation = new HashSet<Empleado>();
        }
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nombre completo requerido")]
        [StringLength(200)]
        [Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; }
        [Required]
        [RegularExpression(@"^\d{8,9}$", ErrorMessage = "La cédula debe tener 9 dígitos como máximo y deben ser solo números")]
        [Display(Name = "Cédula")]
        public string Cedula { get; set; }
        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime? FechaNacimiento { get; set; }
        [Required(ErrorMessage = "La fecha de ingreso es requerida")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de ingreso")]
        public DateTime FechaIngreso { get; set; }
        [Display(Name = "Jefe")]
        public int? IdJefe { get; set; }
        [Required]
        [Display(Name = "Area de trabajo")]
        public int IdArea { get; set; }


        //Aqui cuando use el ORM Entity Framework me puso la propiedad Foto como tiene byte[]
        //Lo cambie a string para almacenar el nombre y la extension en la base de datos, 
        // y guardar la imagen en la carpeta wwwroot/images. para luego nada mas llamar
        // a asp-for="Foto" y traerme el string (ejm: fotoperfil.jpg) para que cuando vaya al perfil de un empleado
        // pasarle a la etiqueta <img src="~/images/@Viewbag">, asi me traia el string de la base de datos y la img del servidor
        //pero se me complico y fue la unica caracteristica que no pude completar.
        [Display(Name = "Fotografia")]
        public string Foto { get; set; }

        public virtual Area IdAreaNavigation { get; set; }
        public virtual Empleado IdJefeNavigation { get; set; }
        public virtual ICollection<EmpleadoHabilidad> EmpleadoHabilidad { get; set; }
        public virtual ICollection<Empleado> InverseIdJefeNavigation { get; set; }
    }
}
