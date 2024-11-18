using Crud_Test.Data;
using Crud_Test.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Crud_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonDBController : ControllerBase
    {
        private readonly YourDbContext _context;

        public PersonDBController(YourDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> GetPeople()
        {
            return await _context.People.ToListAsync();
        }

        [HttpGet("id")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeopleById(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        [HttpPost]
        public async Task<ActionResult> PostPerson(Person person)
        {
            try
            {


                if (_context.People.Any(p => p.Id == person.Id))
                {
                    return Conflict(new { message = "Person already exist" });

                }

                _context.People.Add(person);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetPeople), new { id = person.Id }, person);
            }
            catch (DbUpdateException ex)
            {
                // Handle database update-specific exceptions for more informative error handling
                return StatusCode(500, new { message = "An error occurred while saving to the database.", details = ex.Message });
            }
            catch (Exception ex)
            {
                // Catch any other general exceptions and return a BadRequest response with the exception message
                return BadRequest(new { message = "An error occurred while processing your request.", details = ex.Message });
            }

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            // Ensure the ID in the route matches the ID in the request body
            if (id != person.Id)
            {
                return BadRequest(new { message = "The ID in the URL does not match the ID in the request body." });
            }

            // Attach the entity and mark it as modified
            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(new
                {
                    message = "The record you attempted to update was modified or deleted by another user.",
                    details = ex.Message
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while saving to the database.",
                    details = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred while processing your request.",
                    details = ex.Message
                });
            }

            return NoContent();

        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.People.FindAsync(id);

            if (person == null)
            {
                return NotFound(); // Returns 404 if the person doesn't exist
            }

            _context.People.Remove(person); // Remove the person from the DbContext
            await _context.SaveChangesAsync(); // Save the changes to the database

            return NoContent(); // Returns 204 No Content to indicate successful deletion
        }
    }
}
