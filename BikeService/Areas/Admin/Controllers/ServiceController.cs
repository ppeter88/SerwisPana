using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeService.Data;
using BikeService.Models;
using BikeService.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeService.Areas.Admin.Controllers
{
    [Authorize(Roles = UserRoles.AdminUser)]
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ServiceController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        /*GET - Index*/
        public async Task<IActionResult> Index()
        {

            return View(await _db.Service.ToListAsync());
        }

        /*GET - Create*/
        public IActionResult Create()
        {
            return View();
        }

        /*POST - Create*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            if(ModelState.IsValid)
            {
                _db.Service.Add(service);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        /*GET - Edit*/
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var service = await _db.Service.FindAsync(id);
            if(service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        /*POST - Edit*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Service service)
        {
            if(ModelState.IsValid)
            {
                _db.Update(service);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        /*DELETE*/
        public async Task<IActionResult> Delete(int? id)
        {
            var service = await _db.Service.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            _db.Service.Remove(service);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        /*GET - View*/
        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var service = await _db.Service.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

    }
}