using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Services;


namespace KooliProjekt.Controllers
{
    public class OrganizersController : Controller
    {
        private readonly IOrganizerService _organizer;

        public OrganizersController(IOrganizerService organizer)
        {
            _organizer = organizer;
        }

        // GET: Organizers
        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await _organizer.List(page, 5));
        }

        // GET: Organizers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizer = await _organizer.Get(id);
            if (organizer == null)
            {
                return NotFound();
            }

            return View(organizer);
        }

        // GET: Organizers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organizers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Organizer organizer)
        {
            if (ModelState.IsValid)
            {
                await _organizer.Save(organizer);
                return RedirectToAction(nameof(Index));
            }
            return View(organizer);
        }

        // GET: Organizers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizer = await _organizer.Get(id);
            if (organizer == null)
            {
                return NotFound();
            }
            return View(organizer);
        }

        // POST: Organizers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Organizer organizer)
        {
            if (id != organizer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _organizer.Save(organizer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var existingClient = await _organizer.Get(id);
                    if (existingClient == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // This will now actually throw the exception
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(organizer);
        }

        // GET: Organizers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizer = await _organizer.Get(id);
            if (organizer == null)
            {
                return NotFound();
            }

            return View(organizer);
        }

        // POST: Organizers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var organizer = await _organizer.Get(id);
            if (organizer != null)
            {
                await _organizer.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<bool> ClientExists(int id)
        {
            return await _organizer.Get(id) != null;
        }
    }
}
