using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Http;

namespace FinalProject.Controllers
{
    public class CustomersController : Controller
    {
        private string userID;
        private string userEmail;
        private string userName;
        private string hasApartment;
        private string hasOrders;

        private readonly DatabaseContext _context;

        public CustomersController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            Customer customer = new Customer();
            return PartialView("_CreateCustomerPartial", customer);
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Address,BillingAddress,Phone,Email,Password,ConfirmPassword")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    return Json(new { status = "DUPLICATE" });
                }
                var savedCustomer = await _context.Customers.SingleOrDefaultAsync(c => c.Email == customer.Email);
                if (savedCustomer == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                userID = savedCustomer.ID.ToString();
                userEmail = savedCustomer.Email;
                userName = savedCustomer.Name;
                hasApartment = "false";
                hasOrders = "false";

                SetSessionData();
                SetViewData();
                return Json(new { status = "OK" });
            }

            return Json(new
            {
                status = "FAILED",
                errorMessages = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(k => k.Key, v => v.Value.Errors[0].ErrorMessage)
            });
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            GetSessionData();
            if (userID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var customer = await _context.Customers.SingleOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return RedirectToAction("Index", "Home");
            }

            SetViewData();
            return PartialView("_EditCustomerPartial", customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Address,BillingAddress,Phone,Email,Password,ConfirmPassword")] Customer customer)
        {
            GetSessionData();
            if (userID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id != customer.ID)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.ID))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        throw;
                    }
                }

                return Json(new { status = "OK" });
            }

            return Json(new
            {
                status = "FAILED",
                errorMessages = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(k => k.Key, v => v.Value.Errors[0].ErrorMessage)
            });
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.ID == id);
        }

        private void GetSessionData()
        {
            userID = HttpContext.Session.GetString(Constants.USER_ID);
            userEmail = HttpContext.Session.GetString(Constants.USER_EMAIL);
            userName = HttpContext.Session.GetString(Constants.USER_NAME);
            hasApartment = HttpContext.Session.GetString(Constants.HAS_APARTMENT);
            hasOrders = HttpContext.Session.GetString(Constants.HAS_ORDERS);
        }

        private void SetSessionData()
        {
            HttpContext.Session.SetString(Constants.USER_ID, userID);
            HttpContext.Session.SetString(Constants.USER_EMAIL, userEmail);
            HttpContext.Session.SetString(Constants.USER_NAME, userName);
            HttpContext.Session.SetString(Constants.HAS_APARTMENT, hasApartment);
            HttpContext.Session.SetString(Constants.HAS_ORDERS, hasOrders);
        }

        private void SetViewData()
        {
            ViewData[Constants.USER_ID] = userID;
            ViewData[Constants.USER_EMAIL] = userEmail;
            ViewData[Constants.USER_NAME] = userName;
            ViewData[Constants.HAS_APARTMENT] = hasApartment;
            ViewData[Constants.HAS_ORDERS] = hasOrders;
        }

    }
}
