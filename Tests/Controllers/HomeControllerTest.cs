using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using System;
using System.Linq;
using UnitTesting.Controllers;
using UnitTesting.Data;
using Xunit;

namespace UnitTesting.Tests.Controllers
{
    public class HomeControllerTest
    {
        private MyDbContext _context;
        private HomeController _controller;

        public HomeControllerTest()
        {
            // Initialize DbContext in memory
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseInMemoryDatabase();
            _context = new MyDbContext(optionsBuilder.Options);

            // Seed data
            _context.People.Add(new Person()
            {
                FirstName = "John",
                LastName = "Doe"
            });
            _context.People.Add(new Person()
            {
                FirstName = "Sally",
                LastName = "Doe"
            });
            _context.SaveChanges();

            // Create test subject
            _controller = new HomeController(_context);
        }

        [Fact]
        public void Get_person_john_returns_john()
        {
            // Act
            var result = _controller.GetPerson("John") as ViewResult;

            // Assert
            Assert.IsType(typeof(Person), result.ViewData.Model);
            Person model = (Person)result.ViewData.Model;
            Assert.Equal("John", model.FirstName);
            Assert.Equal("Doe", model.LastName);
        }

        [Fact]
        public void Get_non_existent_person_returns_null()
        {
            // Act
            var result = _controller.GetPerson("Fred") as ViewResult;

            // Assert
            Assert.Null(result.ViewData.Model);
        }

        [Fact]
        public void Add_person_saves_to_db_with_generated_id()
        {
            // Arrange
            Guid personId = Guid.NewGuid();
            Person person = new Person()
            {
                Id = personId,
                FirstName = "Billy",
                LastName = "McBill"
            };
            var beforePersonCount = _context.People.Count();

            // Act
            var result = _controller.AddPerson(person) as HttpStatusCodeResult;

            // Assert
            Assert.Equal(200, result.StatusCode);
            Person savedPerson = _context.People.Single(x => x.FirstName == "Billy"
                                                             && x.LastName == "McBill");
            Assert.NotEqual(personId, savedPerson.Id);
            Assert.Equal(beforePersonCount + 1, _context.People.Count());
        }

    }
}
