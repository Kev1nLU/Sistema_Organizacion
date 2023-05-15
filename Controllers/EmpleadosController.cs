using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Organizacion.Data;
using Sistema_Organizacion.Models;

namespace Sistema_Organizacion.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpleadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Empleados
     
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            //Var empleado para extraer data de la DB
            var empleado = from s in _context.Empleado
                           select s;

            //Implementando buscador de empleados 
            if (!String.IsNullOrEmpty(searchString))
            {
                empleado = _context.Empleado.Where(s => s.NombreCompleto.ToUpper().Contains(searchString.ToUpper())
                                       || s.IdArea.ToString().Contains(searchString.ToUpper()));
            }

            if (_context.Empleado == null) {
                return Problem("Entity set is null ");
            }

            //Viewbag que contiene a todos los empleados
            //Sirve para recargar el combo box de Jefe con todos los empleados
            var empleados = _context.Empleado.ToList();
            ViewBag.Empleados = empleados;

            //Viewbag que contiene la data de las Areas
            //nuevamente para recargar el combo box de las areas
            var areas = _context.Area.ToList();
            ViewBag.Areas = new SelectList(areas, "Id", "Nombre");

            //Returna la lista de todos los empleados de forma asyncrona
            return View(await empleado.ToListAsync());
        }

        // GET: Empleados/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleado
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            //fechaActual extrae la fecha del dia de hoy
            DateTime fechaActual = DateTime.Now;

            //Valido que exista una fecha de nacimiento
            if (empleado.FechaNacimiento != null)
            {
                //Obtengo Fecha de nacimiento de la DB
                var fechaNacimiento = empleado.FechaNacimiento;
                //tiempoTranscurrido almacena los annios transcurridos desde el nacimiento
                //del empleado hasta el dia de hoy
                TimeSpan tiempoTranscurrido = fechaActual.Subtract((DateTime)fechaNacimiento);
                //Se almacena en edadAnnios el total de dias transcurridos entre la fecha de nacimiento
                // hasta el dia de hoy, se divide por 365.25 para tomar en cuenta los annios bisiestos
                int edadAnnios = (int)(tiempoTranscurrido.TotalDays / 365.25);
                //Se almacena la cantidad de annio.(Esto deberia estar en otro metodo aparte de este)
                ViewBag.Edadf = edadAnnios;
            }

            //Valido que exista una fecha de nacimiento
            if (empleado.FechaIngreso != null)
            {
                //Obtengo Fecha de nacimiento
                var fechaIngre = empleado.FechaIngreso;

                //Realiza la misma función que el codigo anterior pero para los años
                //que lleva trabajando el empleado
                TimeSpan tiempoTranscurrido = fechaActual.Subtract((DateTime)fechaIngre);
                int edadAnniosi = (int)(tiempoTranscurrido.TotalDays / 365.25);
                ViewBag.Edadi = edadAnniosi;
            }

            return View(empleado);
        }

        // GET: Empleados/Create
        public IActionResult Create()
        {
            var areas = _context.Area.ToList();
            if (areas == null || areas.Count == 0)
            {
                return RedirectToAction("Error", "Index");
            }
           
            //Se pasa una lista de todos los empleados para el combo box
            //de esta manera aparece el nombre del empleado y no el id
            var empleados = _context.Empleado.ToList();
            ViewBag.Empleados = empleados;
            //Se pasa una lista de todos las areas para el combo box
            //de esta manera aparece el nombre de las area y no el id
            ViewBag.Areas = new SelectList(areas, "Id", "Nombre");
            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreCompleto,Cedula,Correo,FechaNacimiento,FechaIngreso,IdJefe,IdArea,Foto")] Empleado empleado)
        {
          
            if (ModelState.IsValid)
            {

                // se verifica que exista este empleado tiene un jefe
                if (empleado.IdJefe.HasValue)
                {
                    //el codigo _context.Empleado.Find(empleado.IdJefe) busca en la DB
                    //al empleado con el id de jefe, luego en empleado.IdJefeNavigation
                    //se establece que el empleado ahora esta asociado con un jefe
                    empleado.IdJefeNavigation = _context.Empleado.Find(empleado.IdJefe);
                }

                _context.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

           

            return View(empleado);
        }
        //GET: Odtener a los empleados que tengan un jefe en particular
        public List<Empleado> GetEmpleadoJefe(int idJefe) 
        {
            //aqui guardamos en la variable empleados y almacenamos los empleados que
            //tengan el mismo id del jefe y el id del parametro dado, posteriormnete se almacenan en 
            //la variable empleados
            var empleados = _context.Empleado.Where(e => e.IdJefe == idJefe).ToList();

            //bucle foreach para cada objeto empleado dentro de empleados
            foreach (var empleado in empleados)
            {
                //Usando la recursividad solicitada se devuelve una lista de los empleados que
                //tienen un jefe con el idJefe.
                //luego usando el inverseIdJefeNavigation que genero EF 
                // la cual representa una lista de los empleados que tienen al a un empleado como su jefe
                empleado.InverseIdJefeNavigation = GetEmpleadoJefe(empleado.Id);
            }

            //Finalmente retornamos esa lista de empleados con un jefe
            return empleados;
        }

        public async Task<IActionResult> Organizacion() 
        {
            //Aqui busca en la DB cuales empleados que tienen por lo menos un empleado a 
            //su cargo, use esto para esta parte: https://stackoverflow.com/questions/30921873/getting-all-employees-under-a-manager-c-sharp
            var jefes = _context.Empleado.Where(e => e.InverseIdJefeNavigation.Count > 0).ToList();
            //Lista vacias llamada jerarquia
            var jerarquia = new List<Empleado>();

            //Recorremos la lista de jefes
            foreach (var jefe in jefes)
            {
                //guaramos en jerarquia el objeto jefe"
                jerarquia.Add(jefe);
                //Aqui se obtienen a todos los empleados del objeto jefe en la iteración
                //el resultado se agrega a jerarquia
                jerarquia.AddRange(GetEmpleadoJefe(jefe.Id));
            }

            //Retornamos la jerarquia a la view Organizacion
            return View(jerarquia);
        }

        // GET: Empleados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            var areas = _context.Area.ToList();
            if (areas == null || areas.Count == 0)
            {
                return RedirectToAction("Error", "Index");
            }

            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleado.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

           
            var empleados = _context.Empleado.ToList();
            ViewBag.Empleados = empleados;

            ViewBag.Areas = new SelectList(areas, "Id", "Nombre");

            ViewBag.Imagen = empleado.Foto;

          

            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreCompleto,Cedula,Correo,FechaNacimiento,FechaIngreso,IdJefe,IdArea,Foto")] Empleado empleado)
        {
           
            if (id != empleado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (empleado.IdJefe.HasValue)
                {
                    empleado.IdJefeNavigation = _context.Empleado.Find(empleado.IdJefe);
                }
                try
                {
                    _context.Update(empleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        // GET: Empleados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            var areas = _context.Area.ToList();
            if (areas == null || areas.Count == 0)
            {
                return RedirectToAction("Error", "Index");
            }
            var empleados = _context.Empleado.ToList();
            ViewBag.Empleados = empleados;

            ViewBag.Areas = new SelectList(areas, "Id", "Nombre");


            var empleado = await _context.Empleado
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {



            var empleado = await _context.Empleado.FindAsync(id);

            if (empleado.IdJefe.HasValue)
            {
                empleado.IdJefeNavigation = _context.Empleado.Find(empleado.IdJefe);
            }
            _context.Empleado.Remove(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleado.Any(e => e.Id == id);
        }
    }
}
