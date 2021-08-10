using BikeService.Actions;
using BikeService.Data;
using BikeService.Models;
using BikeService.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BikeService.Areas.Serviceman.Controllers
{
    [Area("Serviceman")]
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private int PageSize = 5;

        public RequestController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        /*GET - Index*/
        public async Task<IActionResult> Index (int? status, int requestPage = 1)
        {
            var requestsList = new RequestsWithPageNumbers
            {
                Requests = await _db.Requests.Include(r => r.Status)
                                             .Include(r => r.Customer)
                                             .Include(r => r.EmployeeAssignee)
                                             .Include(r => r.EmployeeReporter)
                                             .Include(r => r.Bike)
                                             .Where(r => r.StatusId == status)
                                             .OrderByDescending(r => r.PriorityLine).ThenBy(r => r.Date)
                                             .ToListAsync()
            };
            var count = requestsList.Requests.Count();
            requestsList.Requests = requestsList.Requests.Skip((requestPage - 1) * PageSize)
                                                         .Take(PageSize).ToList();
            requestsList.PagingInfo = new PagingInfo
            {
                CurrentPage = requestPage,
                RequestsPerPage = PageSize,
                TotalRequests = count,
                urlForPage = "/Serviceman/Request?status=" + status + "&requestPage=:"
            };
            return View(requestsList);
        
        }

        /*GET - Create*/
        public async Task<IActionResult> Create()
        {
            string tmpSymb = "";
            using (RequestAct rAct = new RequestAct(_db))
            {
                tmpSymb = rAct.GenerateTmpSymbol().ToString();
            }

            var model = new RequestWithPos()
            {
                Request = new Request(),
                Assignee = await _db.Employee.ToListAsync(),
                Reporter = await _db.Employee.ToListAsync(),
                ReuestSymbol = tmpSymb
            };
            return View(model);
        }

        /*POST - Create*/
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RequestWithPos NewRequest)
        {
            /*Sprawdzam czy dostałem dane klienta, który był już wcześniej w serwisie jeśli nie, to zakładam nowego w bazie*/
            int customerId;
            string customerIdStr = Request.Form["CustomerId"];
            string phoneNumber = Request.Form["Phone"];
            string emailAddress = Request.Form["Email"];
            string customerName = Request.Form["CustomerName"];
            string bicycleName = Request.Form["Bike"];
            if(Request.Form["PriorityLine"] == "true")
            {
                NewRequest.Request.PriorityLine = true;
            }
            var customer = new Customer();
            Int32.TryParse(customerIdStr, out customerId);
            if (customerId > 0)
            {
                /*Jeśli zmieniły się dane klienta, to je aktualizuję*/
                NewRequest.Request.CustomerId = customerId;
                customer = await _db.Customer.FindAsync(customerId);
                if ((customer != null) && (phoneNumber != customer.Phone || emailAddress != customer.EMail))
                {
                    customer.EMail = emailAddress;
                    customer.Phone = phoneNumber;
                    _db.Update(customer);
                    await _db.SaveChangesAsync();
                }
            }
            else
            {
                /*Dodaję nowego klienta w bazie*/
                //var customer = new Customer {
                //    Name = customerName,
                //    Phone = phoneNumber,
                //    EMail = emailAddress
                //};
                customer.Name = customerName.Trim();
                customer.Phone = phoneNumber;
                customer.EMail = emailAddress;
                customer.NameAndPhone = customer.Name + " " + customer.Phone;
                _db.Customer.Add(customer);
                await _db.SaveChangesAsync();
                NewRequest.Request.CustomerId = customer.Id;
            }

            if (!string.IsNullOrEmpty(bicycleName))
            {
                /*Dodaję nowy rower do bazy*/
                var bicycle = new Bike
                {
                    CustomerId = customer.Id,
                    Name = bicycleName
                };
                _db.Bike.Add(bicycle);
                await _db.SaveChangesAsync();
                NewRequest.Request.BikeId = bicycle.Id;
            }

            /*Przypisywanie właściwego symbolu*/
            string symb = "";
            using (RequestAct rAct = new RequestAct(_db))
            {
                symb = rAct.GenerateSymbol(DateTime.Now);
            }
            NewRequest.Request.Symbol = symb;

            /*Zapis do bazy danych*/
            if(!ModelState.IsValid)
            {
                return View(NewRequest);
            }
            _db.Requests.Add(NewRequest.Request);
            await _db.SaveChangesAsync();

            /*Zapisywanie dodanych zdjęć*/
            string webRootPath = _hostingEnvironment.WebRootPath;
            var images = HttpContext.Request.Form.Files;
            var savedRequest = await _db.Requests.FindAsync(NewRequest.Request.Id);
            string requestPeriod = savedRequest.Date.ToString("MMyyyy");
            int requestId = savedRequest.Id;
            int cnt = 0;
            if (images.Count > 0)
            {
                var uploads = Path.Combine(webRootPath, @"Images\Bikes\" + requestPeriod);
                if(!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                foreach(var file in images)
                {
                    var extension = Path.GetExtension(file.FileName);
                    string fileName = requestId.ToString() + '_' + cnt.ToString() + extension;
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    cnt++;

                    var image = new RequestImages()
                    {
                        RequestId = requestId,
                        Path = @"\Images\Bikes\" + requestPeriod + @"\" + fileName,
                        FileName = fileName
                    };
                    _db.RequestImages.Add(image);
                    await _db.SaveChangesAsync();
                }
            }

            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", new { status = 1 });
        }

        /*POST - Edit*/
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RequestWithPos EditedRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(EditedRequest);
            }
            var rek = EditedRequest.Request.Id;

            /*Sprawdzam czy dostałem dane klienta, który był już wcześniej w serwisie jeśli nie, to zakładam nowego w bazie*/
            int customerId;
            string customerIdStr = Request.Form["CustomerId"];
            string phoneNumber = Request.Form["Phone"];
            string emailAddress = Request.Form["Email"];
            string customerName = Request.Form["CustomerName"];
            string bicycleName = Request.Form["Bike"];
            if (Request.Form["PriorityLine"] == "true")
            {
                EditedRequest.Request.PriorityLine = true;
            }
            Int32.TryParse(customerIdStr, out customerId);
            if (customerId > 0)
            {
                /*Jeśli zmieniły się dane klienta, to je aktualizuję*/
                EditedRequest.Request.CustomerId = customerId;
                var customer = await _db.Customer.FindAsync(customerId);
                if (customer != null)
                {
                    if (phoneNumber != customer.Phone || emailAddress != customer.EMail)
                    {
                        customer.EMail = emailAddress;
                        customer.Phone = phoneNumber;
                        _db.Update(customer);
                        await _db.SaveChangesAsync();
                    }
                }
            }
            else
            {
                /*Dodaję nowego klienta w bazie*/
                var customer = new Customer
                {
                    Name = customerName,
                    Phone = phoneNumber,
                    EMail = emailAddress
                };
                customer.NameAndPhone = customer.Name + " " + customer.Phone;
                _db.Customer.Add(customer);
                await _db.SaveChangesAsync();
                EditedRequest.Request.CustomerId = customer.Id;

                /*Dodaję nowy rower do bazy*/
                var bicycle = new Bike
                {
                    CustomerId = customer.Id,
                    Name = bicycleName
                };
                _db.Bike.Add(bicycle);
                await _db.SaveChangesAsync();
            }

            /*Zapis do bazy danych*/
            _db.Update(EditedRequest.Request);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", new { status = EditedRequest.Request.StatusId });
        }

        /*GET - View*/
        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var request = await _db.Requests.Include(r => r.Status)
                                            .Include(r => r.Customer)
                                            .Include(r => r.EmployeeAssignee)
                                            .Include(r => r.EmployeeReporter)
                                            .SingleOrDefaultAsync(r => r.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            var requestWithPos = new RequestWithPos()
            {
                Request = request,
                Assignee = await _db.Employee.ToListAsync(),
                Reporter = await _db.Employee.ToListAsync(),
                Customer = request.Customer,
                Images = await _db.RequestImages.Where(i => i.RequestId == id).ToListAsync(),
                Bikes = await _db.Bike.Where(b => b.CustomerId == request.CustomerId).ToListAsync()
            };

            return View(requestWithPos);
        }

        /*GET - Edit*/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var request = await _db.Requests.Include(r => r.Status)
                                            .Include(r => r.Customer)
                                            .Include(r => r.EmployeeAssignee)
                                            .Include(r => r.EmployeeReporter)
                                            .SingleOrDefaultAsync(r => r.Id == id);
            if (request == null)
            {
                return NotFound();
            }
            var requestWithPos = new RequestWithPos()
            {
                Request = request,
                Assignee = await _db.Employee.ToListAsync(),
                Reporter = await _db.Employee.ToListAsync(),
                Customer = request.Customer,
                Images = await _db.RequestImages.Where(i => i.RequestId == id).ToListAsync(),
                Bikes = await _db.Bike.Where(b=> b.CustomerId == request.CustomerId).ToListAsync()
            };

            return View(requestWithPos);
        }

        /*DELETE*/
        public async Task<IActionResult> Delete(int? id)
        {
            var request = await _db.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }
            else
            {
                int requestStatus = request.StatusId;
                _db.Requests.Remove(request);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", new { status = requestStatus });
            }
        }

        /*___________________________________________________________________Dane dla JS_____________________________________________________________________*/

        [ActionName("GetCustomerName")]
        public IActionResult GetCustomerName(string nameAndPhone)
        {
            nameAndPhone = nameAndPhone.Trim();

            var customerData = new CustomerData
            {
                Customer = _db.Customer.FirstOrDefault(p => p.NameAndPhone == nameAndPhone)
            };

            List<Bike> bikes = _db.Bike
                                  .Where(b => b.CustomerId == customerData.Customer.Id)
                                  .OrderByDescending(b => b.Id)
                                  .Select(b => new Bike
                                  {
                                      Id = b.Id,
                                      Name = b.Name,
                                      CustomerId = b.CustomerId,
                                      Comment = b.Comment
                                  }).ToList();
            customerData.BikesList = bikes;

            return Json(customerData);
        }

        [ActionName("GetCustomerNameAndPhone")]
        public JsonResult GetCustomerNameAndPhone()
        {
            List<Customer> customers = _db.Customer.Select(c => new Customer {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
                NameAndPhone = c.NameAndPhone,
                EMail = c.EMail,
                Descript = c.Descript
            }).ToList();

            return Json(customers);
        }

        [ActionName("AddNewBicycle")]
        public JsonResult AddNewBicycle(string customer, string bicycle, string comment)
        {
            Int32.TryParse(customer, out int customerIdInt);
            if (customerIdInt > 0)
            {
                var bike = new Bike
                {
                    CustomerId = customerIdInt,
                    Name = bicycle,
                    Comment = comment
                };
                _db.Bike.Add(bike);
                _db.SaveChanges();
            }

            List<Bike> bikes = _db.Bike
                                  .Where(b => b.CustomerId == customerIdInt)
                                  .OrderByDescending(b => b.Id)
                                  .Select(b => new Bike
                                  {
                                      Id = b.Id,
                                      Name = b.Name,
                                      CustomerId = b.CustomerId,
                                      Comment = b.Comment
                                  }).ToList();

            return Json(bikes);
        }

        [ActionName("ChangeStatus")]
        public JsonResult ChangeStatus(string requestIdForm)
        {
            Int32.TryParse(requestIdForm, out int requestId);
            var request = new Request();
            var requestWithImages = new RequestWithPos();
            if (requestId > 0)
            {
                request = _db.Requests.Include(r => r.Status)
                                      .Include(r => r.Customer)
                                      .Include(r => r.EmployeeAssignee)
                                      .Include(r => r.EmployeeReporter)
                                      .SingleOrDefault(r => r.Id == requestId);
                requestWithImages.Assignee = _db.Employee.ToList();
                requestWithImages.Request = request;
                requestWithImages.Images = _db.RequestImages.Where(i => i.RequestId == request.Id).ToList();
            }

            return Json(requestWithImages);
        }

        [ActionName("ChangeStatusConfirm")]
        public void ChangeStatusConfirm(string requestIdForm, string assigneIdForm, string valueForm, string descriptForm, string statusOfRequestForm, string newStatusForm)
        {
            Int32.TryParse(statusOfRequestForm, out int statusOfRequest);
            Int32.TryParse(requestIdForm, out int requestId);
            Int32.TryParse(assigneIdForm, out int assigneId);
            Int32.TryParse(newStatusForm, out int newStatus);
            decimal.TryParse(valueForm, out decimal value);
            var request = new Request();
            if(requestId > 0)
            {
                request = _db.Requests.SingleOrDefault(r => r.Id == requestId);
                request.Assignee = assigneId;
                request.GrossValue = value;
                request.Description = descriptForm;
                request.StatusId = newStatus;
                _db.Update(request);
                _db.SaveChanges();
            }
        }

        [ActionName("SaveLastUpdatedRequest")]
        public void SaveLastUpdatedRequest(string requestIdForm)
        {
            AppConfig.LastUpdatedRequestSave(requestIdForm);
        }
    }
}
