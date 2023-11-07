using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace WebApplication3
{
   

    [ApiController]
    [Route("api/operations")]
    public class OperationsController : ControllerBase
    {
        public OperationsController(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }
        private static List<Operation> operations = new List<Operation>();
        private readonly ApplicationContext applicationContext;

        // GET api/operations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operation>>> GetOperations()
        {
            return await applicationContext.Operations.ToListAsync();
        }

        // GET api/operations/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Operation>> GetOperation(int id)
        {
            return await applicationContext.Operations.FindAsync(id);
        }

        // POST api/operations
        [HttpPost]
        public async Task<ActionResult<Operation>> CreateOperation(Operation operation)
        {
            /*operation.Id = operations.Count + 1;*/

            await applicationContext.Operations.AddAsync(operation);
            await applicationContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOperation), new { id = operation.Id }, operation);
        }
        
        // PUT api/operations/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Operation>> UpdateOperation(int id, Operation updatedOperation)
        {
            
            var operation = await applicationContext.Operations.FindAsync(id);

            operation.Name = updatedOperation.Name;
            operation.Description = updatedOperation.Description;
            
            applicationContext.Operations.Update(operation);
            await applicationContext.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetOperation), new { id = operation.Id }, operation);

        }

        // DELETE api/operations/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOperation(int id)
        {
            var op = await applicationContext.Operations.FindAsync(id);
             applicationContext.Operations.Remove(op);
             await applicationContext.SaveChangesAsync();
             return Ok();

        }
    }

    public sealed class ApplicationContext : DbContext
    {
        public DbSet<Operation> Operations { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }
    }
    
    
    public class Operation
    {

        [System.ComponentModel.DataAnnotations.Key]
        [Column("id")] 
        public int Id { get; set; }

        [Column("Name")] 
        public string Name { get; set; }

        [Column("Description")] 
        public string Description { get; set; }
        
    }

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