using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApplication3
{
   

    [ApiController]
    [Route("api/operations")]
    public class OperationsController : ControllerBase
    {
        private static List<Operation> operations = new List<Operation>();

        // GET api/operations
        [HttpGet]
        public ActionResult<IEnumerable<Operation>> GetOperations()
        {
            return Ok(operations);
        }

        // GET api/operations/{id}
        [HttpGet("{id}")]
        public ActionResult<Operation> GetOperation(int id)
        {
            var operation = operations.Find(op => op.Id == id);
            if (operation == null)
                return NotFound();

            return Ok(operation);
        }

        // POST api/operations
        [HttpPost]
        public ActionResult<Operation> CreateOperation(Operation operation)
        {
            operation.Id = operations.Count + 1;
            operations.Add(operation);
            return CreatedAtAction(nameof(GetOperation), new { id = operation.Id }, operation);
        }

        // PUT api/operations/{id}
        [HttpPut("{id}")]
        public ActionResult<Operation> UpdateOperation(int id, Operation updatedOperation)
        {
            var operation = operations.Find(op => op.Id == id);
            if (operation == null)
                return NotFound();

            operation.Name = updatedOperation.Name;
            operation.Description = updatedOperation.Description;
            return Ok(operation);
        }

        // DELETE api/operations/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteOperation(int id)
        {
            var operation = operations.Find(op => op.Id == id);
            if (operation == null)
                return NotFound();

            operations.Remove(operation);
            return NoContent();
        }
    }

    public class Operation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class StatusCodeException : Exception
    {
        public StatusCodeException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; set; }
    }

    [Route("api/operations")]
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}