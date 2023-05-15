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
            var empleado = from s in _context.Empleado
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                empleado = _context.Empleado.Where(s => s.NombreCompleto.ToUpper().Contains(searchString.ToUpper())
                                       || s.IdArea.ToString().Contains(searchString.ToUpper()));
            }

            if (_context.Empleado == null) {
                return Problem("Entity set is null ");
            }

            var empleados = _context.Empleado.ToList();
            ViewBag.Empleados = empleados;

            var areas = _context.Area.ToList();
            ViewBag.Areas = new SelectList(areas, "Id", "Nombre");

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

            DateTime fechaActual = DateTime.Now;

            //Valido que exista una fecha de nacimiento
            if (empleado.FechaNacimiento != null)
            {
                //Obtengo Fecha de nacimiento
                var fechaNacimiento = empleado.FechaNacimiento;

                TimeSpan tiempoTranscurrido = fechaActual.Subtract((DateTime)fechaNacimiento);
                int edadAnnios = (int)(tiempoTranscurrido.TotalDays / 365.25);
                ViewBag.Edadf = edadAnnios;
            }
            else
            {

            }

            //Valido que exista una fecha de nacimiento
            if (empleado.FechaIngreso != null)
            {
                //Obtengo Fecha de nacimiento
                var fechaIngre = empleado.FechaIngreso;

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
           
            var empleados = _context.Empleado.ToList();
            ViewBag.Empleados = empleados;

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

                if (empleado.IdJefe.HasValue)
                {
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
            var empleados = _context.Empleado.Where(e => e.IdJefe == idJefe).ToList();

            foreach (var empleado in empleados)
            {
                empleado.InverseIdJefeNavigation = GetEmpleadoJefe(empleado.Id);
            }


            return empleados;
        }

        public async Task<IActionResult> Organizacion() 
        {
            var jefes = _context.Empleado.Where(e => e.InverseIdJefeNavigation.Count > 0).ToList();
            var jerarquia = new List<Empleado>();

            foreach (var jefe in jefes)
            {
                jerarquia.Add(jefe);
                jerarquia.AddRange(GetEmpleadoJefe(jefe.Id));
            }

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
