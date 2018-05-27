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
    public class ApartmentsController : Controller
    {
        private string userID;
        private string userEmail;
        private string userName;
        private string hasApartment;
        private string hasOrders;

        private readonly DatabaseContext _context;

        public ApartmentsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Apartments/Create
        public IActionResult Create()
        {
            GetSessionData();
            if (userID == null)
            {
                return RedirectToAction("Index", "Home");
            }

            SetViewData();
            Apartment apartment = new Apartment();
            return PartialView("_CreateApartmentPartial", apartment);
        }

        // POST: Apartments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApartmentType,ApartmentArea,PropertyArea,CustomerID")] Apartment apartment)
        {
            GetSessionData();
            if (userID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                _context.Add(apartment);
                await _context.SaveChangesAsync();

                hasApartment = "true";
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

        // GET: Apartments/Edit/5
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
            var apartment = await _context.Apartments.SingleOrDefaultAsync(m => m.CustomerID == id);
            if (apartment == null)
            {
                return RedirectToAction("Index", "Home");
            }

            SetViewData();
            return PartialView("_EditApartmentPartial", apartment);
        }

        // POST: Apartments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ApartmentType,ApartmentArea,PropertyArea,CustomerID")] Apartment apartment)
        {
            GetSessionData();
            if (userID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id != apartment.ID)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apartment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartmentExists(apartment.ID))
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

        private bool ApartmentExists(int id)
        {
            return _context.Apartments.Any(e => e.ID == id);
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
