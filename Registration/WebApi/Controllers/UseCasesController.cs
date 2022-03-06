using Commands.Implementation;
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
            _logger.LogInformation("RegistrationController GetById running at: {time}, id: {id}", DateTimeOffset.Now, id);
            return Ok(await Mediator.Send(new GetUseCasesByIdQuery { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUseCasesCommand command)
        {
            _logger.LogInformation("RegistrationController Create running at: {time}", DateTimeOffset.Now);
            return Ok(await Mediator.Send(command));
        }
    }
}
