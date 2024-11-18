using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Crud_Test.Model;


namespace Crud_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        public readonly List <Person> _Person = new List<Person> ();

        [HttpGet]
        public ActionResult<IEnumerable<Person>> Get()
        {
            return _Person;
        }

        [HttpPost]
        public ActionResult Create (Person person)
        {
            _Person.Add(person);
            return Ok();
        }

        [HttpPut]
        public ActionResult Update(int id, Person person)
        {
            var exstingPerson = _Person.FirstOrDefault(x => x.Id == id);
            if (exstingPerson == null)
            {
                return NotFound();
            }
            exstingPerson.Name = person.Name;
            exstingPerson.Age = person.Age;
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var people = _Person.FirstOrDefault(x => x.Id == id);
            if (people == null)
            {
                return NotFound();  
            }
           
            _Person.Remove(people);
            return NoContent() ;    
        }
    }
}
