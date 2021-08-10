using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeService.Data;
using BikeService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeService.Areas.Serviceman.Controllers
{
    [Area("Serviceman")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CustomerController(ApplicationDbContext db)
        {
            _db = db;
        }

        /*GET - Index*/
        public async Task<IActionResult> Index()
        {
            return View(await _db.Customer.ToListAsync());
        }

        /*GET - Create*/
        public IActionResult Create()
        {
            return View();
        }

        /*POST - Create*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.NameAndPhone = customer.Name + " " + customer.Phone;
                _db.Customer.Add(customer);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        /*DELETE*/
        public async Task<IActionResult> Delete (int? id)
        {
            var customer = await _db.Customer.FindAsync(id);
            if(customer == null)
            {
                return NotFound();
            }
            else
            {
                _db.Customer.Remove(customer);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        /*GET - Edit*/
        public async Task<IActionResult> Edit (int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var customer = await _db.Customer.FindAsync(id);
            if(customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        /*POST - Edit*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Customer customer)
        {
            if(ModelState.IsValid)
            {
                customer.NameAndPhone = customer.Name + " " + customer.Phone;
                _db.Update(customer);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        /*GET - View*/
        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = await _db.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
    }
}