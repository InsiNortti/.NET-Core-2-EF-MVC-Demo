using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private string userID;
        private string userEmail;
        private string userName;
        private string hasApartment;
        private string hasOrders;

        private readonly DatabaseContext _context;

        public HomeController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            GetSessionData();
            if (userID != null)
            {
                SetViewData();
            }

            ModelState.Clear();
            return View();
        }

        // POST: Home/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var customers = from c in _context.Customers select c;
            var customer = await customers
                .Include(c => c.Apartment)
                .Include(c => c.Orders)
                .SingleOrDefaultAsync(c => c.Email == email && c.Password == password);

            if (customer == null)
            {
                return Json(new { status = "LOGIN FAILED" });
            }

            userID = customer.ID.ToString();
            userEmail = customer.Email;
            userName = customer.Name;
            if (customer.Apartment != null)
            {
                hasApartment = "true";
            } else
            {
                hasApartment = "false";
            }
            if (customer.Orders.Count() > 0)
            {
                hasOrders = "true";
            }
            else
            {
                hasOrders = "false";
            }

            SetSessionData();
            SetViewData();
            return Json(new { status = "OK" });
        }

        // POST: Home/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
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
