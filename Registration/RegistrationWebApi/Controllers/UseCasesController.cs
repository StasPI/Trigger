using Commands;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class RegistrationController : BaseController
    {
        private readonly ILogger<RegistrationController> _logger;
        public RegistrationController(ILogger<RegistrationController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("RegistrationController HttpGet running at Time: {time}, | Id: {id}", DateTimeOffset.Now, id);
            if (id <= 0) return BadRequest();
            return Ok(await Mediator.Send(new GetByIdUseCasesCommand { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostUseCasesCommand command)
        {
            _logger.LogInformation("RegistrationController HttpPost running at Time: {time}", DateTimeOffset.Now);
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("RegistrationController HttpDelete running at Time: {time}, | Id: {id}", DateTimeOffset.Now, id);
            if (id <= 0) return BadRequest();
            return Ok(await Mediator.Send(new DeleteUseCasesCommand { Id = id }));
        }

        [HttpPut]
        public async Task<IActionResult> Update(PutUseCasesCommand command)
        {
            _logger.LogInformation("RegistrationController HttpPut running at Time: {time}, | Id: {id}", DateTimeOffset.Now, command.Id);
            if (command.Id <= 0) return BadRequest();
            return Ok(await Mediator.Send(command));
        }
    }
}
