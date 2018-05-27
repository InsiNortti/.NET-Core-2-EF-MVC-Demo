using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Http;

namespace FinalProject.Controllers
{
    public class OrdersController : Controller
    {
        private string userID;
        private string userEmail;
        private string userName;
        private string hasApartment;
        private string hasOrders;

        private readonly DatabaseContext _context;

        public OrdersController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            GetSessionData();
            if (userID == null)
            {
                return RedirectToAction("Index", "Home");
            }

            SetViewData();
            var orders = await _context.Orders.Where(o => o.CustomerID == Int32.Parse(userID)).ToListAsync();
            return View(orders);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            GetSessionData();
            if (userID == null)
            {
                return RedirectToAction("Index", "Home");
            }

            SetViewData();
            Order order = new Order();
            return PartialView("_CreateOrderPartial", order);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("State,Description,OrderDate,CustomerID")] Order order)
        {
            GetSessionData();
            if (userID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();

                hasOrders = "true";
                SetSessionData();
                return Json(new { status = "OK" });
            }

            return Json(new
            {
                status = "FAILED",
                errorMessages = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(k => k.Key, v => v.Value.Errors[0].ErrorMessage)
            });
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
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

            SetViewData();
            var order = await _context.Orders.SingleOrDefaultAsync(o => o.ID == id);
            return PartialView("_DetailsOrderPartial", order);
        }

        // GET: Orders/Edit/5
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
            var order = await _context.Orders.SingleOrDefaultAsync(o => o.ID == id);
            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (order.State != State.TILATTU)
            {
                return RedirectToAction("Index");
            }

            SetViewData();
            return PartialView("_EditOrderPartial", order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Description,OrderDate,CustomerID")] Order order)
        {
            if (id != order.ID)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.ID))
                    {
                        return NotFound();
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

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var order = await _context.Orders.SingleOrDefaultAsync(o => o.ID == id);
            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return PartialView("_DeleteOrderPartial", order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(o => o.ID == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return Json(new { status = "OK" });
        }

        // POST: Orders/Discard/5
        public async Task<IActionResult> Discard(int id)
        {
            GetSessionData();
            if (userID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var order = await _context.Orders.SingleOrDefaultAsync(o => o.ID == id);
            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            }
            order.State = State.HYLÄTTY;
            order.CancelDate = DateTime.Today;
            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.ID))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Orders/Accept/5
        public async Task<IActionResult> Accept(int id)
        {
            GetSessionData();
            if (userID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var order = await _context.Orders.SingleOrDefaultAsync(o => o.ID == id);
            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            }
            order.State = State.HYVÄKSYTTY;
            order.AcceptDate = DateTime.Today;
            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.ID))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Index");
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.ID == id);
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
