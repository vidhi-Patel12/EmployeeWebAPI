using EmployeeWebAPI.Controllers;
using EmployeeWebAPI.Interface;
using EmployeeWebAPI.Models;
using EmployeeWebAPI.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EmployeeWebAPI.Tests
{
    [TestClass]
    public class EmployeeControllerTests
    {
        [TestMethod]
        public async Task Get_ReturnsEmployeeDetails()
        {
            // Arrange
            var mockEmployeeService = new Mock<IEmployee>();
            mockEmployeeService.Setup(service => service.GetEmployeeDetails())
                               .Returns(GetTestEmployees());

            var controller = new EmployeeController(mockEmployeeService.Object, null, null);

            // Act
            ActionResult<IEnumerable<EmployeeClass>> result = await controller.Get();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<EmployeeClass>>));
            var employees = (result as ActionResult<IEnumerable<EmployeeClass>>).Value;
            Assert.IsNotNull(employees);
            Assert.AreEqual(2, employees.Count());

        }

        private List<EmployeeClass> GetTestEmployees()
        {
            var employees = new List<EmployeeClass>
            {
                new EmployeeClass { EmployeeId = 1, EmployeeName = "John Doe" },
                new EmployeeClass { EmployeeId = 2, EmployeeName = "Jane Doe" }
            };
            return employees;
        }

        [TestMethod]
        public async Task PostValidEmployeeReturnsOkResult()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var dbContext = new DatabaseContext(dbContextOptions);

            var mockEmployeeService = new Mock<IEmployee>();
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            mockEnvironment.Setup(env => env.WebRootPath).Returns("wwwroot");

            var controller = new EmployeeController(mockEmployeeService.Object, mockEnvironment.Object, dbContext);

            var employeeViewModel = new EmployeeViewModel
            {
                EmployeeName = "John Doe",
                BirthDate = "2024-06-10",
                Gender = "Male",
                Email = "john.doe@example.com",
                Password = "password",
                Image = GetTestFile()
            };

            // Act
            var result = await controller.Post(employeeViewModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(EmployeeClass));
        }

        private IFormFile GetTestFile()
        {
            var fileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.png";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(ms.Length);
            fileMock.Setup(f => f.ContentType).Returns("image/png");

            fileMock.Setup(f => f.CopyTo(It.IsAny<Stream>())).Callback<Stream>(stream =>
            {
                ms.Position = 0;
                ms.CopyTo(stream);
            });

            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>())).Returns((Stream stream, CancellationToken token) =>
            {
                ms.Position = 0;
                return ms.CopyToAsync(stream);
            });

            return fileMock.Object;
        }


        [TestMethod]
        public async Task PutValidEmployeeReturnsOkResult()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var dbContext = new DatabaseContext(dbContextOptions);

            // Seed an employee to update
            var existingEmployee = new EmployeeClass
            {
                EmployeeName = "Jane Doe",
                BirthDate = "2020-01-01",
                Gender = "Female",
                Email = "jane.doe@example.com",
                Password = "password",
                Image = "existing.png"
            };
            dbContext.Employee.Add(existingEmployee);
            await dbContext.SaveChangesAsync();

            var mockEmployeeService = new Mock<IEmployee>();
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            mockEnvironment.Setup(env => env.WebRootPath).Returns("wwwroot");

            var controller = new EmployeeController(mockEmployeeService.Object, mockEnvironment.Object, dbContext);

            var employeeViewModel = new EmployeeViewModel
            {
                //EmployeeId = existingEmployee.Id,
                EmployeeName = "John Doe",
                BirthDate = "2024-06-10",
                Gender = "Male",
                Email = "john.doe@example.com",
                Password = "password",
                Image = GeteditTestFile()
            };

            // Act
            var result = await controller.Put(existingEmployee.EmployeeId, employeeViewModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(EmployeeClass));
        }

        private IFormFile GeteditTestFile()
        {
            var fileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.png";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(ms.Length);
            fileMock.Setup(f => f.ContentType).Returns("image/png");

            fileMock.Setup(f => f.CopyTo(It.IsAny<Stream>())).Callback<Stream>(stream =>
            {
                ms.Position = 0;
                ms.CopyTo(stream);
            });

            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>())).Returns((Stream stream, CancellationToken token) =>
            {
                ms.Position = 0;
                return ms.CopyToAsync(stream);
            });

            return fileMock.Object;
        }


        [TestMethod]
        public async Task DeleteValidEmployeeReturnsOkResult()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            using (var context = new DatabaseContext(options))
            {
                // Ensure context is properly initialized and not null
                if (context == null)
                {
                    Assert.Fail("Failed to initialize database context");
                }

                var mockEmployeeService = new Mock<IEmployee>();
                var webHostEnvironment = new Mock<IWebHostEnvironment>();
                var controller = new EmployeeController(mockEmployeeService.Object, webHostEnvironment.Object, context);

                // Arrange: Create a new employee to delete
                var existingEmployee = new EmployeeClass
                {
                    EmployeeId = 1,
                    EmployeeName = "Jane Doe",
                    BirthDate = "2020-01-01",
                    Gender = "Female",
                    Email = "jane.doe@example.com",
                    Password = "password",
                    Image = "existing.png"
                };

                mockEmployeeService.Setup(s => s.DeleteEmployee(existingEmployee.EmployeeId))
                                   .Returns(existingEmployee);

                // Act: Call the DeleteEmployee method to delete the employee
                var result = await controller.Delete(existingEmployee.EmployeeId);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ActionResult<EmployeeClass>));
                var actionResult = result as ActionResult<EmployeeClass>;
                Assert.IsNotNull(actionResult); 
                Assert.IsInstanceOfType(actionResult.Value, typeof(EmployeeClass));
                var deletedEmployee = actionResult.Value as EmployeeClass;
                Assert.AreEqual(existingEmployee.EmployeeId, deletedEmployee.EmployeeId);
            }
        }
    }
}

