using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using UnitTesting.Data;

namespace UnitTesting.Controllers
{
    public class HomeController : Controller
    {
        private MyDbContext _context;

        public HomeController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult GetPerson(string name)
        {
            Person person = _context.People.FirstOrDefault(x => x.FirstName == name);
            return View(person);
        }

        [HttpPost]
        public IActionResult AddPerson(Person person)
        {
            // Let db assign id
            person.Id = Guid.Empty;
            _context.Add(person);
            _context.SaveChanges();
            return Ok();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
