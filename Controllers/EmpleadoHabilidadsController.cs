using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Organizacion.Data;
using Sistema_Organizacion.Models;


namespace Sistema_Organizacion.Controllers
{
    public class EmpleadoHabilidadsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpleadoHabilidadsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EmpleadoHabilidads
        public async Task<IActionResult> Index()
        {

            var empleados = _context.Empleado.ToList();
            ViewBag.Empleados = new SelectList(empleados, "Id", "NombreCompleto");

            return View(await _context.EmpleadoHabilidad.ToListAsync());
        }

        // GET: EmpleadoHabilidads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleadoHabilidad = await _context.EmpleadoHabilidad
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleadoHabilidad == null)
            {
                return NotFound();
            }

            return View(empleadoHabilidad);
        }

        // GET: EmpleadoHabilidads/Create
        public IActionResult Create()
        {

            var empleados = _context.Empleado.ToList();
            ViewBag.Empleados = new SelectList(empleados, "Id", "NombreCompleto");
            return View();
        }

        // POST: EmpleadoHabilidads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdEmpleado,NombreHabilidad")] EmpleadoHabilidad empleadoHabilidad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empleadoHabilidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empleadoHabilidad);
        }

        // GET: EmpleadoHabilidads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleadoHabilidad = await _context.EmpleadoHabilidad.FindAsync(id);
            if (empleadoHabilidad == null)
            {
                return NotFound();
            }


            var empleados = _context.Empleado.ToList();
            ViewBag.Empleados = empleados;

            return View(empleadoHabilidad);
        }

        // POST: EmpleadoHabilidads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEmpleado,NombreHabilidad")] EmpleadoHabilidad empleadoHabilidad)
        {
            if (id != empleadoHabilidad.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleadoHabilidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoHabilidadExists(empleadoHabilidad.Id))
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
            return View(empleadoHabilidad);
        }

        // GET: EmpleadoHabilidads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleadoHabilidad = await _context.EmpleadoHabilidad
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleadoHabilidad == null)
            {
                return NotFound();
            }

            return View(empleadoHabilidad);
        }

        // POST: EmpleadoHabilidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleadoHabilidad = await _context.EmpleadoHabilidad.FindAsync(id);
            _context.EmpleadoHabilidad.Remove(empleadoHabilidad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoHabilidadExists(int id)
        {
            return _context.EmpleadoHabilidad.Any(e => e.Id == id);
        }
    }
}
