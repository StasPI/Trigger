using Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {

        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<Customer>> Get([FromRoute] long id)
        //{
        //    Repository<Customer, long> customerGet = new Repository<Customer, long>(_context);
        //    Customer cGet = await customerGet.GetAsync(id);
        //    if (cGet != null)
        //    {
        //        return Ok(cGet);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        [HttpPost("")]
        public async Task<IActionResult> Post(HttpContent command)//Post.Command command)
        {
            Console.WriteLine(command);
            var a = command;
            //Repository<Customer, long> customerCheck = new Repository<Customer, long>(_context);
            //Customer cCheck = await customerCheck.GetAsync(customerPost.Id);
            //if (cCheck == null)
            //{
            //    await _context.AddAsync(customerPost);
            //    await _context.SaveChangesAsync();
            //    return CreatedAtAction(nameof(Post), new { id = customerPost.Id }, customerPost);
            //}
            //else
            //{
            //    return Conflict();
            //}
            return Conflict();
        }

        //// PUT api/<ValuesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
