using Commands.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class RegistrationController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetUseCasesByIdQuery { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUseCasesCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
